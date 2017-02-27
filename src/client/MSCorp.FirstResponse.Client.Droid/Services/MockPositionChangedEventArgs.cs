using Android.Gms.Maps.Model;

namespace MSCorp.FirstResponse.Client.Droid.Services
{
    public class MockPositionChangedEventArgs
    {
        public LatLng Position { get; }

        public MockPositionChangedEventArgs(LatLng position)
        {
            Position = position;
        }
    }
}