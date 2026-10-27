namespace Scantry.Core.Models
{
    public class ExternalProductModel
    {
        public string Barcode { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string ProductType { get; set; } = "General";
        public string Category { get; set; } = "Pantry";
        public decimal? SizeValue { get; set; }
        public string SizeUnit { get; set; } = "pcs";
    }
}