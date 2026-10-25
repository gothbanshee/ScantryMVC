using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Scantry.Data.Models
{
    [PrimaryKey(nameof(ProductId), nameof(HouseholdId))]
    public class HouseholdProduct
    {
        [Comment("Id of the Product.")]
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [Comment("Id of the Household.")]
        public int HouseholdId { get; set; }

        [ForeignKey(nameof(HouseholdId))]
        public required Household Household { get; set; } = null!;
    }
}