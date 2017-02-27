using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Maps.Routes;
using MSCorp.FirstResponse.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Maps
{
    public abstract class AbstractRouteManager
    {
        private readonly RoutesUpdater _routeUpdater;
        protected readonly CustomMap FormsMap;
        protected readonly AbstractPushpinManager PushpinManager;

        protected AbstractRouteManager(CustomMap formsMap, AbstractPushpinManager pushpinManager)
        {
            FormsMap = formsMap;
            PushpinManager = pushpinManager;
            _routeUpdater = new RoutesUpdater();
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

            IEnumerable<Geoposition> routePositions = await CalculateRoute(from, to);
            DrawRouteInMap(routePositions);
        }

        public abstract IEnumerable<Geoposition> GetCurrentUserRoute();

        public abstract Task<IEnumerable<Geoposition>> CalculateRoute(Geoposition from, Geoposition to);
        
        protected abstract void DrawRouteInMap(IEnumerable<Geoposition> positions);
    }
}