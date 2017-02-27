using System.Data.Entity;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Configuration
{
    public class DriverEntity
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            var driver = modelBuilder.Entity<Driver>();

            driver
                .ToTable("Drivers");

            driver
                .HasKey(i => i.Id);

            driver
             .Property(c => c.LicenceNo)
             .IsRequired();
        }
    }
}
