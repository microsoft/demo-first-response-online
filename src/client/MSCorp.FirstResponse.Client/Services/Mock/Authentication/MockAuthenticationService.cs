using MSCorp.FirstResponse.Client.Data;
using MSCorp.FirstResponse.Client.Data.Base;
using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.Services.Authentication;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Mock.Authentication
{
    class MockAuthenticationService : AuthenticationService
    {
        public static string DefaultUser = "jclarkson";

        protected readonly UserRole[] _userRoles;

        public MockAuthenticationService(IRequestProvider requestProvider) : base(requestProvider) {
            _userRoles = DataRepository.LoadUserRoles().ToArray();
        }

        public override async Task<UserRole> LoginAsync(string userName, string password)
        {
            var user = _userRoles.FirstOrDefault(x => string.Compare(x.UserName, userName,
                                                                                 StringComparison.OrdinalIgnoreCase) == 0);
            if (user == null)
            {
                user = _userRoles.FirstOrDefault(x => string.Compare(x.UserName, DefaultUser,
                                                                                 StringComparison.OrdinalIgnoreCase) == 0);
            }

            Settings.CurrentUser = user.UserName;

            await Task.Delay(500);

            return user;
        }
    }
}
