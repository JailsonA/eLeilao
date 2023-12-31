using DataAccessLayer.IRepository;
using DataAccessLayer.Model;
using DataAccessLayer.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
        private readonly IDecToken _decToken;

        public AuctionController(ILogger<AuctionController> logger,
                            IProductRepository productRepo, IAuctionRepository auctionRepo,
                            IUsersRepository usersRepo, IDecToken decToken)
        {
            _logger = logger;
            _productRepo = productRepo;
            _auctionRepo = auctionRepo;
            _usersRepo = usersRepo;
            _decToken = decToken;
        }


        [PrivilegeUser("Admin")]
        [HttpPost]
        public IActionResult CreateProduct(ProductMV product, [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            string token = authorizationHeader.Substring("Bearer ".Length).Trim();
            var userLogged = _decToken.GetLoggedUser(token);
            product.UserId = userLogged.UserId;
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

        [PrivilegeUser("Admin")]
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

        [PrivilegeUser("Admin")]
        [HttpPost]
        public IActionResult CreateAuction(AuctionMV auctionMV, [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            string token = authorizationHeader.Substring("Bearer ".Length).Trim();
            var userLogged = _decToken.GetLoggedUser(token);
            auctionMV.CreatorUserId = userLogged.UserId;
            try
            {
                if (!ModelState.IsValid) return BadRequest("modelstate not valid");
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

        [PrivilegeUser("Admin")]
        [HttpPost]
        public async Task<IActionResult> AddMessage(int auctionId, string message)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                if (_auctionRepo.AddAuctionMessage(auctionId, message))
                {
                    var auction = _auctionRepo.GetAuctionById(auctionId);
                    return Ok("Message add");
                }
                else
                    return BadRequest("Message not add");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [PrivilegeUser("Admin")]
        [HttpGet]
        public IActionResult GetAllAdmAuction([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            string token = authorizationHeader.Substring("Bearer ".Length).Trim();
            var userLogged = _decToken.GetLoggedUser(token);
            try
            {
                var auctions = _auctionRepo.GetAllAuction(userLogged.UserId);
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


        [PrivilegeUser("User")]
        [HttpGet]
        public IActionResult GetAllAuctionbyUser([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            string token = authorizationHeader.Substring("Bearer ".Length).Trim();
            var userLogged = _decToken.GetLoggedUser(token);
            try
            {
                var auctions = _auctionRepo.GetAuctionByUser(userLogged.UserId);
                if (auctions != null)
                    return Ok(auctions);
                else
                    return BadRequest(null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //update auction status
        [PrivilegeUser("Admin")]
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
        [PrivilegeUser("Admin")]
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
        [PrivilegeUser("Admin")]
        [HttpPost]
        public IActionResult DeleteAuction(int auctionId, [FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            try
            {
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


        [PrivilegeUser("Admin")]
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

        [PrivilegeUser("User")]
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

        [PrivilegeUser("User")]
        [HttpGet]
        public IActionResult GetAllBids([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            string token = authorizationHeader.Substring("Bearer ".Length).Trim();
            var userLogged = _decToken.GetLoggedUser(token);
            try
            {
                var bids = _auctionRepo.GetAllBids(userLogged.UserId);
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
        public IActionResult getString()
        {
            return Ok("Hello World");
        }

    }
}