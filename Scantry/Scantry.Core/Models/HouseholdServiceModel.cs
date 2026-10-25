using System.ComponentModel.DataAnnotations;
using static Scantry.GCommon.Constants.EntityValidationConstants;

namespace Scantry.Core.Models
{
    public class HouseholdServiceModel
    {
        public int HouseholdId { get; set; }

        [Required]
        [StringLength(HouseholdNameMaxLength, MinimumLength = HouseholdNameMinLength,
                ErrorMessage = "The Household name must be between {2} and {1} characters long.")]
        public string HouseholdName { get; set; } = null!;

        public string? AccessToken { get; set; }

        public List<HouseholdUserServiceModel> HouseholdUsers { get; set; } = [];

        public List<HouseholdProductServiceModel> HouseholdProducts { get; set; } = [];
    }
}