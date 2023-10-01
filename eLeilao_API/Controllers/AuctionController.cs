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
        private readonly IAuctionRepository _auctionRepo;
        private readonly IUsersRepository _usersRepo;


        public AuctionController(ILogger<AuctionController> logger, IUsersRepository usersRepository, 
                            IProductRepository productRepo, IAuctionRepository auctionRepo,
                            IUsersRepository usersRepo)
        {
            _logger = logger;
            _productRepo = productRepo;
            _auctionRepo = auctionRepo;
            _usersRepo = usersRepo;
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
        [HttpGet]
        public IActionResult GetAllProduct()
        {
            try
            {
                var products = _productRepo.GetAllProduct();
                if (products != null)
                    return Ok(products);
                else
                    return BadRequest("Product not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult CreateAuction(AuctionMV auctionMV)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (_auctionRepo.CreateAuction(auctionMV))
                    return Ok("Auction add");
                else
                    return BadRequest("Auction not add");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult AddMessage(int auctionId, string message)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (_auctionRepo.AddAuctionMessage(auctionId, message))
                    return Ok("Message add");
                else
                    return BadRequest("Message not add");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult GetAllAdmAuction(int userId)
        {
            try
            {
                var auctions = _auctionRepo.GetAllAuction(userId);
                if (auctions != null)
                    return Ok(auctions);
                else
                    return BadRequest("Auction not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //update auction status
        [HttpPost]
        public IActionResult AuctionStatus(int auctionId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (_auctionRepo.AuctionStatus(auctionId))
                    return Ok("Auction status update");
                else
                    return BadRequest("Auction status not update");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //update auction info
        [HttpPost]
        public IActionResult UpdateAuction(AuctionMV auctionMV)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (_auctionRepo.UpdateAuction(auctionMV))
                    return Ok("Auction update");
                else
                    return BadRequest("Auction not update");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //delete auction by update status
        [HttpPost]
        public IActionResult DeleteAuction(int auctionId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (_auctionRepo.DeleteAuction(auctionId))
                    return Ok("Auction delete");
                else
                    return BadRequest("Auction not delete");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult AddBid(BidMV bidMV)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (_auctionRepo.AddBid(bidMV))
                    return Ok("Bid add");
                else
                    return BadRequest("Bid not add");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult GetAllBids(int userId)
        {
            try
            {
                var bids = _auctionRepo.GetAllBids(userId);
                if (bids != null)
                    return Ok(bids);
                else
                    return BadRequest("Bid not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult GetAllUsers(int userLogged)
        {
            try
            {
                var users = _usersRepo.GetAllUsers(userLogged);
                if (users != null)
                    return Ok(users);
                else
                    return BadRequest("Users not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult GetAllUserByAuction(int auctionId)
        {
            try
            {
                var users = _usersRepo.GetAllUsersByAuction(auctionId);
                if (users != null)
                    return Ok(users);
                else
                    return BadRequest("Users not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}