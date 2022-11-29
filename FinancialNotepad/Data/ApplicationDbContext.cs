using FinancialNotepad.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinancialNotepad.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Currency> Currencies { get; set; }
    }
}