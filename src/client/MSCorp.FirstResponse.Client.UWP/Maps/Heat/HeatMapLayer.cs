using MSCorp.FirstResponse.Client.Extensions;
using MSCorp.FirstResponse.Client.Maps.Heat;
using System;
using System.Linq;
using System.Threading;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms.Maps;

namespace MSCorp.FirstResponse.Client.UWP.Maps.Heat
{
    public class HeatMapLayer : Grid
    {
        #region Private Properties

        private WebView _baseWebView;
        private Geopath _locations;
        private Geopoint _center = new Geopoint(new BasicGeoposition() { Latitude = 0, Longitude = 0 });
        private Uri _pageUri = new Uri("ms-appx-web:///Assets/HeatMap.html", UriKind.Absolute);

        private Map _formsMap;
        private MapControl _map;

        private int _viewChangeEndWaitTime = 100;

        private double _radius = 10000;
        private double _intensity = 0.5;
        private int _minRadius = 2;

        private bool _isLoaded, _isMoving;
        private bool _zoomed, _panned;

        private DateTime _lastMovement;
        private Timer _updateTimer;

        private Image _img;

        #endregion

        #region Constructor

        public HeatMapLayer(Map map)
            : base()
        {
            _formsMap = map;
            HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;

            _baseWebView = new WebView()
            {
                Name = "BaseWebView",
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch,
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch,
                DefaultBackgroundColor = Windows.UI.Colors.Transparent
            };

            _baseWebView.NavigationCompleted += OnNavigationCompleted;
            _baseWebView.ScriptNotify += BaseWebViewScriptNotify;
            _baseWebView.Navigate(_pageUri);

            Children.Add(_baseWebView);

            _img = new Image();
            Children.Add(_img);
        }

        #endregion

        #region Public Properties

        public double Intensity
        {
            get { return _intensity; }
            set
            {
                _intensity = Math.Min(1, Math.Max(0, value));
                SetOptions();
            }
        }

        public Geopath Locations
        {
            get { return _locations; }
            set
            {
                _locations = value;
                Render();
            }
        }

        public MapControl ParentMap
        {
            get { return _map; }
            set
            {
                _map = value;
                AttachMapEvents();
            }
        }

        public double Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
            }
        }

        #endregion

        #region Private Methods

        #region Map Event Handlers

        private void DetachMapEvents()
        {
            _map.ZoomLevelChanged -= OnMapZoomLevelChanged;
            _map.CenterChanged -= OnMapCenterChanged;
            _map.SizeChanged -= OnMapSizeChanged;
        }

        private void AttachMapEvents()
        {
            Width = _map.ActualWidth;
            Height = _map.ActualHeight;

            DetachMapEvents();

            _map.ZoomLevelChanged += OnMapZoomLevelChanged;
            _map.CenterChanged += OnMapCenterChanged;
            _map.SizeChanged += OnMapSizeChanged;

            if (_updateTimer != null)
            {
                _updateTimer.Dispose();
            }

            _updateTimer = new Timer(async (s) =>
            {
                try
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        if ((_panned || _zoomed) && !_isMoving && (DateTime.Now - _lastMovement).Milliseconds < _viewChangeEndWaitTime)
                        {
                            Render();

                            _panned = false;
                            _zoomed = false;
                        }
                    });
                }
                catch { }
            }, null, 0, _viewChangeEndWaitTime);
        }

        private void OnMapCenterChanged(object sender, object e)
        {
            _img.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            if (_center.Position.Latitude != _map.Center.Position.Latitude || _center.Position.Longitude != _map.Center.Position.Longitude)
            {
                _panned = true;
                _center = _map.Center;
            }
            else
            {
                _panned = false;
            }

            _lastMovement = DateTime.Now;
            _isMoving = false;
        }

        private void OnMapZoomLevelChanged(object sender, object e)
        {
            _img.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            _zoomed = true;
            _lastMovement = DateTime.Now;
            _isMoving = false;
        }

        private void OnMapSizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            Width = _map.ActualWidth;
            Height = _map.ActualHeight;

            ResizeHeatMap();
        }

        #endregion

        private void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            _baseWebView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            _isLoaded = true;
            LoadHeatMap();
            ResizeHeatMap();
            Render();
        }

        private async void BaseWebViewScriptNotify(object sender, NotifyEventArgs e)
        {
            try
            {
                var bmp = new BitmapImage();
                var imageBytes = HeatMapHelper.GetHeatMapImageBytes(e.Value);

                using (var ms = new Windows.Storage.Streams.InMemoryRandomAccessStream())
                {
                    using (var writer = new Windows.Storage.Streams.DataWriter(ms.GetOutputStreamAt(0)))
                    {
                        writer.WriteBytes(imageBytes);
                        await writer.StoreAsync();
                    }

                    await bmp.SetSourceAsync(ms);
                }

                imageBytes = null;
                _img.Source = bmp;
                _img.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            catch { }
        }

        #region Heat Map Methods

        private void LoadHeatMap()
        {
            var options = HeatMapHelper.GetOptionsJson(_intensity);
            InvokeJS("LoadHeatMap", new string[] { options });
        }

        private void ResizeHeatMap()
        {
            InvokeJS("Resize", new string[] { Width + "", Height + "" });
        }

        private void Render()
        {
            try
            {
                if (_isLoaded && _map != null)
                {
                    var locationsStr = Locations?.Positions.Select(c =>
                    {
                        Windows.Foundation.Point p;

                        _map.GetOffsetFromLocation(new Geopoint(c), out p);

                        return p;
                    })
                    .Select(l => $"{l.X},{l.Y}")
                    .Aggregate((x, y) => $"{x}|{y}");

                    var zoom = _formsMap.GetZoomLevel();
                    var renderParameters = HeatMapHelper.GetRenderParameters(locationsStr, _radius, _minRadius, zoom + 2);
                    string[] args = { renderParameters.Locations, renderParameters.Radius, renderParameters.Zoom };

                    InvokeJS("Render", args);
                }
            }
            catch { }
        }

        private void SetOptions()
        {
            var options = HeatMapHelper.GetOptionsJson(_intensity);
            InvokeJS("SetOptions", new string[] { options, "true" });
        }

        #endregion

        private async void InvokeJS(string methodName, string[] args)
        {
            try
            {
                if (_isLoaded)
                {
                    await Dispatcher.RunAsync(CoreDispatcherPriority.High, async () =>
                    {
                        await _baseWebView.InvokeScriptAsync(methodName, args);
                    });
                }
            }
            catch
            {
                // Ignored
            }
        }

        #endregion
    }
}