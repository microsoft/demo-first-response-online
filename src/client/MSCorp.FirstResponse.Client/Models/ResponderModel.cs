using MSCorp.FirstResponse.Client.Helpers;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Models
{
    public class ResponderModel : ExtendedBindableObject
    {
        private ResponseStatus status;

        public int Id { get; set; }
        public DepartmentType ResponderDepartment { get; set; }
        public ResponseStatus Status {
            get
            {
                return status;
            }
            set
            {
                status = value;
                RaisePropertyChanged(() => Status);
            }
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int IncidentId { get; set; }
        public int RouteId { get; set; }
        public bool IsPriority { get; set; }
        public IncidentModel Incident { get; set; }
        public ResponderRequestModel Request { get; set; }

        public Geoposition GeoLocation
        {
            get { return new Geoposition { Latitude = Latitude, Longitude = Longitude }; }
            set { Latitude = value.Latitude; Longitude = value.Longitude; }
        }

        public string ResponderInitial
        {
            get
            {
                switch (ResponderDepartment)
                {
                    case DepartmentType.Fire:
                        return "F";
                    case DepartmentType.Police:
                        return "P";
                    case DepartmentType.Ambulance:
                        return "M";
                    default:
                        return "P";
                }
            }
        }

        public string ResponderCode => string.Format("{0}{1}", ResponderInitial, Id);

        public Color StatusColor
        {
            get
            {
                switch (Status)
                {
                    case ResponseStatus.Available:
                        return Color.FromHex("#4CAF50");
                    case ResponseStatus.EnRoute:
                        return Color.FromHex("#2196F3");
                    case ResponseStatus.Busy:
                        return Color.FromHex("#F44336");
                    default:
                        return Color.FromHex("#4CAF50");
                }
            }
        }
    }
}
