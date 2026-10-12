using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Scantry.Data.Models;

namespace Scantry.Data
{
    // Inherit from IdentityDbContext with your custom ApplicationUser
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Household> Households { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<HouseholdUsers> HouseholdUsers { get; set; }

        public DbSet<HouseholdProduct> HouseholdProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}