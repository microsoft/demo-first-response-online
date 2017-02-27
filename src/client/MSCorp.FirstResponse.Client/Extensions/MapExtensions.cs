using MSCorp.FirstResponse.Client.Models;
using System;
using Xamarin.Forms.Maps;

namespace MSCorp.FirstResponse.Client.Extensions
{
    public static class MapExtensions
    {
        public static void SetPosition(this Map map, Geoposition position)
        {
            SetPosition(map, position, Distance.FromMiles(map.VisibleRegion.Radius.Miles));
        }

        public static void SetPosition(this Map map, Geoposition position, Distance distance)
        {
            var fPosition = new Position(position.Latitude, position.Longitude);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(fPosition, distance));
        }

        public static int GetZoomLevel(this Map map)
        {
            var x = (map.VisibleRegion.LatitudeDegrees + map.VisibleRegion.LongitudeDegrees) / 2.0;
            int zoom = (int)Math.Floor(Math.Log(360 / x, 2));

            return zoom;
        }
    }
}
