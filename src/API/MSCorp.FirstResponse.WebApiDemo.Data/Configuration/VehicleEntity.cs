using System.Data.Entity;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Configuration
{
    public class VehicleEntity
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            var vehicle = modelBuilder.Entity<Vehicle>();

            vehicle
                .ToTable("Vehicles");

            vehicle
                .HasKey(i => i.Id);

            vehicle
                .Property(v => v.PlateNo)
                .IsRequired();
        }
    }
}
