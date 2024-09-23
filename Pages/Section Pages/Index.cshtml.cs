using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

namespace AsyncAcademy.Pages.Section_Page
{
    public class IndexModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public IndexModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        public IList<Section> Section { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Section = await _context.Sections.ToListAsync();
        }
    }
}
