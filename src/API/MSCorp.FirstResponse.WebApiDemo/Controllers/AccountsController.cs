using Microsoft.AspNet.Identity;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;
using MSCorp.FirstResponse.WebApiDemo.Data.Models;
using MSCorp.FirstResponse.WebApiDemo.Data.Repositories;
using System.Threading.Tasks;
using System.Web.Http;

namespace MSCorp.FirstResponse.WebApiDemo.Controllers
{

    [RoutePrefix("api/accounts")]
    public class AccountsController : ApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly IUsersRepository _usersRepository;

        public AccountsController(
            UserManager<User> userManager, 
            IUsersRepository usersRepository)
        {
            _userManager = userManager;
            _usersRepository = usersRepository;
        }

        [HttpGet]
        [Route("user/{userName}")]
        public async Task<User> GetUser(string userName)
        {
          return await _usersRepository.GetUser(userName);
        }

        // POST api/accounts/register
        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            await _userManager.CreateAsync(user, user.PasswordHash);

            return Ok();
        }

        // POST api/accounts/login
        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Login([FromBody] LoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.FindByNameAsync(model.Username);

                if (identityUser != null && await _userManager.CheckPasswordAsync(identityUser, model.Password))
                {
                    return Ok(new { RoleName = identityUser.RoleName, Name = identityUser.UserName, UserRoleImage = identityUser.UserRoleImage });
                }

                return Unauthorized();
            }

            return BadRequest();
        }

    }
}