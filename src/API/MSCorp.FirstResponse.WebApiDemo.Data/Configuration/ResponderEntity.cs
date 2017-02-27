using System.Data.Entity;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Configuration
{
    public class ResponderEntity
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            var responder = modelBuilder.Entity<Responder>();

            responder
                .ToTable("Responders");

            responder
                .HasKey(i => i.Id);
        }
    }
}
