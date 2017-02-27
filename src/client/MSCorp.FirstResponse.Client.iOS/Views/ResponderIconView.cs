using Foundation;
using MSCorp.FirstResponse.Client.Models;
using ObjCRuntime;
using System;
using UIKit;

namespace MSCorp.FirstResponse.Client.iOS
{
    public partial class ResponderIconView : UIView
    {
        public ResponderIconView (IntPtr handle) : base (handle)
        {
        }

        public static ResponderIconView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib("ResponderIconView", null, null);
            var v = Runtime.GetNSObject<ResponderIconView>(arr.ValueAt(0));

            return v;
        }

        public override void AwakeFromNib()
        {
        }

        public void LoadResponderData(ResponderModel responder)
        {
            NameLabel.Text = responder.ResponderCode;
        }
    }
}