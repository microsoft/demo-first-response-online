using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using MSCorp.FirstResponse.Client.Droid.Extensions;
using MSCorp.FirstResponse.Client.Models;

namespace MSCorp.FirstResponse.Client.Droid.Maps.Icons
{
    public class TicketIcon : BaseIcon
    {
        private LayoutInflater _inflater;

        public TicketIcon(TicketModel ticket)
            : base()
        {
            Ticket = ticket;

            _inflater = LayoutInflater.From(Xamarin.Forms.Forms.Context);
            var ticketIconView = _inflater.Inflate(Resource.Layout.ticket_icon_content, null);
            var ticketIcon = ticketIconView.FindViewById<ImageView>(Resource.Id.ticket_icon);

            if (Ticket.Type == "Traffic Violation")
            {
                ticketIcon.SetImageResource(Resource.Drawable.pin_car);
            }
            else
            {
                ticketIcon.SetImageResource(Resource.Drawable.pin_pedestrian);
            }

            Bitmap icon = ticketIconView.AsBitmap(Xamarin.Forms.Forms.Context, 106, 144);
            MarkerOptions.SetIcon(BitmapDescriptorFactory.FromBitmap(icon));
        }

        public TicketModel Ticket { get; }
    }
}