using Microsoft.EntityFrameworkCore;
using Scantry.GCommon.Enums;
using System.ComponentModel.DataAnnotations;
using static Scantry.GCommon.Constants.EntityValidationConstants;
using static Scantry.GCommon.Constants.UserValidationConstants;

namespace Scantry.Core.Models
{
    public class TicketServiceModel
    {
        public int TicketId { get; set; }

        [Required]
        public TicketStatus Status { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public TicketTypes TicketType { get; set; }

        [Unicode(true)]
        [MaxLength(TicketTextMaxLength)]
        public string? Description { get; set; }

        [Required]
        [Unicode(true)]
        [StringLength(TicketTextMaxLength, MinimumLength = TicketTextMinLength,
            ErrorMessage = "The Admin response must be between {2} and {1} characters long.")]
        [Display(Name = "Scantry Team Response")]
        public required string AdminResponse { get; set; }

        [Required]
        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Completed on")]
        public DateTime? CompletedOn { get; set; }

        /* Relations */

        [Unicode(true)]
        [MaxLength(UserIdMaxLength)]
        [Display(Name = "Ticket creator")]
        public string? CreatedByUserId { get; set; }

        [Unicode(true)]
        [MaxLength(UserIdMaxLength)]
        public string? AssignedToAdminId { get; set; }

        [MaxLength(UserNameMaxLength)]
        [Display(Name = "Responsible admin")]
        public string? AssignedToAdminName { get; set; }

        public int? ProductId { get; set; }

        [Display(Name = "Related product barcode")]
        public string? ProductBarcode { get; set; }

        public int? HouseholdId { get; set; }

        [Display(Name = "Related household")]
        public string? HouseholdName { get; set; }
    }
}