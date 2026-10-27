using Scantry.Core.Models;
using Scantry.Data.Models;

namespace Scantry.Core.Extensions
{
    public static class MappingExtensions
    {
        // 1. Product mapping
        public static ProductServiceModel ToServiceModel(this Product product)
        {
            return new ProductServiceModel
            {
                ProductId = product.ProductId,
                Barcode = product.Barcode,
                ProductType = product.ProductType,
                Brand = product.Brand,
                Model = product.Model,
                Variant = product.Variant,
                SizeUnit = product.SizeUnit,
                SizeValue = product.SizeValue
            };
        }

        // 2. HouseholdProduct mapping
        public static HouseholdProductServiceModel ToServiceModel(this HouseholdProduct householdProduct)
        {
            return new HouseholdProductServiceModel
            {
                HouseholdProductId = householdProduct.HouseholdProductId,
                Category = householdProduct.Category,
                CurrentQuantity = householdProduct.CurrentQuantity,
                MinimumThreshold = householdProduct.MinimumThreshold,
                ExpirationDate = householdProduct.ExpirationDate,
                IsFavourite = householdProduct.IsFavourite,
                HouseholdId = householdProduct.HouseholdId,
                Product = householdProduct.Product.ToServiceModel() // Calls the product extension above!
            };
        }

        // 3. Household User mapping
        public static HouseholdUserServiceModel ToServiceModel(this HouseholdUser householdUser)
        {
            return new HouseholdUserServiceModel
            {
                HouseholdId = householdUser.HouseholdId,
                ApplicationUserId = householdUser.ApplicationUserId,
                UserName = householdUser.ApplicationUser.UserName ?? "Unknown"
            };
        }

        // 4. Household mapping (combines them all)
        public static HouseholdServiceModel ToServiceModel(this Household household)
        {
            return new HouseholdServiceModel
            {
                HouseholdId = household.HouseholdId,
                AccessToken = household.AccessToken.ToString(),
                HouseholdName = household.HouseholdName,
                HouseholdUsers = household.HouseholdUsers?
                    .Select(hu => hu.ToServiceModel())
                    .ToList() ?? [],
                HouseholdProducts = household.HouseholdProducts?
                    .Select(hp => hp.ToServiceModel())
                    .ToList() ?? []
            };
        }
    }
}