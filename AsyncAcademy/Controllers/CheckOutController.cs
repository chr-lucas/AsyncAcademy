using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Stripe; 
using Stripe.Checkout;

namespace AsyncAcademy.Controllers
{
    public class PaymentRequest
    {
        public int amount { get; set; }
        public string cardHolderName { get; set; }
        public string cardNumber { get; set; }
        public int cvc { get; set; }
        public string description { get; set; }
    }



    [Route("create-checkout-session")]
    [ApiController]
    public class CheckoutApiController : Controller
    {




        [HttpPost]
        public async Task<ActionResult> CreateCheckoutSession()
        {
            var currentUserID = HttpContext.Session.GetInt32("CurrentUserId");
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
                SuccessUrl = "https://localhost:7082/PaymentSuccess?session_id={CHECKOUT_SESSION_ID}",  // Add session ID to the success URL
                //SuccessUrl = "https://localhost:7082/PaymentSuccess",  
                CancelUrl = "https://localhost:7082/PaymentCanceled",
                ClientReferenceId = currentUserID.ToString()
            };


            var service = new SessionService();
            Session session = service.Create(options);

            // Log the SuccessUrl for debugging
            Console.WriteLine("SuccessUrl sent to Stripe: " + options.SuccessUrl);

            TempData["Session"] = session.Id;
            //TempData["PaymentIntent"] = session.PaymentIntentId;//COMING NULL HERE BASED ON BREAKPOINT INFO
            // TempData["Receipt"] = session.PaymentIntent.LatestCharge.ReceiptUrl;//COMING NULL HERE BASED ON BREAKPOINT INFO

            return Json(new { id = session.Id });
        }


    }








    [Route("PaymentSuccess")]
    public class PaymentSuccessController : Controller
    {
        private readonly AsyncAcademyContext _context;

        public PaymentSuccessController(AsyncAcademyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> OnPaymentSuccess([FromQuery] string session_id)
        {
            // Fetch the Stripe session using the session ID
            var service = new SessionService();
            System.Diagnostics.Debug.WriteLine(Request);
            var session = await service.GetAsync(session_id);


            //TempData["card"] = session.PaymentMethodOptions.Card.RawJObject;

            // Check that the session was completed successfully
            if (session.PaymentStatus == "paid")
            {
                // Get the client reference ID (which is the userId) that we passed as "options"
                // in the checkout code 
                var userId = int.Parse(session.ClientReferenceId);

                // Get the total amount paid (Stripe stores amounts in cents)
                decimal amountPaid = (decimal)session.AmountTotal / 100m;

                // Find the user's payment record
                var paymentRecord = await _context.Payments.FirstOrDefaultAsync(s => s.UserId == userId);

                if (paymentRecord != null)
                {
                    // Update existing payment record
                    paymentRecord.AmountPaid += amountPaid;
                    paymentRecord.Timestamp = DateTime.Now;

                    _context.Payments.Update(paymentRecord);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Create a new payment record if none exists
                    var newPayment = new Payment
                    {
                        UserId = userId,
                        AmountPaid = amountPaid,
                        Timestamp = DateTime.Now
                    };
                    _context.Payments.Add(newPayment);
                    await _context.SaveChangesAsync();
                }



            }


            return RedirectToPage("/PaymentSuccess2", new { session_id = session_id });

        }


        //-----------------------THIS WILL BE THE NEW CHECKOUT CONTROLLER (AS OF 10/19/24)----------------------------


        //[Route("create-checkout-session")]
        //[ApiController]
        //public class CheckoutController : Controller
        //{
        //    [HttpPost]
        //    public async Task<ActionResult> CreateCheckOutSession([FromBody] PaymentRequest request)
        //    {
        //        var currentUserID = HttpContext.Session.GetInt32("CurrentUserId");
        //        var body = await new System.IO.StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        //        var parsedBody = JObject.Parse(body);
        //        var amount = parsedBody["amount"].Value<int>(); //Will read the amount field from JSON

        //        var options = new SessionCreateOptions
        //        {
        //            LineItems = new List<SessionLineItemOptions>
        //            {
        //                new SessionLineItemOptions
        //                {
        //                    PriceData = new SessionLineItemPriceDataOptions
        //                    {
        //                        UnitAmount = request.amount * 100,
        //                        Currency = "usd",
        //                        ProductData = new SessionLineItemPriceDataProductDataOptions
        //                        {
        //                            Name = "Tuition Due",
        //                        },
        //                    },
        //                    Quantity = 1,
        //                },
        //            },
        //            //new Payment

        //        };

        //        return null;
        //    }






            //[Route("api/webhooks/stripe")]
            //[ApiController]
            //public class WebHookController : Controller
            //{
            //    private readonly AsyncAcademyContext _context;

            //    public WebHookController(AsyncAcademyContext context)
            //    {
            //        _context = context;
            //    }

            //    [HttpPost]
            //    public async Task<IActionResult> HandleStripeEvent()
            //    {
            //        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            //        Console.WriteLine("Received Stripe webhook payload: " + json);
            //        var stripeSignature = Request.Headers["Stripe-Signature"];
            //        var STRIPE_WEBHOOK_SECRET = "whsec_6fdMyVUL25owPksvtY6vBNnaKxW9PXrd";
            //        Console.WriteLine("TESTING WEBHOOK!");

            //        try
            //        {
            //            var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, STRIPE_WEBHOOK_SECRET);
            //            Console.WriteLine("PaymentSuccess router was used");

            //            if (stripeEvent.Type == "checkout.session.completed")
            //            {
            //                var session = stripeEvent.Data.Object as Session;
            //                Console.WriteLine("TESTING WEBHOOK 1");

            //                //We retrieve payment details:
            //                var userId = int.Parse(session.ClientReferenceId);
            //                var amountPaid = session.AmountTotal / 100m; //Stripe stores amounts in cents, this converts it to dollars


            //                //Find the user in your database and update the Payments table
            //                var paymentRecord = await _context.Payments.FirstOrDefaultAsync(p => p.UserId == userId);

            //                if (paymentRecord != null)
            //                {
            //                    // If amountPaid is null, set it to 0 as a default value
            //                    paymentRecord.AmountPaid += amountPaid ?? 0m;
            //                    paymentRecord.Timestamp = DateTime.Now;

            //                    //We save the changes to the db:
            //                    _context.Payments.Update(paymentRecord);
            //                    await _context.SaveChangesAsync();

            //                    Console.WriteLine($"Payment record updated for user {userId}. Amount paid: {amountPaid}");
            //                }

            //                else
            //                {
            //                    Console.WriteLine($"No payment record found for user {userId}");
            //                }
            //            }

            //            return Ok();
            //        }

            //        catch (StripeException e)
            //        {
            //            Console.WriteLine($"Stripe webhook error: {e.Message}");
            //            return BadRequest(new { error = e.Message });
            //        }
            //    }
            //}

        }
    }


