using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Webkit;
using Android.Widget;
using Java.Interop;
using MSCorp.FirstResponse.Client.Extensions;
using MSCorp.FirstResponse.Client.Maps.Heat;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Maps;

namespace MSCorp.FirstResponse.Client.Droid.Maps.Heat
{
    public class HeatMapLayer : LinearLayout, IHeatMapLayerWebClientCallback
    {
        private const string HeatMapDocumentPath = "file:///android_asset/HeatMap.html";

        #region Private Properties

        private WebView _baseWebView;
        private Map _formsMap;
        private GoogleMap _map;
        private ImageView _img;
        private double _radius = 10000;
        private double _intensity = 0.5;
        private int _minRadius = 2;
        private IEnumerable<LatLng> _locations;

        #endregion

        #region Constructor

        public HeatMapLayer(Context context, Map map) 
            : base(context)
        {
            _formsMap = map;
            _baseWebView = new WebView(context);
            _baseWebView.SetWebViewClient(new HeatMapWebClient(this));
            _baseWebView.Settings.JavaScriptEnabled = true;
            _baseWebView.Settings.AllowUniversalAccessFromFileURLs = true;
            _baseWebView.Visibility = Android.Views.ViewStates.Invisible;
            _baseWebView.AddJavascriptInterface(this, "jsBridge");
            WebView.SetWebContentsDebuggingEnabled(true);

            _baseWebView.LoadUrl(HeatMapDocumentPath);

            _img = new ImageView(context);

            AddView(_img);
        }

        #endregion

        public IEnumerable<LatLng> Locations
        {
            get { return _locations; }
            set
            {
                _locations = value;
                Render();
            }
        }

        public GoogleMap ParentMap
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

        public void OnPageLoaded()
        {
            LoadHeatMap();
            ResizeHeatMap();
            Render();
        }

        // JS interface
        [Export]
        [JavascriptInterface]
        public void HeatMapRenderCallback(string base64img)
        {
            var activity = Xamarin.Forms.Forms.Context as Activity;

            activity?.RunOnUiThread(async () =>
            {
                try
                {
                    var imageBytes = HeatMapHelper.GetHeatMapImageBytes(base64img);
                    Bitmap decodedByte = await BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length);

                    _img.SetImageBitmap(decodedByte);
                    _img.Visibility = Android.Views.ViewStates.Visible;
                }
                catch (System.Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"Error decoding heat map image: {e}");
                }
            });
        }

        private void AttachMapEvents()
        {
            DetachMapEvents();

            _map.CameraChange += OnMapCameraChange;
        }

        private void OnMapCameraChange(object sender, GoogleMap.CameraChangeEventArgs e)
        {
            _img.Visibility = Android.Views.ViewStates.Invisible;
            Render();
            _img.Visibility = Android.Views.ViewStates.Visible;
        }

        private void DetachMapEvents()
        {
            _map.CameraChange -= OnMapCameraChange;
        }

        private void Render()
        {
            var locationsStr = Locations?.Select(c => LatLngToPoint(c, _map))
                                         .Select(l => $"{l.X},{l.Y}")
                                         .Aggregate((x, y) => $"{x}|{y}");

            var zoom = _formsMap.GetZoomLevel();
            var renderParameters = HeatMapHelper.GetRenderParameters(locationsStr, _radius, _minRadius, zoom + 3);

            _baseWebView.EvaluateJavascript($"Render('{renderParameters.Locations}', '{renderParameters.Radius}', '{renderParameters.Zoom}');", null);
        }

        private void ResizeHeatMap()
        {
            _baseWebView.EvaluateJavascript($"Resize('{Width}', '{Height}');", null);
        }

        private void LoadHeatMap()
        {
            var options = HeatMapHelper.GetOptionsJson(_intensity);
            _baseWebView.EvaluateJavascript($"LoadHeatMap(\"{options}\");", null);
        }

        private void SetOptions()
        {
            var options = HeatMapHelper.GetOptionsJson(_intensity);
            _baseWebView.EvaluateJavascript($"SetOptions(\"{options}\", 'true');", null);
        }

        private Point LatLngToPoint(LatLng geoposition, GoogleMap map)
        {
            Point point = _map.Projection.ToScreenLocation(geoposition);

            return point;
        }
    }
}