// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MSCorp.FirstResponse.Client.iOS
{
    [Register ("MapIncidentInfoView")]
    partial class MapIncidentInfoView
    {
        [Outlet]
        UIKit.UILabel DescriptionLabel { get; set; }


        [Outlet]
        UIKit.UILabel DescriptionTitleLabel { get; set; }


        [Outlet]
        UIKit.UIImageView DialogContainer { get; set; }


        [Outlet]
        UIKit.UIView Header { get; set; }


        [Outlet]
        UIKit.UIButton HeaderCloseButton { get; set; }


        [Outlet]
        UIKit.UIImageView HeaderIconImage { get; set; }


        [Outlet]
        UIKit.UILabel HeaderTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel LocationLabel { get; set; }


        [Outlet]
        UIKit.UILabel LocationTitleLabel { get; set; }


        [Outlet]
        UIKit.UIButton NavigateButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DescriptionLabel != null) {
                DescriptionLabel.Dispose ();
                DescriptionLabel = null;
            }

            if (DescriptionTitleLabel != null) {
                DescriptionTitleLabel.Dispose ();
                DescriptionTitleLabel = null;
            }

            if (DialogContainer != null) {
                DialogContainer.Dispose ();
                DialogContainer = null;
            }

            if (Header != null) {
                Header.Dispose ();
                Header = null;
            }

            if (HeaderCloseButton != null) {
                HeaderCloseButton.Dispose ();
                HeaderCloseButton = null;
            }

            if (HeaderIconImage != null) {
                HeaderIconImage.Dispose ();
                HeaderIconImage = null;
            }

            if (HeaderTitleLabel != null) {
                HeaderTitleLabel.Dispose ();
                HeaderTitleLabel = null;
            }

            if (LocationLabel != null) {
                LocationLabel.Dispose ();
                LocationLabel = null;
            }

            if (LocationTitleLabel != null) {
                LocationTitleLabel.Dispose ();
                LocationTitleLabel = null;
            }

            if (NavigateButton != null) {
                NavigateButton.Dispose ();
                NavigateButton = null;
            }
        }
    }
}