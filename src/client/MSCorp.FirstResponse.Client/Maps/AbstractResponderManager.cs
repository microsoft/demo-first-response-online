using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Extensions;
using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Maps.Routes;
using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.Services.Dialog;
using MSCorp.FirstResponse.Client.Services.Incidents;
using MSCorp.FirstResponse.Client.ViewModels.Base;
using Plugin.Toasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MSCorp.FirstResponse.Client.Maps
{
    public abstract class AbstractResponderManager
    {
        public event EventHandler<IEnumerable<TicketModel>> LoadTicketsRequest;

        public event EventHandler UnloadTicketsRequest;

        /// <summary>
        /// It saves last predefined route taken by a given responder
        /// </summary>
        private readonly Dictionary<int, int> _responderRoutes = new Dictionary<int, int>();

        private IList<RouteModel> _predefinedRoutes;
        private readonly RoutesUpdater _routeUpdater;

        private Task _updaterTask;
        private CancellationTokenSource _updaterTokenSource;

        private bool userNotified;

        protected readonly CustomMap FormsMap;
        protected readonly AbstractRouteManager RouteManager;
        protected readonly AbstractPushpinManager PushpinManager;

        protected AbstractResponderManager(CustomMap formsMap, AbstractRouteManager routeManager, AbstractPushpinManager pushpinManager)
        {
            FormsMap = formsMap;
            RouteManager = routeManager;
            PushpinManager = pushpinManager;
            _routeUpdater = routeManager.RouteUpdater;
            _routeUpdater.RouteStep += OnRouteStep;
            _routeUpdater.RouteCompleted += OnRouteCompleted;
            _routeUpdater.RouteHalfCompleted += OnRouteHalfCompleted;

            _updaterTokenSource = new CancellationTokenSource();
        }

        public void StopResponderUpdater()
        {
            // Cancel previous task if existing
            _updaterTokenSource.Cancel();
        }

        public abstract void StartResponderUpdater();

        protected Task InnerStartResponderUpdater()
        {
            if (FormsMap.Routes == null || FormsMap.Responders == null)
            {
                return default(Task);
            }

            _updaterTokenSource = new CancellationTokenSource();
            var token = _updaterTokenSource.Token;
            var routes = FormsMap.Routes.ToList();
            var responders = FormsMap.Responders.ToList();

            _updaterTask = Initialize(responders, routes, token);

            return _updaterTask;
        }

        private async Task Initialize(IEnumerable<ResponderModel> responders, IEnumerable<RouteModel> routes, CancellationToken ct = default(CancellationToken))
        {
            _responderRoutes.Clear();
            _predefinedRoutes = routes.ToList();

            foreach (var responder in responders)
            {
                var defaultRoute = _predefinedRoutes.Where(r => r.Id == responder.RouteId)
                                                    .DefaultIfEmpty(routes.ElementAt(0))
                                                    .FirstOrDefault();
                AssignRouteToResponder(responder, defaultRoute);

                if (ct.IsCancellationRequested)
                {
                    System.Diagnostics.Debug.WriteLine("AbstractResponderManager.Initialize: Updater task cancelled");
                    ct.ThrowIfCancellationRequested();
                }
            }

            await _routeUpdater.Run(ct);
        }

        private async void AssignRouteToResponder(ResponderModel responder, RouteModel defaultRoute)
        {
            if (defaultRoute.RoutePoints.Any())
            {
                var routeInstance = new Route<ResponderModel>(defaultRoute.RoutePoints.ToArray());
                routeInstance.Element = responder;

                // calculate route from the actual point to the route start point
                Geoposition responderPosition = PushpinManager.GetResponderPosition(responder);
                IEnumerable<Geoposition> routeToStartPosition = await RouteManager.CalculateRoute(responderPosition, routeInstance.RoutePositions.First());
                if (!routeToStartPosition.Any())
                {
                    // if routes api dont return any points, route directly to the position.
                    routeToStartPosition = new []{ responderPosition, routeInstance.RoutePositions.First()};
                }
                routeInstance.AddRouteToStartPoint(routeToStartPosition);
                _routeUpdater.AddRoute(routeInstance);

                var routeIndex = _predefinedRoutes.IndexOf(defaultRoute);
                _responderRoutes[responder.Id] = routeIndex;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("FullRoute has zero elements, route could not be started");
            }
        }

        public void DismissCurrentUserIncident()
        {
            // Clear search polygon
            RemoveSearchAreaPolygon();

            // Request to unload tickets
            var handler = UnloadTicketsRequest;
            handler?.Invoke(this, EventArgs.Empty);

            // Reset user availability and re-enable map incident interaction
            CurrentUserStatus.Reset();
        }

        public void StartUserNavigation()
        {
            if (!CurrentUserStatus.CanNavigate)
            {
                return;
            }

            userNotified = false;

            Geoposition currentUserPosition = PushpinManager.GetCurrentUserPosition();
            IEnumerable<Geoposition> userRoutePositions = RouteManager.GetCurrentUserRoute();

            if (userRoutePositions == null || !userRoutePositions.Any())
            {
                return;
            }

            // now user has a route and start navigating
            CurrentUserStatus.AttendingIncidentId = FormsMap.CurrentIncident.Id;
            CurrentUserStatus.IsNavigating = true;

            var route = new Route<UserRole>(userRoutePositions.ToArray());

            route.Element = null;
            route.AddStartPoint(currentUserPosition);
            route.Init();
            _routeUpdater.AddRoute(route);
        }

        protected abstract void DrawSearchAreaPolygon(Geoposition[] polygonData);

        protected abstract void UpdatePushpinPosition(Route e);

        protected abstract void RemoveSearchAreaPolygon();

        private void OnRouteStep(object sender, Route e)
        {
            UpdatePushpinPosition(e);
        }

        private async void OnRouteHalfCompleted(object sender, Route e)
        {
            System.Diagnostics.Debug.WriteLine($"Route {e.Id} half completed.");

            if (e is Route<UserRole>)
            {
                if (!userNotified)
                {
                    userNotified = true;

                    if (Device.OS == TargetPlatform.iOS)
                    {
                        IDialogService notificator = ViewModelLocator.Instance.Resolve<IDialogService>();
                        notificator.ShowLocalNotification("Dispatch updated incident", () => {});
                    }
                    else
                    {
                        // send toast notification
                        var notificator = DependencyService.Get<IToastNotificator>();

                        var options = new NotificationOptions()
                        {
                            Title = "Dispatch updated incident",
                            IsClickable = true,
                            ClearFromHistory = true
                        };
                        await notificator.Notify(options);
                    }

                    UpdateIncidentDescription();
                    await RequestAmbulanceOnIncident();
                }
            }
        }

        private void UpdateIncidentDescription()
        {
            IncidentModel currentIncident = FormsMap.Incidents?.FirstOrDefault(i => i.Id == CurrentUserStatus.AttendingIncidentId);
            if (!currentIncident.Description.Contains("Driver has collided with the curb. In shock and potentially sustained an injury."))
            {
                currentIncident.Description += ". Driver has collided with the curb. In shock and potentially sustained an injury.";
                this.PushpinManager.HideIncidentInformationPanel();
                if (FormsMap.CurrentIncident?.Id != CurrentUserStatus.AttendingIncidentId)
                {
                    FormsMap.CurrentIncident = null;
                }
                FormsMap.CurrentIncident = currentIncident;
                this.PushpinManager.ShowIncidentInformationPanel(currentIncident);
            }
        }
        
        private async Task RequestAmbulanceOnIncident()
        {
            // locate attending incident (can differ from selected)
            IncidentModel currentIncident = FormsMap.Incidents?.FirstOrDefault(i => i.Id == CurrentUserStatus.AttendingIncidentId);
            if (currentIncident.IsHighPriority)
            {
                // search or create ambulance responder
                var ambulance = new ResponderModel
                {
                    Id = 8,
                    ResponderDepartment = DepartmentType.Ambulance,
                    Status = ResponseStatus.EnRoute,
                    Longitude = Settings.AmbulanceLongitude,
                    Latitude = Settings.AmbulanceLatitude,
                    IsPriority = true
                };

                // add responder to map
                this.PushpinManager.RemoveResponder(ambulance);
                this.PushpinManager.AddResponders(new List<ResponderModel>() { ambulance });

                var fromPosition = new Geoposition()
                {
                    Latitude = ambulance.Latitude,
                    Longitude = ambulance.Longitude
                };

                var toPosition = new Geoposition()
                {
                    Latitude = currentIncident.Latitude,
                    Longitude = currentIncident.Longitude
                };

                // create route from ambulance to incident
                var routeAmbulance = await this.RouteManager.CalculateRoute(fromPosition, toPosition);

                if (!routeAmbulance.Any())
                {
                    // map route service fails
                    routeAmbulance = new[] { fromPosition, toPosition };
                }

                // start route movement
                var route = new Route<ResponderModel>(routeAmbulance.ToArray());
                route.Element = ambulance;
                route.AddStartPoint(new Geoposition()
                {
                    Latitude = ambulance.Latitude,
                    Longitude = ambulance.Longitude
                });
                route.Init();
                _routeUpdater.AddRoute(route);
            }
        }

        private async void OnRouteCompleted(object sender, Route e)
        {
            System.Diagnostics.Debug.WriteLine($"Route {e.Id} completed.");

            if (e is Route<UserRole>)
            {
                await OnUserArrivedToDestination();
            }
            else if (e is Route<ResponderModel>)
            {
                var responderRoute = e as Route<ResponderModel>;
                OnResponderRouteCompleted(responderRoute);
            }
        }

        private void OnResponderRouteCompleted(Route<ResponderModel> route)
        {
            ResponderModel responder = route.Element;

            if (responder.Status == ResponseStatus.Available)
            {
                int currentPredefinedRouteIndex;
                _responderRoutes.TryGetValue(responder.Id, out currentPredefinedRouteIndex);

                int nextPredefinedRouteIndex = _predefinedRoutes.Count > currentPredefinedRouteIndex + 1
                    ? currentPredefinedRouteIndex + 1
                    : 0;

                var defaultRoute = _predefinedRoutes.ElementAt(nextPredefinedRouteIndex);
                AssignRouteToResponder(responder, defaultRoute);
            }
            else if (responder.Status == ResponseStatus.EnRoute)
            {
                // state as busy
                responder.Status = ResponseStatus.Busy;
                if (responder.IsPriority)
                {
                    
                }
            }
        }

        private async Task OnUserArrivedToDestination()
        {
            IncidentModel currentIncident = FormsMap.Incidents?.FirstOrDefault(i => i.Id == CurrentUserStatus.AttendingIncidentId);
            CurrentUserStatus.IsNavigating = false;

            if (currentIncident == null)
            {
                return;
            }

            // set incident as selected 
            if (FormsMap.CurrentIncident?.Id != CurrentUserStatus.AttendingIncidentId)
            {
                FormsMap.CurrentIncident = null;
                FormsMap.CurrentIncident = currentIncident;
            }

            // display identify button on the UI
            currentIncident.ReadyToIdentify = CurrentUserStatus.IsAttendingAnIncident = true;

            // disable informationPanel
            PushpinManager.HideIncidentInformationPanel();

            // call search-area api to fetch the polygon and the related tickets
            IIncidentsService incidentsService = ViewModelLocator.Instance.Resolve<IIncidentsService>();
            currentIncident.SearchArea = await incidentsService.GetSearchAreaForIncidentAsync(currentIncident.Id);

            // Draw search polygon
            if (currentIncident.SearchArea?.Polygon?.Any() == true)
            {
                var polygonData = currentIncident.SearchArea.Polygon;
                DrawSearchAreaPolygon(polygonData);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("No search area defined for current incident");
            }

            // Draw near tickets
            if (currentIncident.SearchArea?.Tickets?.Any() == true)
            {
                var handler = LoadTicketsRequest;
                handler?.Invoke(this, currentIncident.SearchArea.Tickets);
            }

            // Clear navigation
            RouteManager.ClearAllUserRoutes();

            // Disable interaction with rest of incidents
            PushpinManager.SetInteraction(false);
            FormsMap.IsForceNavigation = false;

            // Center and zoom map
            FormsMap.SetPosition(currentIncident.GeoLocation, Distance.FromMiles(1));
        }
    }
}
