using System.ComponentModel.DataAnnotations;

namespace Scantry.GCommon.Enums
{
    public enum TicketTypes
    {
        [Display(Name = "Product not found by barcode")]
        MissingProduct,

        [Display(Name = "Product information false or incomplete")]
        FalseProductInfo,

        [Display(Name = "Issues with a household")]
        HouseholdIssue,

        [Display(Name = "General feedback for Scantry")]
        GeneralFeedback
    }
}