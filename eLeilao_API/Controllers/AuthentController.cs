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

        public AuthentController(ILogger<AuthentController> logger, IUsersRepository usersRepository)
        {
            _logger = logger;
            _usersRepo = usersRepository;
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
    }
}
