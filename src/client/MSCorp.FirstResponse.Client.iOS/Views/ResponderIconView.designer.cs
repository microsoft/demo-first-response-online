// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace MSCorp.FirstResponse.Client.iOS
{
    [Register ("ResponderIconView")]
    partial class ResponderIconView
    {
        [Outlet]
        UIKit.UILabel NameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (NameLabel != null) {
                NameLabel.Dispose ();
                NameLabel = null;
            }
        }
    }
}