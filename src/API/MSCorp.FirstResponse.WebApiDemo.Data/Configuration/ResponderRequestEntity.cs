using System.Data.Entity;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Configuration
{
    public class ResponderRequestEntity
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            var responderRequest = modelBuilder.Entity<ResponderRequest>();

            responderRequest
                .ToTable("ResponderRequests");

            responderRequest
                .HasKey(i => i.Id);


            responderRequest
                .Property(i => i.DepartmentType)
                .IsRequired();
        }
    }
}
