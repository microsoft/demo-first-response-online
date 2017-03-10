using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Maps.Routes
{
    public class RoutesUpdater
    {
        public event EventHandler<Route> RouteStep;
        public event EventHandler<Route> RouteHalfCompleted;
        public event EventHandler<Route> RouteCompleted;

        private readonly ConcurrentDictionary<string, Route> _routes;
        private TimeSpan _updateInterval;

        public RoutesUpdater()
        {
            _routes = new ConcurrentDictionary<string, Route>();
            _updateInterval = GlobalSetting.UpdateInterval;
        }

        public Task Run(CancellationToken ct = default(CancellationToken))
        {
            return UpdateLoop(ct);
        }

        public void AddRoute(Route route)
        {
            route.Init();
            _routes.TryAdd(route.Id, route);
        }

        public void RemoveRoute(Route route)
        {
            Route r;
            _routes.TryRemove(route.Id, out r);
        }

        private async Task UpdateLoop(CancellationToken ct = default(CancellationToken))
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    if (ct.IsCancellationRequested)
                    {
                        System.Diagnostics.Debug.WriteLine("RoutesUpdater.UpdateLoop: Updater task cancelled");
                        ct.ThrowIfCancellationRequested();
                    }

                    foreach (KeyValuePair<string, Route> key in _routes.ToList())
                    {
                        Route route = key.Value;

                        if (route.ArrivedToDestination)
                        {
                            var handler = RouteCompleted;
                            handler?.Invoke(this, route);
                            RemoveRoute(route);
                        }
                        else
                        {
                            if (route.ArrivedToMiddle) {
                                var halfHandler = RouteHalfCompleted;
                                halfHandler?.Invoke(this, route);
                            }

                            CalculateRouteStep(route);

                            var handler = RouteStep;
                            handler?.Invoke(this, route);
                        }

                        if (ct.IsCancellationRequested)
                        {
                            System.Diagnostics.Debug.WriteLine("RoutesUpdater.UpdateLoop: Updater task cancelled");
                            ct.ThrowIfCancellationRequested();
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Routes updater exception {ex}");
                }
                finally
                {
                    await Task.Delay(_updateInterval);
                }
            }
        }

        private void CalculateRouteStep(Route route)
        {
            if (route.ArrivedToDestination)
            {
                return;
            }

            double maxStepDistance = Settings.UserSpeed * _updateInterval.TotalHours;

            Geoposition origin = route.CurrentPosition;
            Geoposition destination = route.NextPosition;

            double pendingDistanceToDestination = GeoCodeCalc.CalcDistance(origin.Latitude, origin.Longitude, destination.Latitude, destination.Longitude);

            if (pendingDistanceToDestination < maxStepDistance)
            {
                // when distance to next point is lower than expected step, simply jump to next point
                route.MoveToNextPosition();
            }
            else
            {
                double distanceProportion = 2 * maxStepDistance / pendingDistanceToDestination;
                double latDiff = destination.Latitude - origin.Latitude;
                double lonDiff = destination.Longitude - origin.Longitude;

                var nextGeoposition = new Geoposition
                {
                    Latitude = origin.Latitude + (latDiff * distanceProportion),
                    Longitude = origin.Longitude + (lonDiff * distanceProportion)
                };

                route.MoveToPosition(nextGeoposition);
            }
        }

        // http://pietschsoft.com/post/2008/02/Calculate-Distance-Between-Geocodes-in-C-and-JavaScript
        internal static class GeoCodeCalc
        {
            public const double EarthRadiusInMiles = 3956.0;
            public const double EarthRadiusInKilometers = 6367.0;

            public static double ToRadian(double val) { return val * (Math.PI / 180); }


            public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }
            
            /// <summary> 
            /// Calculate the distance between two geocodes. Defaults to using Miles. 
            /// </summary> 
            public static double CalcDistance(double lat1, double lng1, double lat2, double lng2)
            {
                return CalcDistance(lat1, lng1, lat2, lng2, GeoCodeCalcMeasurement.Miles);
            }
            /// <summary> 
            /// Calculate the distance between two geocodes. 
            /// </summary> 
            public static double CalcDistance(double lat1, double lng1, double lat2, double lng2, GeoCodeCalcMeasurement m)
            {
                double radius = GeoCodeCalc.EarthRadiusInMiles;
                if (m == GeoCodeCalcMeasurement.Kilometers) { radius = GeoCodeCalc.EarthRadiusInKilometers; }
                return radius * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0) + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
            }
        }

        internal enum GeoCodeCalcMeasurement : int
        {
            Miles = 0,
            Kilometers = 1
        }
    }
}