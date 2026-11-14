using Scantry.GCommon.Enums;
using System.ComponentModel.DataAnnotations;
using static Scantry.GCommon.Constants.EntityValidationConstants;
using static Scantry.GCommon.Messages.HouseholdProductMessages;


namespace Scantry.Core.Models
{
    public class HouseholdProductServiceModel
    {
        public int HouseholdProductId { get; set; }

        [Required]
        public HouseholdProductCategory Category { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ProductQuantityNegative)]
        [Display(Name = "Current quantity")]
        public int CurrentQuantity { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = ProductThresholdNegative)]
        [Display(Name = "Minimum threshold")]
        public int MinimumThreshold { get; set; }

        public bool IsLowStock => CurrentQuantity <= MinimumThreshold;

        [Display(Name = "Expiration date")]
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
                    return ExpirationStatus.Expiring;
                }
                else
                {
                    return ExpirationStatus.Fresh;
                }
            }
        }

        public bool IsFavourite { get; set; } = false;

        public ProductServiceModel Product { get; set; } = null!;

        public required int HouseholdId { get; set; }
    }
}