using AsyncAcademy.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Diagnostics;
using System.Security.Principal;

namespace AsyncAcademy.Pages
{
    public class ProfileModel(AsyncAcademy.Data.AsyncAcademyContext context) : PageModel
    {
        private AsyncAcademy.Data.AsyncAcademyContext _context = context;
        public string accountType = "Student";
        public DateTime birthday;
        public bool isEditable = false;


        [BindProperty]
        public User? Account { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //// Sample data - replace with actual user data retrieval logic
            //UserName = "John Doe";
            //Email = "john.doe@example.com";
            //JoinDate = new DateTime(2023, 1, 15);
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return NotFound();
            }

            Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == currentUserID);

            if (Account == null)
            {
                return NotFound();
            }

            if (Account.IsProfessor == true)
            {
                accountType = "Professor";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");



            if (currentUserID == null)
            {
                return NotFound();
            }

            Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == currentUserID);

            if (_context.Entry(Account).State == EntityState.Detached)
            {
                _context.Users.Attach(Account);
            }

            Debug.WriteLine(_context.Entry(Account).State); // Check the entity state before saving
            Debug.WriteLine("TEST!!");


            if (Account == null)
            {
                return NotFound();
            }

            if(action == "Edit")
            {
                isEditable = true;
                Debug.WriteLine("EDIT BUTTON WAS PRESSED");
                return Page();
            }

            else if(action == "Save")
            {
                Debug.WriteLine("SAVE BUTTON WAS PRESSED");

                if (await TryUpdateModelAsync<User>(Account, "Account", a => a.FirstName, a => a.LastName, a => a.Birthday))
                {
                    Debug.WriteLine("CHANGES BEING SAVED???????????");

                    await _context.SaveChangesAsync();

                    // Reload the entity from the database to ensure data is up-to-date
                    await _context.Entry(Account).ReloadAsync();
                    isEditable = false;
                    return RedirectToPage();
                }
                else
                {
                    // Check if there are any validation errors and log them
                    if (!ModelState.IsValid)
                    {
                        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                        {
                            Debug.WriteLine("Validation Error: " + error.ErrorMessage);
                        }
                    }
                }
            }

            return Page();
        }
    }

    
}
