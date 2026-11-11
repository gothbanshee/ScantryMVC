using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Scantry.GCommon.Constants.UserValidationConstants;

namespace Scantry.Core.Models
{
    public class HouseholdUserServiceModel
    {
        public int HouseholdId { get; set; }

        [Required]
        [MaxLength(UserIdMaxLength)]
        public string ApplicationUserId { get; set; } = null!;

        [Required]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength)]
        [Unicode(true)]
        public string UserName { get; set; } = null!;
    }
}