using MSCorp.FirstResponse.WebApiDemo.Data.Enums;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Entities
{
    public class Responder
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public DepartmentType ResponderDepartment { get; set; }
        public ResponseStatus Status { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int RouteId { get; set; }
        public ResponderRoute Route { get; set; }
    }
}
