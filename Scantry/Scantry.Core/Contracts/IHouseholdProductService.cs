using Scantry.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scantry.Core.Contracts
{
    public interface IHouseholdProductService
    {
        Task<List<HouseholdProductServiceModel>> GetAllProductsForHouseholdAsync(int householdId, string userId);
        Task<List<HouseholdProductServiceModel>> GetLowStockAsync(int householdId, string userId);
        Task<List<HouseholdProductServiceModel>> GetFavouritesAsync(int householdId, string userId);
        Task<List<HouseholdProductServiceModel>> GetExpiringAsync(int householdId, string userId);
        Task<HouseholdProductEditModel?> GetProductForEditAsync(int householdProductId, string userId);
        Task<bool> UpdateHouseholdProductAsync(HouseholdProductEditModel model, string userId);
        Task<bool> DeleteHouseholdProductAsync(int householdProductId, string userId);
        Task ToggleFavouriteAsync(int householdProductId, string userId);
        Task AddProductToHouseholdAsync(int householdId, int productId, string userId);
    }
}