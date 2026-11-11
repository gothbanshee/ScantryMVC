using Microsoft.EntityFrameworkCore;
using Scantry.Core.Contracts;
using Scantry.Core.Extensions;
using Scantry.Core.Models;
using Scantry.Data.Models;
using Scantry.Data.Repositories;
using static Scantry.Core.Extensions.MappingExtensions;
using static Scantry.GCommon.Constants.EntityValidationConstants;

namespace Scantry.Core.Services
{
    public class HouseholdProductService(IRepository repository) : BaseService(repository), IHouseholdProductService
    {
        public async Task<List<HouseholdProductServiceModel>> GetAllProductsForHouseholdAsync(int householdId, string userId)
        {
            await IsUserAMember(householdId, userId);

            List<HouseholdProduct> productsForHousehold = await repository.GetAllWithPredicate<HouseholdProduct>(hp => hp.HouseholdId == householdId)
                                                                          .Include(hp => hp.Product)
                                                                          .OrderBy(hp => hp.Product.ProductType)
                                                                          .ToListAsync();

            return productsForHousehold.Count != 0 ? [.. productsForHousehold.Select(hp => hp.ToServiceModel())]
                                                   : [];
        }

        public async Task<List<HouseholdProductServiceModel>> GetLowStockAsync(int householdId, string userId)
        {
            await IsUserAMember(householdId, userId);

            List<HouseholdProduct> lowStockProducts = await repository.All<HouseholdProduct>()
                                                                      .Where(hp => hp.HouseholdId == householdId && hp.CurrentQuantity <= hp.MinimumThreshold)
                                                                      .Include(hp => hp.Product)
                                                                      .OrderBy(hp => hp.Product.ProductType)
                                                                      .ToListAsync();

            return lowStockProducts.Count != 0 ? [.. lowStockProducts.Select(hp => hp.ToServiceModel())]
                                               : [];
        }

        public async Task<List<HouseholdProductServiceModel>> GetFavouritesAsync(int householdId, string userId)
        {
            await IsUserAMember(householdId, userId);

            List<HouseholdProduct> favouriteProducts = await repository.All<HouseholdProduct>()
                                                                       .Where(hp => hp.HouseholdId == householdId && hp.IsFavourite)
                                                                       .Include(hp => hp.Product)
                                                                       .OrderBy(hp => hp.Product.ProductType)
                                                                       .ToListAsync();

            return favouriteProducts.Count != 0 ? [.. favouriteProducts.Select(hp => hp.ToServiceModel())]
                                                : [];
        }

        public async Task<List<HouseholdProductServiceModel>> GetExpiringAsync(int householdId, string userId)
        {
            await IsUserAMember(householdId, userId);

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            List<HouseholdProduct> expiringProducts = await repository.All<HouseholdProduct>()
                    .Where(hp => hp.HouseholdId == householdId
                      && hp.ExpirationDate.HasValue
                      && hp.ExpirationDate.Value < today.AddDays(ExpirationDateCheck))
                    .Include(hp => hp.Product)
                    .OrderBy(hp => hp.Product.ProductType)
                    .ToListAsync();

            return expiringProducts.Count != 0 ? [.. expiringProducts.Select(hp => hp.ToServiceModel())]
                                               : [];
        }

        public async Task<HouseholdProductEditModel?> GetProductForEditAsync(int householdProductId, string userId)
        {
            HouseholdProduct? householdProduct = await repository.AllTracked<HouseholdProduct>()
                .Include(p => p.Product)
                .Include(p => p.Household)
                .FirstOrDefaultAsync(p => p.HouseholdProductId == householdProductId);

            if (householdProduct == null)
                return null;

            await IsUserAMember(householdProduct.HouseholdId, userId);

            return householdProduct?.ToEditServiceModel();
        }

        public async Task<bool> UpdateHouseholdProductAsync(HouseholdProductEditModel model, string userId)
        {
            HouseholdProduct? householdProduct = await repository
                .FirstOrDefaultTrackedAsync<HouseholdProduct>(hp => hp.HouseholdProductId == model.HouseholdProductId);

            if (householdProduct == null)
                return false;

            await IsUserAMember(householdProduct.HouseholdId, userId);

            householdProduct = model.ToEditEntity(householdProduct);

            return await repository.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteHouseholdProductAsync(int householdProductId, string userId)
        {
            var householdProduct = await repository
                .All<HouseholdProduct>()
                .FirstOrDefaultAsync(hp => hp.HouseholdProductId == householdProductId);

            if (householdProduct == null)
                return false;

            await IsUserAMember(householdProduct.HouseholdId, userId);

            int productDeleted = await repository.DeleteAsync<HouseholdProduct>(hp => hp.HouseholdProductId == householdProductId);

            return productDeleted > 0;
        }

        public async Task ToggleFavouriteAsync(int householdProductId, string userId)
        {
            HouseholdProduct? householdProduct = await repository
                .FirstOrDefaultTrackedAsync<HouseholdProduct>(hp => hp.HouseholdProductId == householdProductId)
                ?? throw new InvalidOperationException("Household product not found.");

            await IsUserAMember(householdProduct.HouseholdId, userId);

            householdProduct.IsFavourite = !householdProduct.IsFavourite;

            await repository.SaveChangesAsync();
        }

        public async Task AddProductToHouseholdAsync(int householdId, int productId, string userId)
        {
            await IsUserAMember(householdId, userId);

            var existingHouseholdProduct = await repository
                .AllTracked<HouseholdProduct>()
                .FirstOrDefaultAsync(hp => hp.HouseholdId == householdId && hp.ProductId == productId);

            if (existingHouseholdProduct != null)
            {
                existingHouseholdProduct.CurrentQuantity += 1;
            }
            else
            {
                var newHouseholdProduct = new HouseholdProduct
                {
                    HouseholdId = householdId,
                    Household = null!,
                    ProductId = productId,
                    CurrentQuantity = 1,
                    MinimumThreshold = 1,
                    Category = "General",
                    IsFavourite = false
                };

                await repository.AddAsync(newHouseholdProduct);
            }

            await repository.SaveChangesAsync();
        }
    }
}