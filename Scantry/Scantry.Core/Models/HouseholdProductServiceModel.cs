using Microsoft.EntityFrameworkCore;
using Scantry.GCommon.Enums;
using System.ComponentModel.DataAnnotations;
using static Scantry.GCommon.Constants.EntityValidationConstants;
using static Scantry.GCommon.Constants.ValidationConstants;

namespace Scantry.Core.Models
{
    public class HouseholdProductServiceModel
    {
        public int HouseholdProductId { get; set; }

        [Required]
        [StringLength(CategoryMaxLength, MinimumLength = CategoryMinLength,
                ErrorMessage = "The Category name must be between {2} and {1} characters long.")]
        [Unicode(false)]
        public string Category { get; set; } = null!;         // see Class HouseholdProductCategories for options

        public int CurrentQuantity { get; set; }

        public int MinimumThreshold { get; set; }

        public bool IsLowStock => CurrentQuantity <= MinimumThreshold;

        //

        public DateOnly ScannedOn { get; set; }

        public DateOnly? ExpirationDate { get; set; }

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

        public bool IsFavourite { get; set; } = false;

        [StringLength(450)]
        [Unicode(true)]
        public string? ScannedByUserId { get; set; }

        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength)]
        [Unicode(true)]
        public string ScannedByUserName { get; set; } = null!;

        public ProductServiceModel Product { get; set; } = null!;

        public required int HouseholdId { get; set; }
    }
}