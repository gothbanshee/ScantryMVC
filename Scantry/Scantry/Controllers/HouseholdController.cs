using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Scantry.Core.Contracts;
using Scantry.Core.Models;
using Scantry.Data.Models;

namespace Scantry.Controllers
{
    [Authorize]
    public class HouseholdController(IHouseholdService householdService, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;

        public async Task<IActionResult> Index()
        {
            List<HouseholdServiceModel> households = await householdService.GetAllHouseholdsForUser(GetUserId());

            if (households.IsNullOrEmpty())
            {
                return RedirectToAction(nameof(Create));
            }

            return View(households);
        }

        [HttpGet]
        public IActionResult Create()
        {
            HouseholdServiceModel model = new();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HouseholdServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await householdService.CreateHouseholdAsync(model, GetUserId());
            return RedirectToAction(nameof(Index));
        }

        private string GetUserId()
        {
            return userManager.GetUserId(User);
        }
    }
}
