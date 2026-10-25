using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Scantry.GCommon.Constants.EntityValidationConstants;

namespace Scantry.Data.Models
{
    public class Household
    {
        [Key]
        [Comment("Primary key for the Household entity.")]
        public int HouseholdId { get; set; }

        [Required]
        [MaxLength(HouseholdNameMaxLength)]
        [Unicode(true)]
        [Comment("Name for the Household entity; supports Unicode.")]
        public required string HouseholdName { get; set; }

        [Comment("GUID Access Token for joining the Household.")]
        public Guid AccessToken { get; set; }

        public List<HouseholdUser> HouseholdUsers { get; set; } = null!;

        public List<HouseholdProduct> HouseholdProducts { get; set; } = null!;
    }
}