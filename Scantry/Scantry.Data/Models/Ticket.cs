using Microsoft.EntityFrameworkCore;
using Scantry.GCommon.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Scantry.GCommon.Constants.EntityValidationConstants;
using static Scantry.GCommon.Constants.UserValidationConstants;

namespace Scantry.Data.Models
{
    public class Ticket
    {
        [Key]
        [Comment("Primary key for the Ticket entity.")]
        public int TicketId {  get; set; }

        [Required]
        [Comment("Current status of the ticket; changed by admin.")]
        public TicketStatus Status { get; set; }

        [Required]
        [Comment("Ticket type; initially set by user.")]
        public TicketTypes TicketType { get; set; }

        [Unicode(true)]
        [MaxLength(TicketTextMaxLength)]
        [Comment("Problem description; provided by user.")]
        public string? Description { get; set; }

        [Required]
        [Unicode(true)]
        [MaxLength(TicketTextMaxLength)]
        [Comment("Admin response to ticket.")]
        public required string AdminResponse { get; set; }

        [Required]
        [Comment("Ticket create date.")]
        public DateTime CreatedOn { get; set; }

        [Comment("Ticket resolve date; added on status Rejected or Completed.")]
        public DateTime? CompletedOn { get; set; }

        /* Relations */

        [Unicode(true)]
        [MaxLength(UserIdMaxLength)]
        [Comment("Id of the User that created the ticket.")]
        public string? CreatedByUserId { get; set; }

        [ForeignKey(nameof(CreatedByUserId))]
        public ApplicationUser? ApplicationUser { get; set; }

        [Unicode(true)]
        [MaxLength(UserIdMaxLength)]
        [Comment("Id of the Admin working on the ticket.")]
        public string? AssignedToAdminId { get; set; }

        [ForeignKey(nameof(AssignedToAdminId))]
        public ApplicationUser? Admin { get; set; }

        [Comment("Id of the reported Product, if applicable.")]
        public int? ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        [Comment("Id of the reported Household, if applicable.")]
        public int? HouseholdId { get; set; }

        [ForeignKey(nameof(HouseholdId))]
        public Household? Household { get; set; }
    }
}