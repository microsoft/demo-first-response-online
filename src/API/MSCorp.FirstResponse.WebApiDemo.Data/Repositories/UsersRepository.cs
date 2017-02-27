using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using MSCorp.FirstResponse.WebApiDemo.Data.Context;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly FirstResponseContext _context;

        public UsersRepository(FirstResponseContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(string userName)
        {
            return await _context
                .Users
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }

}

