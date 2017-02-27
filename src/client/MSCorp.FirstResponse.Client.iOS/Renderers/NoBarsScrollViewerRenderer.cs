using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NoBarsScrollViewer), typeof(NoBarsScrollViewerRenderer))]
namespace MSCorp.FirstResponse.Client.iOS.Renderers
{
    public class NoBarsScrollViewerRenderer : ScrollViewRenderer
	{
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || this.Element == null)
			{
				return;
			}

			ShowsHorizontalScrollIndicator = false;
			ShowsVerticalScrollIndicator = false;
		}
	}
}
