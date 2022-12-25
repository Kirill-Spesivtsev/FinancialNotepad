using FinancialNotepad.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace FinancialNotepad.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger _logger;

        
        public SubscriptionController(ApplicationDbContext context, ILogger<TransactionController> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MakePayment(string amount)
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount =Convert.ToInt32(amount),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "SubscriptionMonth",
                            },

                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = "/Subscription/Success",
                CancelUrl = "/Subscription/Cancel",
            };

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return View();
        }
    }
}
