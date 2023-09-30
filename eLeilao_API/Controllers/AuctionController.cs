using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Mvc;
namespace eLeilao_API.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class AuctionController : ControllerBase
    {
        private readonly ILogger<AuctionController> _logger;
        private readonly IProductRepository _productRepo;

        public AuctionController(ILogger<AuctionController> logger, IUsersRepository usersRepository, IProductRepository productRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
        }



        [HttpPost]
        public IActionResult CreateProduct(ProductMV product)
        {
            {
                try
                {
                    if (!ModelState.IsValid) return BadRequest(ModelState);
                    if (_productRepo.AddProduct(product))
                        return Ok("Product add");
                    else
                        return BadRequest("Product not add");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
        }
    }
}