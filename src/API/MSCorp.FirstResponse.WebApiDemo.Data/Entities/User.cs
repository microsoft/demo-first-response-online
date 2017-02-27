using Microsoft.AspNet.Identity.EntityFramework;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string RoleName { get; set; }
        public string UserRoleImage { get; set; }
    }
}
