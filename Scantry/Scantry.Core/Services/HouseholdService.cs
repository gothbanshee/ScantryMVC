using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scantry.Core.Contracts;
using Scantry.Core.Extensions;
using Scantry.Core.Models;
using Scantry.Data.Models;
using Scantry.Data.Repositories;

namespace Scantry.Core.Services
{
    public class HouseholdService(IRepository repository) : BaseService(repository), IHouseholdService
    {
        public async Task<bool> CreateHouseholdAsync(HouseholdServiceModel householdModel, string userId)
        {
            Household householdEntity = householdModel.ToEntity();

            householdEntity.HouseholdUsers.Add(new HouseholdUser()
            {
                ApplicationUserId = userId
            });

            await repository.AddAsync(householdEntity);

            return await repository.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateHouseholdNameAsync(int householdId, string householdName, string userId)
        {
            Household? household = await repository.AllTracked<Household>()
                                        .Include(h => h.HouseholdUsers)
                                        .FirstOrDefaultAsync(h => h.HouseholdId == householdId)
                                        ?? throw new NullReferenceException();

            await IsUserAMember(household.HouseholdId, userId);

            household.HouseholdName = householdName;

            return await repository.SaveChangesAsync() > 0;
        }

        public async Task<bool> JoinHouseholdAsync(Guid accessToken, string userId)
        {
            Household? household = await repository.FirstOrDefaultAsync<Household>(h => h.AccessToken == accessToken)
                                    ?? throw new NullReferenceException();

            HouseholdUser? userFound = await repository
                .FirstOrDefaultAsync<HouseholdUser>(hu => hu.HouseholdId == household.HouseholdId && hu.ApplicationUserId == userId);

            if (userFound != null)
            {
                return false;
            }

            HouseholdUser householdUser = new()
            {
                HouseholdId = household.HouseholdId,
                ApplicationUserId = userId
            };

            await repository.AddAsync<HouseholdUser>(householdUser);
            int isAdded = await repository.SaveChangesAsync();

            return isAdded > 0;
        }

        public async Task<bool> LeaveHouseholdAsync(int householdId, string userId)
        {
            List<HouseholdUserServiceModel> usersInHousehold = await GetAllUsersForHouseholdAsync(householdId);

            await IsUserAMember(householdId, userId);

            if (usersInHousehold.Count == 1)
            {
                return await DeleteHousehold(householdId, userId);
            }
            else
            {
                int rowsDeleted = await repository.DeleteAsync<HouseholdUser>(hu => hu.HouseholdId == householdId && hu.ApplicationUserId == userId);
                return rowsDeleted > 0;
            }
        }

        public async Task<bool> DeleteHousehold(int householdId, string userId)
        {
            await IsUserAMember(householdId, userId);

            int householdDeleted = await repository.DeleteAsync<Household>(h => h.HouseholdId == householdId);

            return householdDeleted > 0;
        }

        public async Task<List<HouseholdServiceModel>> GetAllHouseholdsForUserWithoutProductsAsync(string userId)
        {
            List<HouseholdUser> householdsForUser = await repository
                .GetAllWithPredicate<HouseholdUser>(hu => hu.ApplicationUserId == userId)
                .Include(hu => hu.Household)
                .Include(hu => hu.ApplicationUser)
                .ToListAsync();

            return householdsForUser == null ? []
                                             : [.. householdsForUser
                                             .Select(hu => hu.Household.ToServiceModel())];
        }

        public async Task<List<HouseholdUserServiceModel>> GetAllUsersForHouseholdAsync(int householdId)
        {
            List<HouseholdUserServiceModel> users = await repository.GetAllWithPredicate<HouseholdUser>(hu => hu.HouseholdId == householdId)
                .Include(hu => hu.ApplicationUser)
                .Select(hu => hu.ToServiceModel())
                .ToListAsync();

            return users;
        }

        public async Task<HouseholdServiceModel> GetHouseholdByIdAsync(int householdId, string userId)
        {
            Household? household = await repository.GetAllWithPredicate<Household>(h => h.HouseholdId == householdId)
                    .Include(h => h.HouseholdUsers)
                        .ThenInclude(hu => hu.ApplicationUser)
                    .Include(h => h.HouseholdProducts)
                        .ThenInclude(hp => hp.Product)
                    .FirstOrDefaultAsync()
                    ?? throw new NullReferenceException();

            await IsUserAMember(household.HouseholdId, userId);

            return household.ToServiceModel();
        }

        /* will need this for admins only */
        //public async Task<List<HouseholdServiceModel>> GetAllHouseholdsAsync()
        //{
        //    return await repository.All<Household>()
        //        .Select(h => new HouseholdServiceModel()
        //        {
        //            HouseholdId = h.HouseholdId,
        //            HouseholdName = h.HouseholdName,
        //            AccessToken = h.AccessToken,
        //            HouseholdUsers = h.HouseholdUsers,
        //            HouseholdProducts = h.HouseholdProducts
        //        })
        //        .ToListAsync();
        //}
    }
}