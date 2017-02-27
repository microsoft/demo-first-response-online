using System;
using System.Collections.Generic;
using BingMapsRESTToolkit;

namespace MSCorp.FirstResponse.WebApiDemo.MapService
{
    public static class MapPointGenerator
    {
        private static readonly Random random = new Random();

        public static Coordinate GetPoint(Coordinate coordinate, int radius = 7000 )
        {
            double radiusInDegrees = radius / 111300f;

            double u = random.NextDouble();
            double v = random.NextDouble();

            double w = radiusInDegrees * Math.Sqrt(u);
            double t = 2 * Math.PI * v;

            double x = w * Math.Cos(t);
            double y = w * Math.Sin(t);

            double newX = x / Math.Cos(coordinate.Latitude);

            double foundLongitude = newX + coordinate.Longitude;
            double foundLatitude = y + coordinate.Latitude;

            return new Coordinate(foundLatitude, foundLongitude);
        }

        public static IEnumerable<Coordinate> GetVertices(Coordinate coordinate, int radius)
        {
            double radiusInDegrees = radius / 111300f;

            var vertices = new List<Coordinate>
            {
                new Coordinate
                {
                    Latitude = coordinate.Latitude + radiusInDegrees,
                    Longitude = coordinate.Longitude + radiusInDegrees
                },
                new Coordinate
                {
                    Latitude = coordinate.Latitude + radiusInDegrees,
                    Longitude = coordinate.Longitude - radiusInDegrees
                },
                new Coordinate
                {
                    Latitude = coordinate.Latitude - radiusInDegrees,
                    Longitude = coordinate.Longitude + radiusInDegrees
                },
                new Coordinate
                {
                    Latitude = coordinate.Latitude - radiusInDegrees,
                    Longitude = coordinate.Longitude - radiusInDegrees
                }
            };

            return vertices;
        }

    }
}