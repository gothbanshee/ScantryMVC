using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scantry.Data.Models
{
    public class HouseholdUser
    {
        [Comment("Id of the Household.")]
        public int HouseholdId { get; set; }

        [ForeignKey(nameof(HouseholdId))]
        public Household Household { get; set; } = null!;

        [Comment("Id of the User.")]
        public string ApplicationUserId { get; set; } = null!;

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}