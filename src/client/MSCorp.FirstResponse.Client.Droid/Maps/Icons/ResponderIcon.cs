using System;
using System.ComponentModel;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using MSCorp.FirstResponse.Client.Droid.Extensions;
using MSCorp.FirstResponse.Client.Models;
using Xamarin.Forms.Platform.Android;

namespace MSCorp.FirstResponse.Client.Droid.Maps.Icons
{
    public class ResponderIcon : BaseIcon
    {
        private LayoutInflater _inflater;
        private View _responderIconView;

        public ResponderIcon(ResponderModel responder)
            : base()
        {
            Responder = responder;

            _inflater = LayoutInflater.From(Xamarin.Forms.Forms.Context);
            _responderIconView = _inflater.Inflate(Resource.Layout.responder_icon_content, null);

            var responderType = _responderIconView.FindViewById<TextView>(Resource.Id.responder_type);
            responderType.Text = Responder.ResponderCode;
            GradientDrawable drawable = (GradientDrawable)responderType.Background;
            drawable.SetColor(Responder.StatusColor.ToAndroid());

            MarkerOptions.SetTitle(Responder.ResponderCode);
            MarkerOptions.SetSnippet(Responder.Incident?.Address);

            Bitmap icon = _responderIconView.AsBitmap(Xamarin.Forms.Forms.Context, 60, 60);
            MarkerOptions.SetIcon(BitmapDescriptorFactory.FromBitmap(icon));
        }

        private void UpdateColor(object sender, PropertyChangedEventArgs e)
        {
            var responderType = _responderIconView.FindViewById<TextView>(Resource.Id.responder_type);
            GradientDrawable drawable = (GradientDrawable)responderType.Background;
            drawable.SetColor((sender as ResponderModel).StatusColor.ToAndroid());
        }

        public ResponderModel Responder { get; }

        // TODO: use this for dynamic map story 
        //public void UpdateStatus(ResponseStatus status)
        //{
        //    Responder.Status = status;

        //    DisplayMetrics displayMetrics = new DisplayMetrics();
        //    var responderType = _responderIconView.FindViewById<TextView>(Resource.Id.responder_type);
        //    GradientDrawable drawable = (GradientDrawable)responderType.Background;
        //    drawable.SetColor(Responder.StatusColor.ToAndroid());
        //}
    }
}