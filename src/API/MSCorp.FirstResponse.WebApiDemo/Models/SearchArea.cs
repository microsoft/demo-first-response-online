using System.Collections.Generic;

namespace MSCorp.FirstResponse.WebApiDemo.Models
{
    public class SearchArea
    {
        public int Id { get; set; }
        public ICollection<PolygonPoint> Polygon { get; set; } = new List<PolygonPoint>();
    }
}