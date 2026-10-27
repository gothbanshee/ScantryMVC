using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Scantry.Data.Models;

namespace Scantry.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Household> Households { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<HouseholdUser> HouseholdUsers { get; set; }

        public DbSet<HouseholdProduct> HouseholdProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<HouseholdProduct>()
                .HasOne(hp => hp.Household)
                .WithMany(h => h.HouseholdProducts)
                .HasForeignKey(hp => hp.HouseholdId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<HouseholdProduct>()
                .HasOne(hp => hp.Product)
                .WithMany()
                .HasForeignKey(hp => hp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<HouseholdUser>()
                .HasOne(hu => hu.Household)
                .WithMany(h => h.HouseholdUsers)
                .HasForeignKey(hu => hu.HouseholdId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<HouseholdUser>(entity =>
            {
                entity.HasKey(hu => new { hu.HouseholdId, hu.ApplicationUserId });

                entity.HasOne(hu => hu.ApplicationUser)
                .WithMany(u => u.UserHouseholds)
                .HasForeignKey(hu => hu.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Product>()
                .Property(p => p.SizeValue)
                .HasColumnType("decimal(18,2)");
        }
    }
}