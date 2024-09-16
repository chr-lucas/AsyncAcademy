using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace AsyncAcademy.Pages
{
    public class ProfileModel : PageModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }

        public void OnGet()
        {
            // Sample data - replace with actual user data retrieval logic
            UserName = "John Doe";
            Email = "john.doe@example.com";
            JoinDate = new DateTime(2023, 1, 15);
        }
    }
}
