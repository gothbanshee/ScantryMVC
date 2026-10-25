using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scantry.Core.Contracts;
using Scantry.Core.Models;
using Scantry.Data.Models;
using Scantry.Data.Repositories;

namespace Scantry.Core.Services
{
    public class HouseholdService(IRepository repository) : IHouseholdService
    {
        public async Task<bool> CreateHouseholdAsync(HouseholdServiceModel model, string userId)
        {
            Household entity = new()
            {
                HouseholdName = model.HouseholdName,
                AccessToken = Guid.NewGuid(),
                HouseholdUsers = [],
                HouseholdProducts = []
            };

            entity.HouseholdUsers.Add(new HouseholdUsers()
            {
                HouseholdId = entity.HouseholdId,
                ApplicationUserId = userId
            });

            await repository.AddAsync(entity);
            int isSaved = await repository.SaveChangesAsync();

            return isSaved > 0;
        }

        public async Task<bool> UpdateHouseholdNameAsync(int householdId, string householdName, string userId)
        {
            Household? household = await repository.AllTracked<Household>()
                                        .Include(h => h.HouseholdUsers)
                                        .FirstOrDefaultAsync(h => h.HouseholdId == householdId)
                                        ?? throw new NullReferenceException();

            List<string> userIdsList = [.. household.HouseholdUsers.Select(hu => hu.ApplicationUserId)];

            if (IsUserAMember(userIdsList, userId) == false)
            {
                throw new UnauthorizedAccessException();
            }

            household.HouseholdName = householdName;
            return await repository.SaveChangesAsync() > 0;

        }

        public async Task<bool> JoinHouseholdAsync(Guid accessToken, string userId)
        {
            Household? household = await repository.All<Household>()
                                    .Include(h => h.HouseholdUsers)
                                    .FirstOrDefaultAsync(h => h.AccessToken == accessToken)
                                    ?? throw new NullReferenceException();

            List<string> userIdsList = [.. household.HouseholdUsers.Select(hu => hu.ApplicationUserId)];

            if (IsUserAMember(userIdsList, userId))
            {
                return false;
            }

            HouseholdUsers householdUser = new()
            {
                HouseholdId = household.HouseholdId,
                ApplicationUserId = userId,
            };

            await repository.AddAsync<HouseholdUsers>(householdUser);
            int isAdded = await repository.SaveChangesAsync();

            return isAdded > 0;
        }

        public async Task<bool> LeaveHouseholdAsync(int householdId, string userId)
        {
            List<ApplicationUser> usersInHousehold = await GetAllUsersForHouseholdAsync(householdId);
            List<string> userIdsList = [.. usersInHousehold.Select(u => u.Id)];

            if(IsUserAMember(userIdsList, userId) == false)
            {
                throw new UnauthorizedAccessException();
            }

            if (usersInHousehold.Count == 1)
            {
                return await DeleteHousehold(householdId, userId);
            }
            else
            {
                int rowsDeleted = await repository.DeleteAsync<HouseholdUsers>(hu => hu.HouseholdId == householdId && hu.ApplicationUserId == userId);
                return rowsDeleted > 0;
            }
        }

        public async Task<bool> DeleteHousehold(int householdId, string userId)
        {
            List<ApplicationUser> usersInHousehold = await GetAllUsersForHouseholdAsync(householdId);
            List<string> userIdsList = [.. usersInHousehold.Select(u => u.Id)];

            if (IsUserAMember(userIdsList, userId) == false)
            {
                throw new UnauthorizedAccessException();
            }

            int householdDeleted = await repository.DeleteAsync<Household>(h => h.HouseholdId == householdId);

            return householdDeleted > 0;
        }

        public async Task<List<HouseholdServiceModel>> GetAllHouseholdsForUserWithoutProductsAsync(string userId)
        {
            List<HouseholdServiceModel> householdsForUser = await repository.GetAllWithPredicate<HouseholdUsers>(hu => hu.ApplicationUserId == userId)
             .Include(hu => hu.Household)
                 .ThenInclude(h => h.HouseholdUsers)
                 .ThenInclude(hu => hu.ApplicationUser)
             .Select(hu => new HouseholdServiceModel()
             {
                 HouseholdId = hu.HouseholdId,
                 HouseholdName = hu.Household.HouseholdName,
                 AccessToken = hu.Household.AccessToken.ToString(),
                 HouseholdUsers = hu.Household.HouseholdUsers
             })
             .ToListAsync();

            if (householdsForUser.IsNullOrEmpty())
            {
                return [];
            }

            return householdsForUser;
        }

        public async Task<List<ApplicationUser>> GetAllUsersForHouseholdAsync(int householdId)
        {
            List<ApplicationUser> users = await repository.GetAllWithPredicate<HouseholdUsers>(hu => hu.HouseholdId == householdId)
                .Include(hu => hu.ApplicationUser)
                .Select(hu => new ApplicationUser()
                {
                    UserName = hu.ApplicationUser.UserName,
                    Id = hu.ApplicationUser.Id
                })
                .ToListAsync();

            return users;
        }

        public async Task<HouseholdServiceModel> GetHouseholdByIdAsync(int householdId, string userId)
        {
            Household? household = await repository
                    .GetAllWithPredicate<Household>(h => h.HouseholdId == householdId)
                    .Include(h => h.HouseholdUsers)
                    .Include(h => h.HouseholdProducts)
                        .ThenInclude(hp => hp.Product)
                    .FirstOrDefaultAsync()
                    ?? throw new NullReferenceException();

            if (household.HouseholdUsers.Any(hu => hu.ApplicationUserId == userId) == false)
            {
                throw new UnauthorizedAccessException();
            }

            HouseholdServiceModel model = new()
            {
                HouseholdId = household.HouseholdId,
                AccessToken = household.AccessToken.ToString(),
                HouseholdName = household.HouseholdName,
                HouseholdUsers = household.HouseholdUsers,
                HouseholdProducts = household.HouseholdProducts
            };

            return model;
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

        private static bool IsUserAMember(List<string> userIdsList, string userId)
        {
            return userIdsList.Contains(userId);
        }
    }
}