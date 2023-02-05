using FinancialNotepad.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using FinancialNotepad.Models;
using Subscription = FinancialNotepad.Models.Subscription;

namespace FinancialNotepad.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger _logger;

        private int amount = 6;

        
        public SubscriptionController(ApplicationDbContext context, ILogger<TransactionController> logger, UserManager<IdentityUser> userManager)
        {
            
            _context = context;
            _logger = logger;
            _userManager = userManager;
            
        }

        public IActionResult Index()
        {
            return View("Checkout");
        }

        [HttpPost]
        public ActionResult MakePayment()
        {

            var basepath = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = Convert.ToInt32(amount) * 100,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Subscription Month",
                            },

                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = basepath + @"/Subscription/Success",
                CancelUrl = basepath + @"/Subscription/Cancel",
            };

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        
        public async Task<IActionResult> Success()
        {
            string userId = _userManager.GetUserId(User);
            var sub = new Subscription
            {
                CurrencyId = 1, 
                DateStart = DateTime.Now, 
                DateEnd = DateTime.Now + TimeSpan.FromDays(30), 
                PayPrice = 10,
                UserId = userId
            };
            _context.Subscriptions.Add(sub);
            IdentityUser user = _userManager.FindByIdAsync(userId).Result;
            await _userManager.AddToRoleAsync(user, "SUBSCRIBER");
            await _context.SaveChangesAsync();
            return View("Success");
        }
    }
}
