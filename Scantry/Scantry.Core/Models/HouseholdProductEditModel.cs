using System.ComponentModel.DataAnnotations;
using static Scantry.GCommon.Constants.EntityValidationConstants;
using static Scantry.GCommon.Messages.HouseholdProductMessages;

namespace Scantry.Core.Models
{
    public class HouseholdProductEditModel
    {
        public int HouseholdProductId { get; set; }
        
        public int HouseholdId { get; set; }

        public string? HouseholdName { get; set; }

        [Required]
        [MaxLength(CategoryMaxLength)]
        public string Category { get; set; } = null!;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = QuantityNegative)]
        public int CurrentQuantity { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = ThresholdNegative)]
        public int MinimumThreshold { get; set; }

        public DateOnly? ExpirationDate { get; set; }

        public string? ProductName { get; set; }
        public string? ProductTypeAndVariant { get; set; }
    }
}