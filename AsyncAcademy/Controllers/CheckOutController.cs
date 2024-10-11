using AsyncAcademy.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Stripe.Checkout;

namespace AsyncAcademy.Controllers
{
    [Route("create-checkout-session")]
    [ApiController]
    public class CheckoutApiController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> CreateCheckoutSession()
        {
            var body = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var parsedBody = JObject.Parse(body);
            var amount = parsedBody["amount"].Value<int>(); //Will read the amount field from JSON

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = amount * 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Tuition Due",
                        },
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = "http://localhost:7082/PaymentSuccess",
                CancelUrl = "http://localhost:4242/cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Json(new { id = session.Id });
        }


    }


    //API controller to handle successful payments and update the payment amount info in the db:
    [Route("PaymentSuccess")]
    public class PaymentSuccessController : Controller
    {
        private readonly AsyncAcademyContext _context;
        public PaymentSuccessController(AsyncAcademyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> OnPaymentSuccess(int userId, decimal amountPaid)
        {
            var studentPaymentRecord = await _context.StudentPayment.FirstOrDefaultAsync(s => s.UserId == userId);

            if(studentPaymentRecord != null)
            {
                //We update the outstanding amount here:
                studentPaymentRecord.Outstanding -= amountPaid;

                //We now update the total amount paid so far:
                studentPaymentRecord.TotalPaid += amountPaid;

                //Time stamp when this payments happened:
                studentPaymentRecord.LastUpdated = DateTime.Now;

                //Save all the changes to the db:
                _context.StudentPayment.Update(studentPaymentRecord);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Account", "YourAccountPageController");
        }
    }

}