using System.Text.Json.Serialization;

namespace Scantry.Core.Models.External
{
    public class OFFResponse
    {
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("product")]
        public OFFProductDetails? Product { get; set; }
    }

    public class OFFProductDetails
    {
        [JsonPropertyName("compared_to_category")]
        public string? Category { get; set; }

        [JsonPropertyName("brands")]
        public string? Brands { get; set; }

        [JsonPropertyName("product_name")]
        public string? Model { get; set; }

        [JsonPropertyName("product_quantity")]
        public decimal? SizeValue { get; set; }

        [JsonPropertyName("product_quantity_unit")]
        public string? SizeUnit { get; set; }

        [JsonPropertyName("product_type")]
        public string? ProductType { get; set; }
    }
}