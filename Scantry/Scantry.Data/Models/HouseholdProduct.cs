using Microsoft.EntityFrameworkCore;
using Scantry.GCommon.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Scantry.GCommon.Constants.EntityValidationConstants;

namespace Scantry.Data.Models
{
    [Index(nameof(Category))]
    public class HouseholdProduct
    {
        [Key]
        [Comment("Primary key for the HouseholdProduct entity.")]
        public int HouseholdProductId { get; set; }

        [Required]
        [MaxLength(CategoryMaxLength)]
        [Unicode(false)]
        [Comment("Category of the product - user-defined.")]
        public string Category { get; set; } = null!;         // see Class HouseholdProductCategories for options

        [Comment("Current quantity of the product in the household.")]
        public int CurrentQuantity { get; set; }

        [Comment("Threshold for buying reminder - user-defined.")]
        public int MinimumThreshold { get; set; }

        [Comment("Flag for buying reminder.")]
        public bool IsLowStock => CurrentQuantity <= MinimumThreshold;

        //

        [Comment("Date of scanning the product.")]
        public DateOnly ScannedOn { get; set; }

        [Comment("Expiration date of the product in the household - if applicable.")]
        public DateOnly? ExpirationDate { get; set; }

        [Comment("Expiration status of the product batch.")]
        public ExpirationStatus Status
        {
            get
            {
                if (!ExpirationDate.HasValue)
                {
                    return ExpirationStatus.Fresh;
                }

                int daysUntilExpiry = ExpirationDate.Value.DayNumber - DateOnly.FromDateTime(DateTime.Today).DayNumber;

                if (daysUntilExpiry < 0)
                {
                    return ExpirationStatus.Expired;
                }
                else if (daysUntilExpiry < ExpirationDateCheck)
                {
                    return ExpirationStatus.ExpiringSoon;
                }
                else
                {
                    return ExpirationStatus.Fresh;
                }
            }
        }

        [Comment("Flag for favourite products.")]
        public bool IsFavourite { get; set; } = false;

        //

        [MaxLength(450)]
        [Unicode(true)]
        public string? ScannedByUserId { get; set; }

        [ForeignKey(nameof(ScannedByUserId))]
        public ApplicationUser? ScannedByUser { get; set; }

        [Comment("Id of the Product.")]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [Comment("Id of the Household.")]
        public int HouseholdId { get; set; }

        [ForeignKey(nameof(HouseholdId))]
        public required Household Household { get; set; } = null!;
    }
}