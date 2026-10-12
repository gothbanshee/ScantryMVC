using Scantry.Core.Contracts;
using Scantry.Core.Models;
using Scantry.Data.Models;
using Scantry.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Scantry.Core.Services
{
    public class HouseholdService(IRepository repository) : IHouseholdService
    {
        public async Task CreateHouseholdAsync(HouseholdServiceModel model, string userId)
        {
            var entity = new Household()
            {
                HouseholdName = model.HouseholdName,
                AccessToken = Guid.NewGuid(),
                HouseholdUsers = [],
                HouseholdProducts = []
            };

            entity.HouseholdUsers.Add(new HouseholdUsers()
            {
                HouseholdId = entity.HouseholdId,
                UserId = userId
            });


            await repository.AddAsync<Household>(entity);
            await repository.SaveChangesAsync();
        }

        //public async Task<HouseholdServiceModel> GetHouseholdByIdAsync()
        //{
        //    return await repository.GetByIdAsync<Household>(entity, Id);
        //}

        public async Task<bool> JoinHouseholdAsync(Guid accessToken, string userId)
        {
            Household? household = await repository.All<Household>()
                                    .Include(h => h.HouseholdUsers)
                                    .FirstOrDefaultAsync(h => h.AccessToken == accessToken);

            if (household == null)
            {
                return false;
            }

            if (household.HouseholdUsers.Any(hu => hu.UserId == userId))
            {
                return false;
            }

            var householdUser = new HouseholdUsers
            {
                HouseholdId = household.HouseholdId,
                Household = household,
                UserId = userId,
                ApplicationUser = null!
            };

            await repository.AddAsync(householdUser);
            await repository.SaveChangesAsync();

            return true;
        }

        public async Task<List<HouseholdServiceModel>> GetAllHouseholdsForUser(string userId)
        {

            var householdForUser = await repository.All<HouseholdUsers>()
                .Where(hu => hu.UserId == userId)
                .Include(hu => hu.Household)
                    .ThenInclude(h => h.HouseholdUsers)
                .Include(hu => hu.Household)
                    .ThenInclude(h => h.HouseholdProducts)
                .Select(hu => new HouseholdServiceModel()
                {
                    HouseholdId = hu.HouseholdId,
                    HouseholdName = hu.Household.HouseholdName,
                    AccessToken = hu.Household.AccessToken,
                    HouseholdUsers = hu.Household.HouseholdUsers,
                    InventoryItems = hu.Household.HouseholdProducts
                })
                .ToListAsync();

            if (householdForUser.IsNullOrEmpty())
            {
                return [];
            }

            return householdForUser;
        }

        public async Task<List<HouseholdServiceModel>> GetAllHouseholdsAsync()
        {
            return await repository.All<Household>()
                .Select(h => new HouseholdServiceModel()
                {
                    HouseholdId = h.HouseholdId,
                    HouseholdName = h.HouseholdName,
                    AccessToken = h.AccessToken,
                    HouseholdUsers = h.HouseholdUsers,
                    InventoryItems = h.HouseholdProducts
                })
                .ToListAsync();
        }
    }
}