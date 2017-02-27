using MSCorp.FirstResponse.Client.Helpers;

namespace MSCorp.FirstResponse.Client.Models
{
    public class DeviceResponderUnit : ResponderModel
    {
        public DeviceResponderUnit()
        {
            Latitude = Settings.UserLatitude;
            Longitude = Settings.UserLongitude;
        }
    }
}