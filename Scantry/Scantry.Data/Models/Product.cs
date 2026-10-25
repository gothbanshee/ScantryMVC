using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static Scantry.GCommon.Constants.EntityValidationConstants;

namespace Scantry.Data.Models
{
    [Index(nameof(Barcode), IsUnique = true)]
    public class Product
    {
        [Key]
        [Comment("Primary key for the Product entity.")]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(BarcodeMaxLength)]
        [Unicode(false)]
        [Comment("Product barcode.")]
        public required string Barcode { get; set; }

        [Required]
        [MaxLength(ProductTypeMaxLength)]
        [Unicode(true)]
        [Comment("Product type.")]
        public required string ProductType { get; set; }    // e.g., "Toothpaste"

        [Required]
        [MaxLength(BrandMaxLength)]
        [Unicode(true)]
        [Comment("Product brand.")]
        public required string Brand { get; set; }          // e.g., "Colgate"

        [Required]
        [MaxLength(ModelMaxLength)]
        [Unicode(true)]
        [Comment("Product model.")]
        public required string Model { get; set; }          // e.g., "Total Advanced"

        [MaxLength(VariantMaxLength)]
        [Unicode(true)]
        [Comment("Product variation (if applicable).")]
        public string? Variant { get; set; }                // e.g., "Whitening / Charcoal"

        [Required]
        [MaxLength(SizeUnitMaxLength)]
        [Unicode(false)]
        [Comment("Product measuring unit.")]
        public required string SizeUnit { get; set; }       // e.g., "ml" -- see Class SizeUnits for options

        [Comment("Product size value.")]
        public decimal SizeValue { get; set; }               // e.g. 500
    }
}