using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
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

        public AuthentController(ILogger<AuthentController> logger, IUsersRepository usersRepository, IAuthRepository authRepo)
        {
            _logger = logger;
            _usersRepo = usersRepository;
            _authRepo = authRepo;
        }

        [HttpPost]
        public IActionResult CreatUsers(UserMV user, int UserId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (_usersRepo.AddUser(user, UserId))
                    return Ok("User add");
                else
                    return BadRequest("User not add");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult DelleteUser(int userId, int userLogged)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (_usersRepo.DeleteUser(userId, userLogged))
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
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string token = _authRepo.logIn(login);
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

    }
}
