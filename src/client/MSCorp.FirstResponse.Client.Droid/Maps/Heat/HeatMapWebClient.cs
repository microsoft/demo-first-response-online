using Android.Webkit;

namespace MSCorp.FirstResponse.Client.Droid.Maps.Heat
{
    internal class HeatMapWebClient : WebViewClient
    {
        private readonly IHeatMapLayerWebClientCallback _callback;

        public HeatMapWebClient(IHeatMapLayerWebClientCallback callback)
        {
            _callback = callback;
        }

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            return true;
        }

        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);

            _callback?.OnPageLoaded();
        }

        public override void OnReceivedError(WebView view, IWebResourceRequest request, WebResourceError error)
        {
            base.OnReceivedError(view, request, error);

            System.Diagnostics.Debug.WriteLine("HeatMapWebClient error: " + error.Description);
        }
    }
}