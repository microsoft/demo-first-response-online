using MSCorp.FirstResponse.Client.ViewModels.Base;

namespace MSCorp.FirstResponse.Client.Models
{
    public class DriverModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string LicenceNo { get; set; }
        public string HomeAddress { get; set; }
        public string Phone { get; set; }
    }
}