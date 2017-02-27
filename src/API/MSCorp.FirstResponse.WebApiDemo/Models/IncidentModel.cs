using System;

namespace MSCorp.FirstResponse.WebApiDemo.Models
{
    /// <summary>
    /// Model of incident data
    /// </summary>
    public class IncidentModel
    {
        public int Id { get; set; }
        public int DepartmentType { get; set; }
        public string ShardName { get; set; }
        public int CityId { get; set; }
        public string CallNumber { get; set; }
        public string Phone { get; set; }
        public string UnmaskedPhone { get; set; }
        public string Title { get; set; }
        public DateTime ReceivedTime { get; set; }
        public string Address { get; set; }
        public string ReportingParty { get; set; }
        public string UnmaskedReportingParty { get; set; }
        public string Description { get; set; }
        public string UpdateDescription { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public bool IsHighPriority { get; set; }
        public IncidentType IncidentCategory { get; set; }
        public int? SearchAreaId { get; set; }

    }
}