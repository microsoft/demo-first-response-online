using System.ComponentModel.DataAnnotations;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Models
{
    public class LoginInputModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}