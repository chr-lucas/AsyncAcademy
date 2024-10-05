using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                SuccessUrl = "http://localhost:4242/success",  // Change URLs to your actual app
                CancelUrl = "http://localhost:4242/cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Json(new { id = session.Id });
        }
    }

}
