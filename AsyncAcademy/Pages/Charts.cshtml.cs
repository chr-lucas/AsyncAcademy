using AsyncAcademy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AsyncAcademy.Pages
{
    public class ChartsModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public ChartsModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        [ViewData]
        public string NavBarLink { get; set;}

        [ViewData]
        public string NavBarText { get; set;}

        public User Account { get; set; }




        public void OnGet()
        {
            //assigns user - Assisted by Chris L.
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            Account = _context.Users.FirstOrDefault(a => a.Id == currentUserID);

            if (Account == null)
            {
                NotFound();
            }

            //set navbar
            if (Account.IsProfessor)
            {
                NavBarLink = "Course Pages/InstructorIndex";
                NavBarText = "Classes";
            }
            else
            {
                NavBarLink = "Course Pages/StudentIndex";
                NavBarText = "Register";
            }


            //chart functionality will be added in later sprints
        }
    }
}
