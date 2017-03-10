using CoreLocation;
using MapKit;
using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.iOS.Maps.Annotations;
using MSCorp.FirstResponse.Client.Maps;
using MSCorp.FirstResponse.Client.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using MSCorp.FirstResponse.Client.Helpers;

namespace MSCorp.FirstResponse.Client.iOS.Maps
{
    public class AnnotationManager : AbstractPushpinManager
    {
        private readonly MKMapView _nativeMap;

        private UserAnnotation _userAnnotation;

        public AnnotationManager(MKMapView nativeMap, CustomMap formsMap)
            : base(formsMap)
        { 
            _nativeMap = nativeMap;

            _nativeMap.DidSelectAnnotationView -= DidSelectAnnotationView;
            _nativeMap.DidSelectAnnotationView += DidSelectAnnotationView;
            _nativeMap.DidDeselectAnnotationView -= DidDeselectAnnotationView;
            _nativeMap.DidDeselectAnnotationView += DidDeselectAnnotationView;
        }

        public override void AddUser()
        {
            _userAnnotation = new UserAnnotation(new CLLocationCoordinate2D
            {
                Latitude = Settings.UserLatitude,
                Longitude = Settings.UserLongitude
            });

            _nativeMap.AddAnnotation(_userAnnotation);
        }

        public override void UnloadTickets()
        {
            var allTicketAnnotations = _nativeMap.Annotations?.OfType<TicketAnnotation>()
                                                              .ToArray();

            if (allTicketAnnotations?.Any() == true)
            {
                _nativeMap.RemoveAnnotations(allTicketAnnotations);
            }
        }

        public MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            if (annotation is IncidentAnnotation)
            {
                return GetViewForIncidentAnnotation(annotation as IncidentAnnotation);
            }
            else if (annotation is UserAnnotation)
            {
                return GetViewForUserAnnotation(annotation as UserAnnotation);
            }
            else if (annotation is ResponderAnnotation)
            {
                return GetViewForResponderAnnotation(annotation as ResponderAnnotation);
            }
            else if (annotation is TicketAnnotation)
            {
                return GetViewForTicketAnnotation(annotation as TicketAnnotation);
            }
            else
            {
                return null;
            }
        }

        public override void ShowIncidentInformationPanel(IncidentModel incident)
        {
            // search for the annotation in the map
            var annotation = _nativeMap?.Annotations?.OfType<IncidentAnnotation>().Where(i => i.Incident.Id == incident.Id).FirstOrDefault();
            _nativeMap.SelectAnnotation(annotation, false);
        }

        public override void HideIncidentInformationPanel()
        {
            // close all selected annotations
            var annotations = _nativeMap?.SelectedAnnotations;
            if (annotations != null)
            {
                foreach (var annotation in annotations)
                {
                    _nativeMap.DeselectAnnotation(annotation, false);
                }
            }
        }

        public override Geoposition GetCurrentUserPosition()
        {
            var userAnnotation = _nativeMap.Annotations.OfType<UserAnnotation>()
                                                       .FirstOrDefault();

            if (userAnnotation == null)
            {
                System.Diagnostics.Debug.WriteLine("User annotation not found!");
                return default(Geoposition);
            }

            return CoordinateConverter.ConvertToAbstraction(userAnnotation.Coordinate);
        }

        public override Geoposition GetResponderPosition(ResponderModel responder)
        {
            var responderAnnotation = _nativeMap.Annotations.OfType<ResponderAnnotation>()
                                                            .Where(r => r.Responder.Id == responder.Id)
                                                            .FirstOrDefault();

            if (responderAnnotation == null)
            {
                System.Diagnostics.Debug.WriteLine("Responder annotation not found!");
                return default(Geoposition);
            }

            return CoordinateConverter.ConvertToAbstraction(responderAnnotation.Coordinate);
        }

        public override void SetInteraction(bool active)
        {
            foreach (var annotation in _nativeMap.Annotations)
            {
                var view = _nativeMap.ViewForAnnotation(annotation);

                if (view != null)
                {
                    view.Enabled = active;
                }
            }
        }

        public override void RemoveAllIncidents()
        {
            IncidentAnnotation[] annotations = 
                _nativeMap?.Annotations?.OfType<IncidentAnnotation>()
                                        .ToArray();

            if (annotations != null)
            {
                _nativeMap.RemoveAnnotations(annotations);
            }
        }

        public override void RemoveIncidents(IEnumerable<IncidentModel> removedIncidents)
        {
            IncidentAnnotation[] annotations =
                _nativeMap?.Annotations?.OfType<IncidentAnnotation>()
                                        .Where(a => removedIncidents.Any(i => i.Id == a.Incident.Id))
                                        .ToArray();

            if (annotations != null)
            {
                _nativeMap.RemoveAnnotations(annotations);
            }
        }

