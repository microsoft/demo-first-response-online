namespace MSCorp.FirstResponse.Client.Models
{
    public class UserRole
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string UserRoleImage { get; set; }
        public string DisplayUserName => Name.ToUpper();
    }
}