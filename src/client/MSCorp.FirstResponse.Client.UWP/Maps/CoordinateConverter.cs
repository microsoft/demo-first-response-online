using Windows.Devices.Geolocation;

namespace MSCorp.FirstResponse.Client.UWP.Maps
{
    public static class CoordinateConverter
    {
        public static Geopoint ConvertToNative(Models.Geoposition position)
        {
            return new Geopoint(new BasicGeoposition
            {
                Latitude = position.Latitude,
                Longitude = position.Longitude
            });
        }

        public static Models.Geoposition ConvertToAbstraction(Geopoint position)
        {
            return new Models.Geoposition
            {
                Latitude = position.Position.Latitude,
                Longitude = position.Position.Longitude
            };
        }
    }
}