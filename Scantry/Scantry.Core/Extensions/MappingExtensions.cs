using Scantry.Core.Models;
using Scantry.Data.Models;

namespace Scantry.Core.Extensions
{
    public static class MappingExtensions
    {
        /* To service model*/

        extension(Household household)
        {
            public HouseholdServiceModel ToServiceModel()
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

        extension(HouseholdUser householdUser)
        {
            public HouseholdUserServiceModel ToServiceModel()
            {
                return new HouseholdUserServiceModel
                {
                    HouseholdId = householdUser.HouseholdId,
                    ApplicationUserId = householdUser.ApplicationUserId,
                    UserName = householdUser.ApplicationUser.UserName ?? "Unknown"
                };
            }
        }

        extension(Product product)
        {
            public ProductServiceModel ToServiceModel()
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
        }

        extension(HouseholdProduct householdProduct)
        {
            public HouseholdProductServiceModel ToServiceModel()
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

            public HouseholdProductEditModel ToEditServiceModel()
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
        }

        /* To entity */

        extension(HouseholdServiceModel householdModel)
        {
            public Household ToEntity()
            {
                return new Household
                {
                    HouseholdName = householdModel.HouseholdName,
                    AccessToken = Guid.NewGuid(),
                    HouseholdUsers = [],
                    HouseholdProducts = []
                };
            }
        }

        extension(HouseholdUserServiceModel householdUserModel)
        {
            public HouseholdUser ToEntity()
            {
                return new HouseholdUser
                {
                    HouseholdId = householdUserModel.HouseholdId,
                    ApplicationUserId = householdUserModel.ApplicationUserId
                };
            }
        }

        extension(ProductServiceModel productModel)
        {
            public Product ToEntity()
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
        }

        extension(HouseholdProductServiceModel householdProductModel)
        {
            public HouseholdProduct ToEntity()
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
        }

        extension(HouseholdProductEditModel editModel)
        {
            public HouseholdProduct ToEditEntity(HouseholdProduct householdProduct)
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
}