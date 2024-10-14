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

namespace AsyncAcademy.Pages
{
    public class PaymentSuccess2Model : PageModel
    {
        //private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        //public PaymentSuccessModel(AsyncAcademy.Data.AsyncAcademyContext context)
        //{
        //    _context = context;
        //}

        //public decimal AmountPaid { get; set; }
        //public int? UserId { get; set; }


        public void OnGet()
        {

        }

        //[BindProperty]
        //public Payment Payment { get; set; } = default!;

        //// For more information, see https://aka.ms/RazorPagesCRUD.
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    _context.Payments.Add(Payment);
        //    await _context.SaveChangesAsync();

        //    return RedirectToPage("/Account");
        //}
    }
}
