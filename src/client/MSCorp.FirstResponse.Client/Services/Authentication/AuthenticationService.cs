using MSCorp.FirstResponse.Client.Data.Base;
using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.Models.Users;
using System;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IRequestProvider _requestProvider;

        public AuthenticationService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public bool IsAuthenticated => Settings.CurrentUser != null;

        public virtual async Task<UserRole> LoginAsync(string userName, string password)
        {
            var auth = new AuthenticationRequest
            {
                UserName = userName,
                Password = password
            };

            UriBuilder builder = new UriBuilder(Settings.ServiceEndpoint);
            builder.Path = "api/accounts/login";

            string uri = builder.ToString();

            try
            {
                var user = await _requestProvider.PostAsync<AuthenticationRequest, UserRole>(uri, auth);
                if (user != null)
                {
                    Settings.CurrentUser = userName;
                }

                return user;
            } catch
            {
                return null;
            }


        }

        public Task<bool> LogoutAsync()
        {
            Settings.RemoveCurrentUser();

            return Task.FromResult(false);
        }
    }
}
