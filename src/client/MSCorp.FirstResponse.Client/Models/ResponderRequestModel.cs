using MSCorp.FirstResponse.Client.Helpers;
using Newtonsoft.Json;
using System.Drawing;

namespace MSCorp.FirstResponse.Client.Models
{
    public class ResponderRequestModel : ExtendedBindableObject
    {
        private string _responseId;

        private string _responseColor;

        public DepartmentType DepartmentType { get; set; }
        public bool UnitResponding { get; private set; }
        public bool UnitResponded { get; set; }
        public string ResponseId
        {
            get { return _responseId; }
            set
            {
                _responseId = value;
                RaisePropertyChanged(() => ResponseId);
            }
        }
        public string ResponseColor
        {
            get { return _responseColor ?? "Transparent"; }
            set {
                _responseColor = value;
                RaisePropertyChanged(() => ResponseColor);
            }
        }

        public ResponderRequestModel() {}

        public ResponderRequestModel(DepartmentType departmentType)
        {
            DepartmentType = departmentType;
        }

        public void RespondToRequest(ResponseStatus responseStatus)
        {
            if (!UnitResponding && responseStatus == ResponseStatus.Available)
            {
                UnitResponding = true;
            }
        }
    }
}