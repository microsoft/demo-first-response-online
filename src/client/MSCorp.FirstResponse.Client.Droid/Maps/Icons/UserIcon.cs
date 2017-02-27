using Android.Gms.Maps.Model;
using MSCorp.FirstResponse.Client.Models;

namespace MSCorp.FirstResponse.Client.Droid.Maps.Icons
{
    public class UserIcon : BaseIcon
    {
        private const int ResponderResource = Resource.Drawable.nav_car;

        public UserIcon()
            : base()
        {
            var responderIcon = BitmapDescriptorFactory.FromResource(ResponderResource);
            MarkerOptions.SetIcon(responderIcon);
        }
    }
}