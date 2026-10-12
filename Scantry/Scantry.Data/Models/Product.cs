using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Scantry.Data.Models
{
    public class Product
    {
        [Key]
        [Comment("Primary key for the Product entity.")]
        public int ProductId { get; set; }

        public List<HouseholdProduct> HouseholdProducts { get; set; } = [];
    }
}