using MSCorp.FirstResponse.Client.Models;
using Android.Gms.Maps.Model;

namespace MSCorp.FirstResponse.Client.Droid.Maps.Icons
{
    public class IncidentIcon : BaseIcon
    {
        private const int AlertResource = Resource.Drawable.pin_alert;
        private const int AnimalResource = Resource.Drawable.pin_animal;
        private const int ArrestResource = Resource.Drawable.pin_arrest;
        private const int CarResource = Resource.Drawable.pin_car;
        private const int FireResource = Resource.Drawable.pin_siren;
        private const int OfficerRequiredResource = Resource.Drawable.pin_officer;
        private const int StrangerResource = Resource.Drawable.pin_stranger;

        public IncidentIcon(IncidentModel incident)
            : base()
        {
            Incident = incident;

            Initialize();
        }

        public IncidentModel Incident { get; }

        private void Initialize()
        {
            switch (Incident.IncidentCategory)
            {
                case IncidentType.Alert:
                    var alertIcon = BitmapDescriptorFactory.FromResource(AlertResource);
                    MarkerOptions.SetIcon(alertIcon);
                    break;
                case IncidentType.Animal:
                    var animalIcon = BitmapDescriptorFactory.FromResource(AnimalResource);
                    MarkerOptions.SetIcon(animalIcon);
                    break;
                case IncidentType.Arrest:
                    var arrestIcon = BitmapDescriptorFactory.FromResource(ArrestResource);
                    MarkerOptions.SetIcon(arrestIcon);
                    break;
                case IncidentType.Car:
                    var carIcon = BitmapDescriptorFactory.FromResource(CarResource);
                    MarkerOptions.SetIcon(carIcon);
                    break;
                case IncidentType.Fire:
                    var fireIcon = BitmapDescriptorFactory.FromResource(FireResource);
                    MarkerOptions.SetIcon(fireIcon);
                    break;
                case IncidentType.OfficerRequired:
                    var officerIcon = BitmapDescriptorFactory.FromResource(OfficerRequiredResource);
                    MarkerOptions.SetIcon(officerIcon);
                    break;
                case IncidentType.Stranger:
                    var strangerIcon = BitmapDescriptorFactory.FromResource(StrangerResource);
                    MarkerOptions.SetIcon(strangerIcon);
                    break;
                default:
                    MarkerOptions.SetIcon(BitmapDescriptorFactory.DefaultMarker());
                    break;
            }
        }
    }
}