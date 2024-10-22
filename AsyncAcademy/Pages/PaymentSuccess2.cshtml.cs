using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AsyncAcademy.Data;
using AsyncAcademy.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace AsyncAcademy.Pages
{
    public class PaymentSuccess2Model : PageModel
    {
        [ViewData]
        public string NavBarLink { get; set; } = "Course Pages/StudentIndex";

        [ViewData]
        public string NavBarText { get; set; } = "Register";

        [ViewData]
        public string NavBarAccountTabLink { get; set; } = "/Account";

        [ViewData]
        public string NavBarAccountText { get; set; } = "Account";

        [BindProperty]
        public Enrollment Enrollment { get; set; } = default!;

        public List<Course> EnrolledCourses { get; set; } = new List<Course>();

        public Decimal StudentPaymentBalance { get; set; } = 0;

        public Payment PaymentRecord { get; set; } = default!;


        public string receiptLink { get; set; } = default!;

        private readonly AsyncAcademyContext _context;

        public PaymentSuccess2Model(AsyncAcademyContext context)
        {
            _context = context;
        }

        public string ReceiptUrl { get; set; }

        public async Task<IActionResult> OnGetAsync([FromQuery] string session_id)
        {
            // Fetch the Stripe session using the session ID
            var service = new SessionService();
            var session = await service.GetAsync(session_id);


            // check if session exists and has a PaymentIntentId
            if (session.PaymentIntentId != null)
            {
                // Retrieve the PaymentIntent using PaymentIntentId
                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = await paymentIntentService.GetAsync(session.PaymentIntentId);

                var chargeId = paymentIntent.LatestChargeId;

                var chargeService = new ChargeService();
                var chargeObject = await chargeService.GetAsync(chargeId);
                receiptLink = chargeObject.ReceiptUrl;


            }

            return Page(); 
        }
    }
}
