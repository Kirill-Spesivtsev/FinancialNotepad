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
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger _logger;

        private int amount = 6;

        public SubscriptionController(ApplicationDbContext context, ILogger<TransactionController> logger, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public IActionResult Index()

        {
            string userId = _userManager.GetUserId(User);
            var sub = _context.Subscriptions.FirstOrDefault(s => s.UserId == userId);
            ViewBag.UserSubscribed = sub != null;
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Cancel()
        {
            string userId = _userManager.GetUserId(User);
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            var sub = _context.Subscriptions.FirstOrDefault(s => s.UserId == userId);
            var service = new SubscriptionService();
            var cancelOptions =  new SubscriptionCancelOptions
            {
                InvoiceNow = false,
                Prorate = false,
            };
            await _userManager.RemoveFromRoleAsync(user,"SUBSCRIBER");
            
            var rem = _context.Subscriptions.Where(s => s.SubId == sub.SubId);
            _context.Subscriptions.RemoveRange(rem);
            try
            {
                service.Cancel(sub.SubId, cancelOptions);
            }
            catch (StripeException)
            {
                
            }
            await _context.SaveChangesAsync();
            await _signInManager.SignInAsync(user, false);
            return View("Cancel");
        }

        [HttpPost]
        public ActionResult Subscribe()
        {
            /*
            var productOptions = new ProductCreateOptions
            {
                Name = "Premium Subscription",
            };
            var productService = new ProductService();
            var product = productService.Create(productOptions);


            var priceOptions = new PriceCreateOptions
            {
                UnitAmount = Convert.ToInt32(amount) * 100,
                Currency = "usd",
                Recurring = new PriceRecurringOptions
                {
                    Interval = "month",
                },
                Product = product.Id,
            };
            var priceService = new PriceService();
            var price = priceService.Create(priceOptions);
            */

            var basePath = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";


            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = "price_1MZ6zALeIaVC1rxLvzf96ab0",
                        Quantity = 1,
                    },
                },
                Mode = "subscription",
                SuccessUrl = basePath + @"/Subscription/Success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = basePath + @"/Subscription/Cancel",

            };
            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }


        [HttpPost]
        public IActionResult MakePayment()
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

        
        public async Task<IActionResult> Success(string session_id)
        {
            string userId = _userManager.GetUserId(User);
            var service = new SessionService();
            var session = service.Get(session_id);
            var subId = session.SubscriptionId;
            var sub = _context.Subscriptions.FirstOrDefault(s => s.UserId == userId);
            if (sub == null)
            {
                sub = new Subscription
                {
                    CurrencyId = 1,
                    PayPrice = amount,
                    UserId = userId,
                    DateStart = DateTime.Now,
                    SubId = session.SubscriptionId,
                    CusId = session.CustomerId
                };
                _context.Subscriptions.Add(sub);
            }
            else
            {
                sub.DateStart = DateTime.Now;
                sub.PayPrice = amount;
                sub.SubId = session.SubscriptionId;
                _context.Subscriptions.Update(sub);
            }
            IdentityUser user = await _userManager.FindByIdAsync(userId);
            await _userManager.AddToRoleAsync(user, "SUBSCRIBER");
            await _context.SaveChangesAsync();
            await _signInManager.SignInAsync(user, false);
            return View("Success");
        }
    }
}
