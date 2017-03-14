using MSCorp.FirstResponse.Client.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Models
{
    public class IncidentModel : ExtendedBindableObject
    {
        private IncidentType _incidentType;
        private bool _isHighPriority;
        private bool _readyToIdentify;
        private string _description;

        public static readonly Point AnchorPoint = new Point(0.5, 1);
        public int Id { get; set; }
        public string CallNumber { get; set; }
        public string Phone { get; set; }
        public string UnmaskedPhone { get; set; }
        public string Title { get; set; }
        public DateTime ReceivedTime { get; set; }
        public string PropertyName { get; set; }
        public string Address { get; set; }
        public string ReportingParty { get; set; }
        public string UnmaskedReportingParty { get; set; }
        public string Description {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                RaisePropertyChanged(()=> Description);
            }
        }
        public string UpdateDescription { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public ObservableCollection<ResponderRequestModel> Responders { get; set; } = new ObservableCollection<ResponderRequestModel>();
        public bool ReadyToIdentify
        {
            get { return _readyToIdentify; }
            set {
                _readyToIdentify = value;
                RaisePropertyChanged(() => ReadyToIdentify);
            }
        }
        public List<SuspectModel> Identities { get; set; } = new List<SuspectModel>();
        public DateTime? FullyAttendedTime { get; set; }
        public Color IncidentColor { get; set; }
        public Geoposition GeoLocation => new Geoposition { Latitude = Latitude, Longitude = Longitude };

        public SearchAreaModel SearchArea { get; set; }

        public IncidentType IncidentCategory
        {
            get { return _incidentType; }
            set
            {
                _incidentType = value;
                var data = IncidentHelper.GetIncidentData(_incidentType, IsHighPriority);
                if (data != null)
                {
                    IncidentIcon = data.Icon;
                    IncidentColor = data.Color;
                }
            }
        }

        public bool IsHighPriority
        {
            get
            {
                return _isHighPriority;
            }
            set
            {
                _isHighPriority = value;
                if (_incidentType != default(int))
                {
                    IncidentCategory = _incidentType;
                }
            }
        }

        public int Priority => _isHighPriority ? 1 : 0;

        public string IncidentIcon { get; set; }

        public IncidentStatus CurrentStatus
        {
            get
            {
                if (Responders.Any(x => !x.UnitResponded))
                {
                    return IncidentStatus.AwaitingResponders;
                }

                if (!FullyAttendedTime.HasValue)
                {
                    FullyAttendedTime = DateTime.Now;
                }

                if ((DateTime.Now - FullyAttendedTime) > GlobalSetting.TimeToResolve)
                {
                    return IncidentStatus.Resolved;
                }

                return IncidentStatus.Resolving;
            }
        }

        public void ResetModel()
        {
            var newRequests = new ObservableCollection<ResponderRequestModel>(Responders.Select(responderRequest => new ResponderRequestModel(responderRequest.DepartmentType)));
            Responders = newRequests;
            FullyAttendedTime = null;
            ReceivedTime = DateTime.Now;
        }
    }
}