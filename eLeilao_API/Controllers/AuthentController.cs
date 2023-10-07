using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using DataAccessLayer.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eLeilao_API.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class AuthentController : ControllerBase
    {
        private readonly ILogger<AuthentController> _logger;
        private readonly IUsersRepository _usersRepo;
        private readonly IAuthRepository _authRepo;
        private readonly IDecToken _decToken;

        public AuthentController(ILogger<AuthentController> logger, IUsersRepository usersRepository, IAuthRepository authRepo, IDecToken decToken)
        {
            _logger = logger;
            _usersRepo = usersRepository;
            _authRepo = authRepo;
            _decToken = decToken;
        }
        [PrivilegeUser("Admin")]
        [HttpPost]
        public IActionResult CreatUsers(UserMV user, [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            string token = authorizationHeader.Substring("Bearer ".Length).Trim();
            var userLogged = _decToken.GetLoggedUser(token);
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (_usersRepo.AddUser(user, userLogged.UserId))
                    return Ok("User add");
                else
                    return BadRequest("User not add");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [PrivilegeUser("Admin")]
        [HttpPost]
        public IActionResult DelleteUser(int userId, [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            string token = authorizationHeader.Substring("Bearer ".Length).Trim();
            var userLogged = _decToken.GetLoggedUser(token);
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                if (_usersRepo.DeleteUser(userId, userLogged.UserId))
                    return Ok("User delete");
                else
                    return BadRequest("User not delete");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult Login(LoginModel login)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                var token = _authRepo.logIn(login);
                if (token != null)
                    return Ok(token);
                else
                    return BadRequest("User not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult GetUserLogged([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            string token = authorizationHeader.Substring("Bearer ".Length).Trim();
            try
            {
                var user = _decToken.GetLoggedUser(token);
                if (user != null)
                    return Ok(user);
                else
                    return BadRequest("User not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
