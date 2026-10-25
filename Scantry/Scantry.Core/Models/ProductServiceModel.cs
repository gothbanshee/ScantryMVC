using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Scantry.GCommon.Constants.EntityValidationConstants;

namespace Scantry.Core.Models
{
    public class ProductServiceModel
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(BarcodeMaxLength, MinimumLength = BarcodeMinLength)]
        [Unicode(false)]
        public required string Barcode { get; set; }

        [Required]
        [StringLength(ProductTypeMaxLength, MinimumLength = ProductTypeMinLength,
                ErrorMessage = "The Product type name must be between {2} and {1} characters long.")]
        [Unicode(true)]
        public required string ProductType { get; set; }    // e.g., "Toothpaste"

        [Required]
        [StringLength(BrandMaxLength, MinimumLength = BrandMinLength,
                ErrorMessage = "The Brand name must be between {2} and {1} characters long.")]
        [Unicode(true)]
        public required string Brand { get; set; }          // e.g., "Colgate"

        [Required]
        [StringLength(ModelMaxLength, MinimumLength = ModelMinLength,
                ErrorMessage = "The Model name must be between {2} and {1} characters long.")]
        [Unicode(true)]
        public required string Model { get; set; }          // e.g., "Total Advanced"

        [StringLength(VariantMaxLength,
                ErrorMessage = "The Variant name cannot be more than {1} characters long.")]
        [Unicode(true)]
        public string? Variant { get; set; }                // e.g., "Whitening / Charcoal"

        [Required]
        [StringLength(SizeUnitMaxLength, MinimumLength = SizeUnitMinLength,
                ErrorMessage = "The size unit must be between {2} and {1} characters long.")]
        [Unicode(false)]
        public required string SizeUnit { get; set; }       // e.g., "ml" -- see Class SizeUnits for options

        public decimal SizeValue { get; set; }
    }
}