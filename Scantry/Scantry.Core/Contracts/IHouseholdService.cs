using Scantry.Core.Models;

namespace Scantry.Core.Contracts
{
    public interface IHouseholdService
    {
        Task CreateHouseholdAsync(HouseholdServiceModel model, string userId);

        //Task<HouseholdServiceModel> GetHouseholdByIdAsync();

        Task<bool> JoinHouseholdAsync(Guid accessToken, string userId);

        Task<List<HouseholdServiceModel>> GetAllHouseholdsForUser(string userId);

        Task<List<HouseholdServiceModel>> GetAllHouseholdsAsync();
    }
}