﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Data;
using AsyncAcademy.Models;

namespace AsyncAcademy.Pages.Course_Pages
{
    public class EditModel : PageModel
    {
        private readonly AsyncAcademy.Data.AsyncAcademyContext _context;

        public EditModel(AsyncAcademy.Data.AsyncAcademyContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Course Course { get; set; } = default!;
        public User Account { get; set; } = default!;

        [ViewData]
        public string NavBarLink { get; set; } // Navigation link
        [ViewData]
        public string NavBarText { get; set; } // Navigation text

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            Course = course;

            // Set navigation properties
            await SetNavBarPropertiesAsync();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(Course.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task SetNavBarPropertiesAsync()
        {
            int? currentUserID = HttpContext.Session.GetInt32("CurrentUserId");

            if (currentUserID == null)
            {
                return; // User is not logged in, do not set nav properties
            }

            Account = await _context.Users.FirstOrDefaultAsync(a => a.Id == currentUserID);

            if (Account == null)
            {
                return; // Account not found, do not set nav properties
            }

            if (Account.IsProfessor) 
            {
                NavBarLink = "InstructorIndex"; // Set NavBarLink directly
                NavBarText = "Classes"; // Set NavBarText directly
            }
            else
            {
                NavBarLink = "StudentIndex"; // Set NavBarLink for non-professors
                NavBarText = "Register"; // Set NavBarText for non-professors
            }
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}