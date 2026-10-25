using Scantry.Core.Models;
using Scantry.Data.Models;

namespace Scantry.Core.Contracts
{
    public interface IHouseholdService
    {
        Task<bool> CreateHouseholdAsync(HouseholdServiceModel model, string userId);

        Task<bool> UpdateHouseholdNameAsync(int householdId, string householdName, string userId);

        Task<bool> JoinHouseholdAsync(Guid accessToken, string userId);

        Task<bool> LeaveHouseholdAsync(int householdId, string userId);

        Task<bool> DeleteHousehold(int householdId, string userId);

        Task<HouseholdServiceModel> GetHouseholdByIdAsync(int householdId, string userId);

        Task<List<HouseholdServiceModel>> GetAllHouseholdsForUserWithoutProductsAsync(string userId);

        Task<List<HouseholdUserServiceModel>> GetAllUsersForHouseholdAsync(int householdId);

        /* will need this for admins only*/
        //Task<List<HouseholdServiceModel>> GetAllHouseholdsAsync();
    }
}