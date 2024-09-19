using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AsyncAcademy.Models;

namespace AsyncAcademy.Data
{
    public class AsyncAcademyContext : DbContext
    {
        public AsyncAcademyContext (DbContextOptions<AsyncAcademyContext> options)
            : base(options)
        {
        }

        public DbSet<AsyncAcademy.Models.User> Users { get; set; } = default!;
        public DbSet<AsyncAcademy.Models.Enrollment> Enrollments { get; set; } = default!;
        public DbSet<AsyncAcademy.Models.Section> Sections { get; set; } = default!;
    }
}
