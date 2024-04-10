using DataAccessLayer.Entites;
using DataAccessLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;

namespace PresentionLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenricRepository<DataAccessLayer.Entites.Product> _product;
        private readonly IGenricRepository<Orders> _orders;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly int productNum;
        private readonly StripeSettings _stripeSettings;

        const string SessionUserId = "_UserId";
        const string SessionOederId = "_OrderId";
        public string SessionId { get; set; }

        public HomeController(ILogger<HomeController> logger,
            IGenricRepository<DataAccessLayer.Entites.Product> product,
            IOptions<StripeSettings> stripeSettings,
            IGenricRepository<Orders> orders,
            UserManager<ApplicationUser> userManager)
        {
            _stripeSettings = stripeSettings.Value;
            _logger = logger;
            _product = product;
            _orders = orders;
            _userManager = userManager;
            productNum = 4;
        }
        
        public IActionResult Index()
        {
            var products = new IndexViewModel
            {
                mobileProducts = _product.GetAll().Where(c => c.categoryId.ToString() == "16f8b600-be60-45c9-ae68-5711cd3966e6").Take(productNum).ToList(),
                smartWatches = _product.GetAll().Where(c => c.categoryId.ToString() == "8f0410b4-aad1-4bf2-a6d8-e5f155f3fcf2").Take(productNum).ToList()
            };
            return View(products);
        }

        public IActionResult Mobiles()
        {
            var mobiles = new IndexViewModel
            {
                mobileProducts = _product.GetAll().Where(c => c.categoryId.ToString() == "16f8b600-be60-45c9-ae68-5711cd3966e6").ToList(),
            };
            string UserId = HttpContext.Session.GetString(SessionUserId);

            return View(mobiles);
        }
        public IActionResult Watches()
        {
            var watches = new IndexViewModel
            {
                smartWatches = _product.GetAll().Where(c => c.categoryId.ToString() == "8f0410b4-aad1-4bf2-a6d8-e5f155f3fcf2").ToList()
            };
            return View(watches);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Search(string searchTerm)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(searchTerm))
                    return RedirectToAction("Index");

                var product = _product.GetAll().
                    Where(p => p.name.ToLower().Contains(searchTerm.ToLower())
                    || p.price.ToString().Contains(searchTerm)
                        ).ToList();

                return View(product);
            }
            else
                return RedirectToAction("Index");
        }

        public IActionResult CreateCheckoutSession(string amount, string name, Guid productId)
        {
            string UserId = HttpContext.Session.GetString(SessionUserId);

            // Retrieve values from form fields
            var currency = "usd"; // Currency code
            var successUrl = "https://localhost:7025/Home/success";
            var cancelUrl = "https://localhost:7025/Home/cancel";

            StripeConfiguration.ApiKey = "sk_test_51OxGo02NbbgLR1ovBaDTuwyy6ZWdGDc4jAFeLg0IcA2Bv9HQGhA29Av8cDTq6N5jYUWJsRqMtgdAZliVbBOOICCA00wF4knawQ";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = currency,
                            UnitAmount = Convert.ToInt32(amount) * 100,  // Amount in smallest currency unit (e.g., cents)
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = name,
                                Description = "Product Description"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl
            };

            var service = new SessionService();
            var session = service.Create(options);
            SessionId = session.Id;

            var order = new Orders()
            {
                orderDate = DateTime.Now,
                totalAmout = Convert.ToInt32(amount),
                UserId = UserId,
                productId = productId,
                shippingAddress = _userManager.FindByIdAsync(UserId).Result.address
            };
            _orders.Create(order);
            _orders.Save();
            HttpContext.Session.SetString(SessionOederId, order.Id.ToString());


            return Redirect(session.Url);
        }

        public IActionResult success()
        {
            string orderIdString = HttpContext.Session.GetString(SessionOederId);
            if (Guid.TryParse(orderIdString, out Guid orderId))
            {
                var order = _orders.GetById(orderId);
                order.statues = "success";
                _orders.Save();
            }
            return View();
        }

        public IActionResult cancel()
        {
            string orderIdString = HttpContext.Session.GetString(SessionOederId);
            if (Guid.TryParse(orderIdString, out Guid orderId))
            {
                var order = _orders.GetById(orderId);
                order.statues = "cancel";
            }
            return RedirectToAction("Index");
        }

    }
}
