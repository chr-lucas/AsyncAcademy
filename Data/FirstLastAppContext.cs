using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FirstLastApp.Models;

namespace FirstLastApp.Data
{
    public class FirstLastAppContext : DbContext
    {
        public FirstLastAppContext (DbContextOptions<FirstLastAppContext> options)
            : base(options)
        {
        }

        public DbSet<FirstLastApp.Models.User> Account { get; set; } = default!;
    }
}
