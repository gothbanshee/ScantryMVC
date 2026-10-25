using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Scantry.Core.Contracts;
using Scantry.Core.Models;
using Scantry.Data.Models;
using static Scantry.GCommon.Messages.GeneralMessages;
using static Scantry.GCommon.Messages.HouseholdMessages;

namespace Scantry.Controllers
{
    [Authorize]
    public class HouseholdController(IHouseholdService householdService, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;

        public async Task<IActionResult> Index()
        {
            List<HouseholdServiceModel> households = await householdService.GetAllHouseholdsForUserWithoutProductsAsync(GetUserId());

            if (households.IsNullOrEmpty())
            {
                TempData["ErrorMessage"] = NoHouseholdJoined;
                return RedirectToAction(nameof(Create));
            }

            return View(households);
        }

        [HttpGet]
        public IActionResult Create()
        {
            HouseholdServiceModel model = new();

            return View("CreateOrJoin", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HouseholdServiceModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), model);
            }

            bool isAdded = await householdService.CreateHouseholdAsync(model, GetUserId());
            if (isAdded)
            {
                TempData["SuccessMessage"] = CreateSuccess;
            }
            else
            {
                TempData["ErrorMessage"] = GeneralError;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int householdId, string householdName)
        {
            if (string.IsNullOrWhiteSpace(householdName))
            {
                TempData["ErrorMessage"] = EmptyHouseholdName;
                return RedirectToAction(nameof(Index));
            }

            bool isRenamed = await householdService.UpdateHouseholdNameAsync(householdId, householdName, GetUserId());

            if (isRenamed)
            {
                TempData["SuccessMessage"] = RenameSuccess;
            }
            else
            {
                TempData["ErrorMessage"] = GeneralError;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Join(Guid accessToken)
        {
            try
            {
                bool joinedSuccessfully = await householdService.JoinHouseholdAsync(accessToken, GetUserId());

                if (joinedSuccessfully)
                {
                    TempData["SuccessMessage"] = JoinSuccess;
                }
                else
                {
                    TempData["ErrorMessage"] = UserAlreadyMember;
                }

                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException)
            {
                TempData["ErrorMessage"] = InvalidJoinLink;
                return View("CreateOrJoin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Join(string accessToken)
        {
            if (!Guid.TryParse(accessToken, out Guid parsedGuid))
            {
                ModelState.AddModelError(string.Empty, InvalidAccessToken);
                return View("CreateOrJoin");
            }

            try
            {
                bool joinedSuccessfully = await householdService.JoinHouseholdAsync(parsedGuid, GetUserId());

                if (joinedSuccessfully)
                {
                    TempData["SuccessMessage"] = JoinSuccess;
                }
                else
                {
                    TempData["ErrorMessage"] = UserAlreadyMember;
                }

                return RedirectToAction(nameof(Index));
            }
            catch (NullReferenceException)
            {
                ModelState.AddModelError(string.Empty, InvalidAccessToken);
                return View("CreateOrJoin");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Leave(int householdId)
        {
            List<HouseholdUserServiceModel> householdUsers = await householdService.GetAllUsersForHouseholdAsync(householdId);

            bool isDeleted = await (householdService.LeaveHouseholdAsync(householdId, GetUserId()));

            if (isDeleted)
            {
                TempData["SuccessMessage"] = LeaveSuccess;
            }
            else
            {
                TempData["ErrorMessage"] = GeneralError;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await householdService.DeleteHousehold(id, GetUserId());

            if (isDeleted)
            {
                TempData["SuccessMessage"] = DeleteSuccess;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = GeneralError;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            HouseholdServiceModel model = await householdService.GetHouseholdByIdAsync(id, GetUserId());

            return View(model);
        }

        private string GetUserId()
        {
            return userManager.GetUserId(User)
                ?? throw new InvalidOperationException(UserIdNotFound);
        }
    }
}