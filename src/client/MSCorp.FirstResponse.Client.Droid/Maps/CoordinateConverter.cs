using Android.Gms.Maps.Model;
using MSCorp.FirstResponse.Client.Models;

namespace MSCorp.FirstResponse.Client.Droid.Maps
{
    public static class CoordinateConverter
    {
        public static LatLng ConvertToNative(Geoposition position)
        {
            return new LatLng(position.Latitude, position.Longitude);
        }

        public static Geoposition ConvertToAbstraction(LatLng position)
        {
            return new Geoposition
            {
                Latitude = position.Latitude,
                Longitude = position.Longitude
            };
        }
    }
}