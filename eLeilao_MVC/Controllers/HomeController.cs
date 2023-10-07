using eLeilao_MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.SignalR;

namespace eLeilao_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;

        public HomeController(HttpClient httpClient)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5262/");
        }

        public async Task<IActionResult> Index()
        {
            string _token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(_token))
            {
                return RedirectToAction("Index");
            }

            var userJson = HttpContext.Session.GetString("User");

            if (string.IsNullOrEmpty(userJson))
            {
                var user = await GetUser(_token);
                userJson = JsonConvert.SerializeObject(user);
                HttpContext.Session.SetString("User", userJson);
            }

            var getUser = JsonConvert.DeserializeObject<UserMV>(userJson);

            var viewModel = new DashboardMV
            {
                Auctions = await GetAuctions(_token),
                Users = getUser,
                AuctionsByUser = await GetAuctionsByUser(_token),
                Bids = await GetAllBids(_token)
            };

            return View(viewModel);
        }

        public IActionResult Login()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return View();
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginMV login)
        {
            if (!ModelState.IsValid) return View(login);

            string apiUrl = "Authent/Login";

            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Token");

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent != null)
                {
                    HttpContext.Session.SetString("Token", responseContent);
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login");
                }

            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> Product()
        {
            string _token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(_token))
            {
                return RedirectToAction("Index");
            }

            var user = await GetUser(_token);
            if (user.UserType != "Admin")
            {
                return RedirectToAction("Index");
            }

            ViewBag.Products = await GetProducts(_token);
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductMV product)
        {

            string token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync("Auction/CreateProduct", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Product");
            }
            else
            {
                return RedirectToAction("Erro");
            }
        }

        public async Task<IActionResult> CreatAuction()
        {
            string _token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(_token))
            {
                return RedirectToAction("Index");
            }

            var user = await GetUser(_token);
            if (user.UserType != "Admin")
            {
                return RedirectToAction("Index");
            }

            ViewBag.Products = await GetProducts(_token);

            return View();
        }

        public async Task<IActionResult> UpdateAuction(int AuctionId)
        {
            string _token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(_token))
            {
                return RedirectToAction("Index");
            }

            var user = await GetUser(_token);
            if (user.UserType != "Admin")
            {
                return RedirectToAction("Index");
            }

            ViewBag.Products = await GetProducts(_token);
            var auction = GetAuctions(_token).Result.FirstOrDefault(x => x.AuctionId == AuctionId);

            return View(auction);
        }

        [HttpPost]
        public IActionResult UpdateAuction(AuctionMV auction)
        {
            string token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(auction), Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync("Auction/UpdateAuction", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(auction);
            }
        }

        public async Task<IActionResult> SeeAuction(int AuctionId)
        {
            string _token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(_token))
            {
                return RedirectToAction("Index");
            }

            var user = await GetUser(_token);
            if (user.UserType != "Admin")
            {
                return RedirectToAction("Index");
            }

            var auction = GetAuctions(_token).Result.FirstOrDefault(x => x.AuctionId == AuctionId);
            return View(auction);
        }

        //AuctionStatus 
        [HttpPost]
        public IActionResult AuctionStatus(int AuctionId)
        {
            string token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(AuctionId), Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync($"Auction/AuctionStatus?AuctionId={AuctionId}", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        //delet auction
        [HttpPost]
        public async Task<IActionResult> DeleteAuction(int AuctionId)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.PostAsync($"Auction/DeleteAuction?auctionId={AuctionId}", null);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Erro");
            }
        }


        public async Task<IActionResult> Bidders(int AuctionId)
        {
            string _token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(_token))
            {
                return RedirectToAction("Index");
            }

            var user = await GetUser(_token);
            if (user.UserType != "Admin")
            {
                return RedirectToAction("Index");
            }

            var newbiders = new CreateBiddersModel
            {
                Users = await GetUserByAuction(_token, AuctionId),
                Auction = GetAuctions(_token).Result.FirstOrDefault(x => x.AuctionId == AuctionId)
            };

            return View(newbiders);
        }

        [HttpPost]
        public IActionResult CreateBidders(string T_fullName, string T_contact, string T_email, string T_password, int T_AuctionId)
        {
            int auctionId = Convert.ToInt32(T_AuctionId); // Use "auctionId" here
            var user = new CreatbidMV
            {
                FullName = T_fullName,
                Contact = T_contact,
                Email = T_email,
                Password = T_password,
                AuctionId = auctionId // Use "auctionId" here
            };

            string token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync("Authent/CreatUsers", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Bidders", new { AuctionId = T_AuctionId });
            }
            else
            {
                return RedirectToAction("Erro");
            }
        }


        //delet bidders
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int userId, int auctionId)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.PostAsync($"Authent/DelleteUser?userId={userId}", null);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Bidders", new { AuctionId = auctionId });
                }
                else
                {
                    return RedirectToAction("Bidders", new { AuctionId = auctionId });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Erro");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        /* Get all appointments by user id */
        private async Task<List<AuctionMV>> GetAuctions(string token)
        {
            string apiUrl = "Auction/GetAllAdmAuction";

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var genericModels = JsonConvert.DeserializeObject<List<AuctionMV>>(responseContent);
                    return genericModels;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // get auctio GetAllAuctionbyUser 
        private async Task<AuctionMV> GetAuctionsByUser(string token)
        {
            string apiUrl = "Auction/GetAllAuctionbyUser";

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var genericModels = JsonConvert.DeserializeObject<AuctionMV>(responseContent);
                    return genericModels;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /* Get user Logged Info */
        private async Task<UserMV> GetUser(string token)
        {
            string apiUrl = "Authent/GetUserLogged";

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    UserMV user = JsonConvert.DeserializeObject<UserMV>(responseContent);

                    return user;
                }
                else
                {
                    return new UserMV();
                }
            }
            catch (Exception ex)
            {
                return new UserMV();
            }
        }

        //get al products
        private async Task<List<ProductMV>> GetProducts(string token)
        {
            string apiUrl = "Auction/GetAllProduct";

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    List<ProductMV> product = JsonConvert.DeserializeObject<List<ProductMV>>(responseContent);

                    return product;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private async Task<List<UserMV>> GetUserByAuction(string token, int auctionId)
        {
            string apiUrl = $"Auction/GetAllUserByAuction?auctionId={auctionId}";

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    List<UserMV> user = JsonConvert.DeserializeObject<List<UserMV>>(responseContent);

                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //create auction
        [HttpPost]
        public IActionResult CreateAuction(AuctionMV auction)
        {
            string token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(auction), Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync("Auction/CreateAuction", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("CreatAuction");
            }
            else
            {
                return View("CreatAuction", auction);
            }
        }

        //send SendMessage 
        [HttpPost]
        public IActionResult SendMessage(string message, int auctionId)
        {
            string token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = _httpClient.PostAsync($"Auction/AddMessage?AuctionId={auctionId}&message={message}", null).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("SeeAuction", new { AuctionId = auctionId });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // add bid
        [HttpPost]
        public IActionResult AddBid(int userId, int auctionId, float bidValue)
        {
            var bid = new AddBidMV
            {
                UserId = userId,
                AuctionId = auctionId,
                BidValue = bidValue
            };

            string token = HttpContext.Session.GetString("Token");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(bid), Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync("Auction/AddBid", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //get all bids
        private async Task<List<BidMV>> GetAllBids(string token)
        {
            string apiUrl = $"Auction/GetAllBids";

            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    List<BidMV> bid = JsonConvert.DeserializeObject<List<BidMV>>(responseContent);

                    return bid;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //logout clear session
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login");
        }
    }
}