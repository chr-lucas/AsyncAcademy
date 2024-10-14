using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Stripe; 
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
                //SuccessUrl = $"http://localhost:7082/PaymentSuccess?session_id={{CHECKOUT_SESSION_ID}}",  // Add session ID to the success URL
                SuccessUrl = $"http://localhost:7082/PaymentSuccess",  
                CancelUrl = "http://localhost:4242/PaymentCanceled",
                ClientReferenceId = currentUserID.ToString()
            };


            var service = new SessionService();
            Session session = service.Create(options);

            // Log the SuccessUrl for debugging
            Console.WriteLine("SuccessUrl sent to Stripe: " + options.SuccessUrl);

            TempData["Session"] = session.Id;

            return Json(new { id = session.Id });
        }


    }


    //[Route("PaymentSuccess")]
    //public class PaymentSuccessController : Controller
    //{
    //    [HttpGet]
    //    public async Task<IActionResult> PaymentSuccess([FromQuery] string session_id)
    //    {
    //        if (string.IsNullOrEmpty(session_id))
    //        {
    //            return BadRequest("Session ID is missing");
    //        }

    //        Console.WriteLine("TESTE TEST");

    //        // Use the session ID to retrieve session details from Stripe
    //        var service = new SessionService();
    //        Session session = await service.GetAsync(session_id);

    //        // (Optional) If you need more details, you can query the customer or other related objects.
    //        // For example, if you want to retrieve the customer associated with this session:
    //        var customerService = new CustomerService();
    //        var customer = await customerService.GetAsync(session.CustomerId);

    //        // Use session details in the view or return them as a model
    //        ViewBag.CustomerName = customer.Name;
    //        ViewBag.AmountPaid = session.AmountTotal / 100.0;  // Stripe amounts are in cents
    //        ViewBag.PaymentStatus = session.PaymentStatus;

    //        // Render the success page
    //        return Redirect("PaymentSuccess");
    //    }
    //}


    //public class SuccessController : Controller
    //{
    //    public SuccessController()
    //    {
    //        // Set your secret key. Remember to switch to your live secret key in production!
    //        // See your keys here: https://dashboard.stripe.com/apikeys
    //        StripeConfiguration.ApiKey = "sk_test_51Q2OkdLffhLXA6EXPrfKh0VcjFq9OydxjlpNAE9tAFVygmAWLSrmKYjjwo2HliwAIicTtxat7XS5OLqAEMw1Ooce00JpCHPLkh";
    //    }

    //    [HttpGet("/PaymentSuccess")]
    //    public ActionResult OrderSuccess([FromQuery] string session_id)
    //    {
    //        var sessionService = new SessionService();
    //        Session session = sessionService.Get(session_id);

    //        var customerService = new CustomerService();
    //        Customer customer = customerService.Get(session.CustomerId);

    //        return Content($"<html><body><h1>Thanks for your order, {customer.Name}!</h1></body></html>");
    //    }
    //}



    //[Route("PaymentSuccess")]
    //public class CheckoutController : Controller
    //{
    //    [HttpGet]
    //    public async Task<IActionResult> PaymentSuccess()
    //    {
    //        // Retrieve session ID from TempData
    //        var sessionId = TempData["SessionId"]?.ToString();

    //        if (string.IsNullOrEmpty(sessionId))
    //        {
    //            // Handle case where session ID is missing (payment might have failed)
    //            return RedirectToAction("Error");  // Redirect to an error page if needed
    //        }

    //        // Use the session ID to retrieve the session details from Stripe
    //        var service = new SessionService();
    //        Session session = await service.GetAsync(sessionId);

    //        if (session.PaymentStatus == "paid")
    //        {
    //            // Handle successful payment, show confirmation message, etc.
    //            Console.WriteLine("Payment went through!!");
    //            return Redirect("PaymentSuccess");  // Make sure you have a PaymentSuccess.cshtml view in the Views folder
    //        }

    //        // If the payment was not successful
    //        return RedirectToAction("Error");  // Handle errors or payment failure
    //    }
    //}




    //[Route("PaymentSuccess")]
    //public class PaymentSuccessController : Controller
    //{
    //    [HttpGet]
    //    public async Task<IActionResult> OnPaymentSuccess(int userId, decimal amountPaid)
    //    {
    //        // Render the PaymentSuccess view or return some kind of success message
    //        return RedirectToPage("/Account"); // Assuming you have a PaymentSuccess.cshtml page
    //    }
    //}




    //API controller to handle successful payments and update the payment amount info in the db:
    [Route("PaymentSuccess")]
    public class PaymentSuccessController : Controller
    {
        private readonly AsyncAcademyContext _context;
        public PaymentSuccessController(AsyncAcademyContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async void OnPaymentSuccess(int userId, decimal amountPaid)
        {
            var studentPaymentRecord = await _context.Payments.FirstOrDefaultAsync(s => s.UserId == userId);

            if (studentPaymentRecord != null)
            {
                //We update the outstanding amount here:
                studentPaymentRecord.AmountPaid -= amountPaid;
                //Console.WriteLine("PaymentSuccess router was used");

                //Time stamp when this payments happened:
                studentPaymentRecord.Timestamp = DateTime.Now;

                //Save all the changes to the db:
                _context.Payments.Update(studentPaymentRecord);
                await _context.SaveChangesAsync();
            }

            //return RedirectToAction("Account", "YourAccountPageController");

            //return Redirect("PaymentSuccess");


        }
    }


    //[Route("PaymentSuccess")]
    //public class PaymentSuccessController : Controller
    //{
    //    private readonly AsyncAcademyContext _context;

    //    public PaymentSuccessController(AsyncAcademyContext context)
    //    {
    //        _context = context;
    //    }

    //    [HttpGet]
    //    public async Task<IActionResult> OnPaymentSuccess(string sessionId)
    //    {
    //        // Fetch the Stripe session using the session ID
    //        var service = new SessionService();
    //        var session = await service.GetAsync(sessionId);

    //        // Check that the session was completed successfully
    //        if (session.PaymentStatus == "paid")
    //        {
    //            // Get the client reference ID (which is the userId)
    //            var userId = int.Parse(session.ClientReferenceId);

    //            // Get the total amount paid (Stripe stores amounts in cents)
    //            decimal amountPaid = (decimal)session.AmountTotal / 100m;

    //            // Find the user's payment record
    //            var paymentRecord = await _context.Payments.FirstOrDefaultAsync(s => s.UserId == userId);

    //            if (paymentRecord != null)
    //            {
    //                // Update existing payment record
    //                paymentRecord.AmountPaid += amountPaid;
    //                paymentRecord.Timestamp = DateTime.Now;

    //                _context.Payments.Update(paymentRecord);
    //                await _context.SaveChangesAsync();
    //            }
    //            else
    //            {
    //                // Create a new payment record if none exists
    //                var newPayment = new Payment
    //                {
    //                    UserId = userId,
    //                    AmountPaid = amountPaid,
    //                    Timestamp = DateTime.Now
    //                };
    //                _context.Payments.Add(newPayment);
    //                await _context.SaveChangesAsync();
    //            }
    //        }

    //        return RedirectToPage("/Account");
    //    }
    //}




    [Route("api/webhooks/stripe")]
    [ApiController]
    public class WebHookController : Controller
    {
        private readonly AsyncAcademyContext _context;

        public WebHookController(AsyncAcademyContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> HandleStripeEvent()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], "whsec_6fdMyVUL25owPksvtY6vBNnaKxW9PXrd");//UPDATE THIS
                Console.WriteLine("PaymentSuccess router was used");

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;

                    //We retrieve payment details:
                    var userId = session.ClientReferenceId;
                    var amountPaid = session.AmountTotal / 100m; //Stripe stores amounts in cents, this converts it to dollars


                    //Find the user in your database and update the Payments table
                    var paymentRecord = await _context.Payments.FirstOrDefaultAsync(p => p.UserId == int.Parse(userId));

                    if (paymentRecord != null)
                    {
                        // If amountPaid is null, set it to 0 as a default value
                        paymentRecord.AmountPaid += amountPaid ?? 0m;
                        paymentRecord.Timestamp = DateTime.Now;

                        //We save the changes to the db:
                        _context.Payments.Update(paymentRecord);
                        await _context.SaveChangesAsync();
                    }
                }

                return Ok();
            }

            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }

}