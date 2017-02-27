using System;
using System.Globalization;

namespace MSCorp.FirstResponse.Client.Maps.Heat
{
    public static class HeatMapHelper
    {
        public static byte[] GetHeatMapImageBytes(string imageData)
        {
            var base64 = imageData.Substring(imageData.IndexOf(",") + 1);
            var imageBytes = Convert.FromBase64String(base64);

            return imageBytes;
        }

        public static HeatMapRenderParameters GetRenderParameters(string locations, double radius, double minRadius, double zoom)
        {
            var groundResolution = (2 * System.Math.PI * 6378135) / System.Math.Round(256 * System.Math.Pow(2, zoom));
            var radiusInPixels = radius / groundResolution;

            if (radiusInPixels < minRadius)
            {
                radiusInPixels = minRadius;
            }

            return new HeatMapRenderParameters
            {
                Locations = locations,
                Radius = radiusInPixels.ToString(CultureInfo.InvariantCulture),
                Zoom = zoom.ToString(CultureInfo.InvariantCulture)
            };
        }

        public static string GetOptionsJson(double intensity)
        {
            return $"{{'intensity': {intensity.ToString(CultureInfo.InvariantCulture)}}}";
        }
    }
}
