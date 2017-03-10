using System.Data.Entity;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Configuration
{
    public class CityEntity
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            var city = modelBuilder.Entity<City>();            

            city
               .ToTable("Cities");

            city
               .HasKey(i => i.Id);

            city.Property(c => c.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            city    
               .Property(c => c.Name)
               .IsRequired();

            city
               .Property(c => c.Latitude)
               .IsRequired();

            city
               .Property(c => c.Longitude)
               .IsRequired();
                        
            city
               .HasMany(c => c.Responders)
               .WithOptional();

            city
              .HasRequired(c => c.AmbulancePosition);
        }
    }
}