        public override void RemoveAllResponders()
        {
            ResponderAnnotation[] annotations =
                   _nativeMap?.Annotations?.OfType<ResponderAnnotation>()
                                           .ToArray();

            if (annotations != null)
            {
                _nativeMap.RemoveAnnotations(annotations);
            }
        }

        public override void RemoveResponder(ResponderModel responder)
        {
            var responderAnnotation = _nativeMap.Annotations.OfType<ResponderAnnotation>()
                                                            .FirstOrDefault(r => r.Responder.Id == responder.Id);

            if (responderAnnotation != null)
            {
                _nativeMap.RemoveAnnotation(responderAnnotation);
            }
        }
        
        protected override void AddIncidentToMap(IncidentModel incident)
        {
            var annotation = new IncidentAnnotation(new
                     CLLocationCoordinate2D
            {
                Latitude = incident.Latitude,
                Longitude = incident.Longitude
            },
                incident);

            _nativeMap.AddAnnotation(annotation);
        }

        protected override void AddResponderToMap(ResponderModel responder)
        {
            var annotation = new ResponderAnnotation(new
                     CLLocationCoordinate2D
            {
                Latitude = responder.Latitude,
                Longitude = responder.Longitude
            },
                responder);

            _nativeMap.AddAnnotation(annotation);
        }

        protected override void AddTicketToMap(TicketModel ticket)
        {
            var annotation = new TicketAnnotation(new
                     CLLocationCoordinate2D
            {
                Latitude = ticket.Latitude,
                Longitude = ticket.Longitude
            },
                ticket);

            _nativeMap.AddAnnotation(annotation);
        }

        private MKAnnotationView GetViewForUserAnnotation(UserAnnotation annotation)
        {
            var annotationView = _nativeMap.DequeueReusableAnnotation(UserAnnotationView.CustomReuseIdentifier);

            if (annotationView == null)
            {
                annotationView = new UserAnnotationView(annotation);
            }

            return annotationView;
        }

        private MKAnnotationView GetViewForIncidentAnnotation(IncidentAnnotation annotation)
        {
            var annotationView = _nativeMap.DequeueReusableAnnotation(IncidentAnnotationView.CustomReuseIdentifier) as IncidentAnnotationView;

            if (annotationView == null)
            {
                annotationView = new IncidentAnnotationView(annotation, annotation.Incident);
                annotationView.OnClose += OnIncidentInfoWindowClose;
                annotationView.OnNavigationRequested += OnIncidentInfoNavigationRequest;
            }
            else
            {
                annotationView.Incident = annotation.Incident;
            }

            return annotationView;
        }

        private MKAnnotationView GetViewForResponderAnnotation(ResponderAnnotation annotation)
        {
            var annotationView = _nativeMap.DequeueReusableAnnotation(ResponderAnnotationView.CustomReuseIdentifier) as ResponderAnnotationView;

            if (annotationView == null)
            {
                annotationView = new ResponderAnnotationView(annotation, annotation.Responder);
            }
            else
            {
                annotationView.Responder = annotation.Responder;
            }

            return annotationView;
        }

        private MKAnnotationView GetViewForTicketAnnotation(TicketAnnotation annotation)
        {
            var annotationView = _nativeMap.DequeueReusableAnnotation(TicketAnnotationView.CustomReuseIdentifier) as TicketAnnotationView;

            if (annotationView == null)
            {
                annotationView = new TicketAnnotationView(annotation, annotation.Ticket);
            }
            else
            {
                annotationView.Ticket = annotation.Ticket;
            }

            return annotationView;
        }

        private void OnIncidentInfoNavigationRequest(object sender, EventArgs e)
        {
            var incidentView = sender as IncidentAnnotationView;

            if (incidentView != null)
            {
                OnNavigationRequested(incidentView.Incident);
            }
        }

        private void OnIncidentInfoWindowClose(object sender, EventArgs e)
        {
            var annotationView = sender as IncidentAnnotationView;

            if (annotationView != null)
            {
                _nativeMap.DeselectAnnotation(annotationView.Annotation, true);
            }

            FormsMap.CurrentIncident = null;
        }

        private void DidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            var incidentAnnotation = e.View as IncidentAnnotationView;

            if (incidentAnnotation == null)
            {
                // We only care about Incident annotations
                return;
            }

            OnIncidentSelected(incidentAnnotation.Incident);
        }

        private void DidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            OnIncidentUnselected(FormsMap.CurrentIncident);
        }
    }
}