using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Scantry.Core.Contracts;
using Scantry.Core.Models;
using Scantry.Data.Models;
using static Scantry.GCommon.Messages.GeneralMessages;
using static Scantry.GCommon.Messages.HouseholdMessages;
using static Scantry.GCommon.Constants.EntityValidationConstants;
using static Scantry.Extensions.ClaimsPrincipalExtensions;

namespace Scantry.Controllers
{
    [Authorize]
    public class HouseholdController(IHouseholdService householdService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<HouseholdServiceModel> households = await householdService.GetAllHouseholdsForUserWithoutProductsAsync(User.GetUserId());

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
                ViewBag.ErrorMessage = HouseholdNameTooLong;
                return View("CreateOrJoin", model);
            }

            bool isAdded = await householdService.CreateHouseholdAsync(model, User.GetUserId());

            if (isAdded)
            {
                TempData["SuccessMessage"] = CreateHouseholdSuccess;
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
            }
            else if (householdName.Length > HouseholdNameMaxLength)
            {
                TempData["ErrorMessage"] = HouseholdNameTooLong;
            }
            else
            {
                bool isRenamed = await householdService.UpdateHouseholdNameAsync(householdId, householdName, User.GetUserId());

                if (isRenamed)
                {
                    TempData["SuccessMessage"] = RenameSuccess;
                }
                else
                {
                    TempData["ErrorMessage"] = GeneralError;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Join(Guid accessToken)
        {
            try
            {
                bool joinedSuccessfully = await householdService.JoinHouseholdAsync(accessToken, User.GetUserId());

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
                ViewBag.ErrorMessage = InvalidJoinLink;
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
                bool joinedSuccessfully = await householdService.JoinHouseholdAsync(parsedGuid, User.GetUserId());

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

            bool isDeleted = await (householdService.LeaveHouseholdAsync(householdId, User.GetUserId()));

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
            bool isDeleted = await householdService.DeleteHousehold(id, User.GetUserId());

            if (isDeleted)
            {
                TempData["SuccessMessage"] = DeleteSuccess;
            }
            else
            {
                TempData["ErrorMessage"] = GeneralError;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            HouseholdServiceModel model = await householdService.GetHouseholdByIdAsync(id, User.GetUserId());

            return View(model);
        }
    }
}