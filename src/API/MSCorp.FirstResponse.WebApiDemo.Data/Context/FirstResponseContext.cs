using Microsoft.AspNet.Identity.EntityFramework;
using MSCorp.FirstResponse.WebApiDemo.Data.Configuration;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;
using System.Data.Entity;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Context
{
    public class FirstResponseContext : IdentityDbContext<User, IdentityRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public FirstResponseContext() : base("DefaultConnection")
        {
        }

        public DbSet<Responder> Responders { get; set; }
        public DbSet<ResponderRoute> ResponderRoutes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<ResponderRequest> ResponderRequests { get; set; }
        public DbSet<HeatMapPoint> HeatMapPoints { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            ResponderEntity.Configure(modelBuilder);
            ResponderRouteEntity.Configure(modelBuilder);
            RoutePointEntity.Configure(modelBuilder);
            CityEntity.Configure(modelBuilder);
            DriverEntity.Configure(modelBuilder);
            VehicleEntity.Configure(modelBuilder);
            ResponderRequestEntity.Configure(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
