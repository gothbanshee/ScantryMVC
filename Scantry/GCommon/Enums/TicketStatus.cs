using System.ComponentModel.DataAnnotations;

namespace Scantry.GCommon.Enums
{
    public enum TicketStatus
    {
        New,
        [Display(Name = "In progress")]
        InProgress,
        Completed,
        Rejected
    }
}