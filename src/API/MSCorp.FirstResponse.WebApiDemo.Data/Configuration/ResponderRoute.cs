using System.Data.Entity;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Configuration
{
    public class ResponderRouteEntity
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            var responderRoute = modelBuilder.Entity<ResponderRoute>();
        }
    }
}
