using Scantry.Data.Models;
using Scantry.Data.Repositories;

namespace Scantry.Core.Services
{
    public abstract class BaseService(IRepository repository)
    {
        protected readonly IRepository repository = repository;

        protected async Task<bool> IsUserAMember(int householdId, string userId)
        {
            HouseholdUser? userFound = await repository.FirstOrDefaultAsync<HouseholdUser>(hu => hu.HouseholdId == householdId && hu.ApplicationUserId == userId) 
                ?? throw new UnauthorizedAccessException();
            return true;
        }
    }
}
