using MSCorp.FirstResponse.Client.Models;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<UserRole> LoginAsync(string userName, string password);

        Task<bool> LogoutAsync();
    }
}
