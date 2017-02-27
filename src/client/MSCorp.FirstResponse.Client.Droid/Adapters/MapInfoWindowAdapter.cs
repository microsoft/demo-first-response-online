using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Views;
using Android.Widget;
using MSCorp.FirstResponse.Client.Models;
using Xamarin.Forms.Platform.Android;

namespace MSCorp.FirstResponse.Client.Droid.Adapters
{
    public class MapInfoWindowAdapter : Java.Lang.Object, GoogleMap.IInfoWindowAdapter
    {
        private const int AlertResource = Resource.Drawable.icon_alert;
        private const int AnimalResource = Resource.Drawable.icon_animal;
        private const int CarResource = Resource.Drawable.icon_car;
        private const int FireResource = Resource.Drawable.icon_fire;
        private const int OfficerRequiredResource = Resource.Drawable.icon_officer;
        private const int StrangerResource = Resource.Drawable.icon_stranger;
        private const int OtherResource = Resource.Drawable.icon_other;

        private LayoutInflater _inflater;

        public MapInfoWindowAdapter()
        {
            _inflater = LayoutInflater.From(Xamarin.Forms.Forms.Context);
        }

        public IncidentModel CurrentIncident { get; set; }

        public View GetInfoWindow(Marker marker)
        {
            if (_inflater != null && CurrentIncident != null)
            {
                var incidentIconView = _inflater.Inflate(Resource.Layout.incident_info_content, null);

                var titleArea = incidentIconView.FindViewById<View>(Resource.Id.incident_marker_title_area);
                if (titleArea != null)
                {
                    titleArea.SetBackgroundColor(CurrentIncident.IncidentColor.ToAndroid());
                }

                UpdateTitleIcon(incidentIconView);

                var incidentTitle = incidentIconView.FindViewById<TextView>(Resource.Id.incident_marker_title);
                if (incidentTitle != null)
                {
                    incidentTitle.Text = CurrentIncident.Title;
                }

                var incidentDescription = incidentIconView.FindViewById<TextView>(Resource.Id.incident_marker_description);
                if (incidentDescription != null)
                {
                    incidentDescription.Text = CurrentIncident.Description;
                }

                var incidentLocation = incidentIconView.FindViewById<TextView>(Resource.Id.incident_marker_location);
                if (incidentLocation != null)
                {
                    incidentLocation.Text = CurrentIncident.Address;
                }

                return incidentIconView;
            }

            return null;
        }

        private void UpdateTitleIcon(View incidentIconView)
        {
            var titleIcon = incidentIconView.FindViewById<ImageView>(Resource.Id.incident_marker_icon);

            if (titleIcon != null)
            {
                int resourceId;

                switch (CurrentIncident.IncidentCategory)
                {
                    case IncidentType.Alert:
                        resourceId = AlertResource;
                        break;
                    case IncidentType.Animal:
                        resourceId = AnimalResource;
                        break;
                    case IncidentType.Car:
                        resourceId = CarResource;
                        break;
                    case IncidentType.Fire:
                        resourceId = FireResource;
                        break;
                    case IncidentType.OfficerRequired:
                        resourceId = OfficerRequiredResource;
                        break;
                    case IncidentType.Stranger:
                        resourceId = StrangerResource;
                        break;
                    default:
                        resourceId = OtherResource;
                        break;
                }

                titleIcon.SetImageResource(resourceId);
            }
        }

        public View GetInfoContents(Marker marker)
        {
            return null;
        }
    }
}