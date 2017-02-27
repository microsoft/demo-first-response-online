using Windows.Devices.Geolocation;

namespace MSCorp.FirstResponse.Client.UWP.Services
{
    public class MockPositionChangedEventArgs
    {
        public Geopoint Position { get; }

        public MockPositionChangedEventArgs(Geopoint position)
        {
            Position = position;
        }
    }
}
