using System.Linq;
using BingMapsRESTToolkit;

namespace MSCorp.FirstResponse.WebApiDemo.MapService
{
    public static class AddressGetter
    {
        private static string _key = "Aj8J8KEAFCZQ6LYzF5h2M01FT7bzptBGVP8ku8y80bB8i8xfeOtT8d - a3JVm3xJe";

        public static string GetFormattedAddress(Coordinate coordinate)
        {
            var r = ServiceManager.GetResponseAsync(new ReverseGeocodeRequest()
            {
                BingMapsKey = _key,
                Point = coordinate
            }).GetAwaiter().GetResult();

            if (r != null && r.ResourceSets != null &&
                r.ResourceSets.Length > 0 &&
                r.ResourceSets[0].Resources != null &&
                r.ResourceSets[0].Resources.Length > 0)
            {
                var location = (Location)r.ResourceSets.First().Resources.First();

                return location.Address.FormattedAddress;
            }

            return null;
        }

    }
}
