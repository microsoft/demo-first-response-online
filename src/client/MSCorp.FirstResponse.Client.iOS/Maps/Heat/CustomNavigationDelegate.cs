using Foundation;
using WebKit;

namespace MSCorp.FirstResponse.Client.iOS.Maps.Heat
{
    internal class CustomNavigationDelegate : WKNavigationDelegate
    {
        private readonly INavigationDelegateCallback _callback;

        public CustomNavigationDelegate(INavigationDelegateCallback callback)
        {
            _callback = callback;
        }

        public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
        {
            _callback?.OnPageLoaded();
        }

        public override void DidFailNavigation(WKWebView webView, WKNavigation navigation, NSError error)
        {
        }
    }
}