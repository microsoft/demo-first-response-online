using CoreGraphics;
using CoreLocation;
using Foundation;
using MapKit;
using MSCorp.FirstResponse.Client.Extensions;
using MSCorp.FirstResponse.Client.Maps.Heat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UIKit;
using WebKit;
using Xamarin.Forms.Maps;

namespace MSCorp.FirstResponse.Client.iOS.Maps.Heat
{
    public class HeatMapLayer : UIView, IWKScriptMessageHandler, INavigationDelegateCallback
    {
        private Map _formsMap;
        private WKWebView _baseWebView;
        private MKMapView _map;
        private UIImageView _img;
        private WKUserContentController _userController;

		private bool _isLoading;
        private double _radius = 10000;
        private double _intensity = 0.5;
        private int _minRadius = 2;
        private IEnumerable<CLLocationCoordinate2D> _locations;

        public HeatMapLayer(Map map)
        {
            _formsMap = map;
			_isLoading = true;
        }

        public IEnumerable<CLLocationCoordinate2D> Locations
        {
            get { return _locations; }
            set
            {
                _locations = value;
                Render();
            }
        }

        public MKMapView ParentMap
        {
            get { return _map; }
            set
            {
                _map = value;
                AttachMapEvents();
            }
        }

        public double Intensity
        {
            get { return _intensity; }
            set
            {
                _intensity = Math.Min(1, Math.Max(0, value));
                SetOptions();
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

        private void AttachMapEvents()
        {
            DetachMapEvents();

            _map.RegionChanged += OnMapRegionChanged;
        }

        private void DetachMapEvents()
        {
            _map.RegionChanged -= OnMapRegionChanged;
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);

            if (newsuper != null)
            {
                CreateWebView(newsuper);
                CreateImageView(newsuper);

                AddSubview(_img);
                AddSubview(_baseWebView);
                LoadWebView();
            }
        }

        public override void RemoveFromSuperview()
        {
            base.RemoveFromSuperview();

            _baseWebView = null;
            _img = null;
            _userController.RemoveAllUserScripts();
        }

        public void OnPageLoaded()
        {
            _isLoading = false;
            LoadHeatMap();
            ResizeHeatMap();
            Render();
        }

        public override bool PointInside(CGPoint point, UIEvent uievent)
        {
            return false;
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            try
            {
                var data = message.Body.ToString();
                var imageBytes = HeatMapHelper.GetHeatMapImageBytes(data);

                NSData imageData = NSData.FromArray(imageBytes);
                UIImage image = UIImage.LoadFromData(imageData);

                _img.Image = image;
                _img.Hidden = false;
            }
            catch (System.Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error decoding heat map image: {e}");
            }
        }

        private void LoadWebView()
        {
            string path = Path.Combine(NSBundle.MainBundle.BundlePath, "HeatMap.html");
            var contents = File.ReadAllText(path);
            var baseUrl = new NSUrl(NSBundle.MainBundle.BundlePath, true);
            _baseWebView.LoadHtmlString(contents, baseUrl);
        }

        private void CreateImageView(UIView newsuper)
        {
            _img = new UIImageView(newsuper.Frame);
            _img.ContentMode = UIViewContentMode.ScaleAspectFill;
            _img.BackgroundColor = UIColor.FromRGBA(0xFF, 0x00, 0x00, 0x08);
        }

        private void CreateWebView(UIView newsuper)
        {
            _userController = new WKUserContentController();
            _userController.AddScriptMessageHandler(this, "jsBridge");
            var preferences = new WKPreferences
            {
                JavaScriptEnabled = true
            };

            var configuration = new WKWebViewConfiguration
            {
                UserContentController = _userController,
                Preferences = preferences,
            };

            _baseWebView = new WKWebView(newsuper.Frame, configuration);
            _baseWebView.Hidden = true;
            _baseWebView.NavigationDelegate = new CustomNavigationDelegate(this);
        }

        private void Render()
        {
            if (_baseWebView == null)
            {
                return;
            }

            var locationsStr = Locations?.Select(c => GetCoordinateProjection(c, _map))
                                         .Select(l => $"{l.X},{l.Y}")
                                         .Aggregate((x, y) => $"{x}|{y}");

			var zoom = _formsMap.GetZoomLevel();
            var renderParameters = HeatMapHelper.GetRenderParameters(locationsStr, _radius, _minRadius, zoom + 2);

            ExecuteScript($"Render('{renderParameters.Locations}', '{renderParameters.Radius}', '{renderParameters.Zoom}');");
        }

        private void ExecuteScript(string script)
        {
			if (_isLoading)
			{
				return;
			}

            try
            {
                _baseWebView.EvaluateJavaScript(script, null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error executing script in HeatMapLayer: {ex}");
            }
        }

        private CGPoint GetCoordinateProjection(CLLocationCoordinate2D coordinate, MKMapView map)
        {
            CGPoint point = _map.ConvertCoordinate(coordinate, map);

            return point;
        }

        private void ResizeHeatMap()
        {
            if (_baseWebView == null)
            {
                return;
            }

            ExecuteScript($"Resize('{Frame.Width}', '{Frame.Height}');");
        }

        private void LoadHeatMap()
        {
            if (_baseWebView == null)
            {
				return;
            }

            var options = HeatMapHelper.GetOptionsJson(_intensity);
            ExecuteScript($"LoadHeatMap(\"{options}\");");
        }

        private void SetOptions()
        {
            if (_baseWebView == null)
            {
                return;
            }

            var options = HeatMapHelper.GetOptionsJson(_intensity);
            ExecuteScript($"SetOptions(\"{options}\", 'true');");
        }

        private void OnMapRegionChanged(object sender, MKMapViewChangeEventArgs e)
        {
            Render();
        }
    }
}