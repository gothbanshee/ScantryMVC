using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scantry.Data.Models
{
    [PrimaryKey(nameof(HouseholdId), nameof(UserId))]
    public class HouseholdUsers
    {
        [Comment("Id of the Household.")]
        public int HouseholdId { get; set; }

        [ForeignKey(nameof(HouseholdId))]
        public Household Household { get; set; } = null!;

        [Comment("Id of the User.")]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; } = null!;

    }
}