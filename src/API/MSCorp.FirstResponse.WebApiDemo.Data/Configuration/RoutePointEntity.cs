using System.Data.Entity;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Configuration
{
    public class RoutePointEntity
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            var routePoint = modelBuilder.Entity<RoutePoint>();

            routePoint
                .Property(oi => oi.Id);
        }
    }
}
