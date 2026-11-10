using Scantry.Core.Models;
using Scantry.Data.Models;

namespace Scantry.Core.Extensions
{
    public static class MappingExtensions
    {
        /* To service model*/
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

        public static HouseholdUserServiceModel ToServiceModel(this HouseholdUser householdUser)
        {
            return new HouseholdUserServiceModel
            {
                HouseholdId = householdUser.HouseholdId,
                ApplicationUserId = householdUser.ApplicationUserId,
                UserName = householdUser.ApplicationUser.UserName ?? "Unknown"
            };
        }

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
                Product = householdProduct.Product.ToServiceModel()
            };
        }

        public static HouseholdProductEditModel ToEditServiceModel(this HouseholdProduct householdProduct)
        {
            return new HouseholdProductEditModel
            {
                HouseholdProductId = householdProduct.HouseholdProductId,
                HouseholdId = householdProduct.HouseholdId,
                HouseholdName = householdProduct.Household.HouseholdName,
                Category = householdProduct.Category,
                CurrentQuantity = householdProduct.CurrentQuantity,
                MinimumThreshold = householdProduct.MinimumThreshold,
                ExpirationDate = householdProduct.ExpirationDate,
                ProductName = $"{householdProduct.Product.Brand} {householdProduct.Product.Model}",
                ProductTypeAndVariant = $"{householdProduct.Product.ProductType} ({householdProduct.Product.Variant})"
            };
        }

        /* To entity */
        public static Household ToEntity(this HouseholdServiceModel householdModel)
        {
            return new Household
            {
                HouseholdName = householdModel.HouseholdName,
                AccessToken = Guid.NewGuid(),
                HouseholdUsers = [],
                HouseholdProducts = []
            };
        }

        public static HouseholdUser ToEntity(this HouseholdUserServiceModel householdUserModel)
        {
            return new HouseholdUser
            {
                HouseholdId = householdUserModel.HouseholdId,
                ApplicationUserId = householdUserModel.ApplicationUserId
            };
        }

        public static Product ToEntity(this ProductServiceModel productModel)
        {
            return new Product
            {
                Barcode = productModel.Barcode,
                ProductType = productModel.ProductType,
                Brand = productModel.Brand,
                Model = productModel.Model,
                Variant = productModel.Variant,
                SizeUnit = productModel.SizeUnit,
                SizeValue = productModel.SizeValue
            };
        }

        public static HouseholdProduct ToEntity(this HouseholdProductServiceModel householdProductModel)
        {
            return new HouseholdProduct
            {
                Category = householdProductModel.Category,
                CurrentQuantity = householdProductModel.CurrentQuantity,
                MinimumThreshold = householdProductModel.MinimumThreshold,
                ExpirationDate = householdProductModel.ExpirationDate,
                IsFavourite = householdProductModel.IsFavourite,
                ProductId = householdProductModel.Product.ProductId,
                HouseholdId = householdProductModel.HouseholdId
            };
        }

        public static HouseholdProduct ToEditEntity(this HouseholdProductEditModel editModel, HouseholdProduct householdProduct)
        {
            return new HouseholdProduct
            {
                Category = editModel.Category,
                CurrentQuantity = editModel.CurrentQuantity,
                MinimumThreshold = editModel.MinimumThreshold,
                ExpirationDate = editModel.ExpirationDate,
                IsFavourite = householdProduct.IsFavourite,
                ProductId = householdProduct.Product.ProductId,
                HouseholdId = householdProduct.HouseholdId
            };
        }
    }
}