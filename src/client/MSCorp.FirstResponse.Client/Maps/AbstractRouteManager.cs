using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Maps.Routes;
using MSCorp.FirstResponse.Client.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Maps
{
    public abstract class AbstractRouteManager
    {
        private readonly RoutesUpdater _routeUpdater;
        private readonly RouteCache _routeCache;
        protected readonly CustomMap FormsMap;
        protected readonly AbstractPushpinManager PushpinManager;

        protected AbstractRouteManager(CustomMap formsMap, AbstractPushpinManager pushpinManager)
        {
            FormsMap = formsMap;
            PushpinManager = pushpinManager;
            _routeUpdater = new RoutesUpdater();
            _routeCache = new RouteCache();
        }

        public RoutesUpdater RouteUpdater
        {
            get
            {
                return _routeUpdater;
            }
        }


        public abstract void ClearAllUserRoutes();

        public async Task DrawRouteToIncident(IncidentModel incident)
        {
            Geoposition from = PushpinManager.GetCurrentUserPosition();

            if (from == default(Geoposition))
            {
                return;
            }

            Geoposition to = new Geoposition
            {
                Latitude = incident.Latitude,
                Longitude = incident.Longitude
            };

            IEnumerable<Geoposition> routePositions = await GetRoute(from, to);
            DrawRouteInMap(routePositions);
        }

        public async Task<IEnumerable<Geoposition>> GetRoute(Geoposition from, Geoposition to)
        {
            bool isCached = _routeCache.HasRoute(from, to);

            if (isCached)
            {
                Debug.WriteLine($"Route from {from} to {to} is already cached.");
                return _routeCache.GetRoute(from, to);
            }
            else
            {
                Debug.WriteLine($"Route from {from} to {to} is not cached.");
                var routePositions = await CalculateRoute(from, to);
                _routeCache.SetRoute(from, to, routePositions);

                return routePositions;
            }
        }

        public abstract IEnumerable<Geoposition> GetCurrentUserRoute();

        public abstract Task<IEnumerable<Geoposition>> CalculateRoute(Geoposition from, Geoposition to);
        
        protected abstract void DrawRouteInMap(IEnumerable<Geoposition> positions);
    }
}