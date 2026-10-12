using Microsoft.AspNetCore.Identity;
namespace Scantry.Data.Models;
// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public List<HouseholdUsers> UserHouseholds { get; set; } = [];
}
