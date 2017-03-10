using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Entities
{
    public class AmbulancePosition
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
