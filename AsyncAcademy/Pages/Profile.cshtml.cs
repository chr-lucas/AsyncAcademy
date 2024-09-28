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

        [ViewData]
        public string NavBarLink { get; set; } = "Course Pages/StudentIndex";

        [ViewData]
        public string NavBarText { get; set; } = "Register";

        [ViewData]
        public string NavBarAccountTabLink { get; set; } = "/Account";

        [ViewData]
        public string NavBarAccountText { get; set; } = "Account";

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
                NavBarLink = "Course Pages/InstructorIndex";
                NavBarText = "Classes";
                NavBarAccountTabLink = "";
                NavBarAccountText = "";
            }
            else
            {
                NavBarAccountTabLink = "/Account";
                NavBarAccountText = "Account";
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
            Debug.WriteLine("TEST!!");


            if (Account == null)
            {
                return NotFound();
            }

            if(action == "Edit")
            {
                isEditable = true;
                Debug.WriteLine("EDIT BUTTON WAS PRESSED");
                Debug.WriteLine(ViewData["NavBarAccountText"]);
                return Page();
            }

            else if(action == "Save")
            {

                // Check if the model state is valid
                if (!ModelState.IsValid)
                {
                    // Redisplay the form with validation errors
                    isEditable = true; // Keep the form in edit mode
                    return Page();
                }

                if (await TryUpdateModelAsync<User>(Account, "Account", a => a.FirstName, a => a.LastName, a => a.Birthday, a => a.Addr_Street, a => a.Addr_City, a => a.Addr_State, a => a.Addr_Zip, a => a.Phone))
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
