using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Scantry.Data.Models;

namespace Scantry.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Household> Households { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<HouseholdUsers> HouseholdUsers { get; set; }

        public DbSet<HouseholdProduct> HouseholdProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<HouseholdUsers>()
                .HasKey(hu => new { hu.HouseholdId, hu.ApplicationUserId });

            builder.Entity<HouseholdProduct>()
                .HasOne(hp => hp.Household)
                .WithMany(h => h.HouseholdProducts)
                .HasForeignKey(hp => hp.HouseholdId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<HouseholdUsers>()
                .HasOne(hu => hu.Household)
                .WithMany(h => h.HouseholdUsers)
                .HasForeignKey(hu => hu.HouseholdId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<HouseholdUsers>()
                .HasOne(hu => hu.ApplicationUser)
                .WithMany(u => u.UserHouseholds)
                .HasForeignKey(hu => hu.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}