using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Scantry.Core.Contracts;
using Scantry.Core.Models;
using Scantry.Data.Models;
using Scantry.GCommon.Enums;
using static Scantry.Extensions.ClaimsPrincipalExtensions;
using static Scantry.GCommon.Messages.GeneralMessages;
using static Scantry.GCommon.Messages.HouseholdProductMessages;

namespace Scantry.Controllers
{
    [Authorize]
    public class HouseholdProductController(IHouseholdProductService householdProductService, IProductService productService) : Controller
    {
        public async Task<IActionResult> MyProducts(int id, string householdName)
        {
            List<HouseholdProductServiceModel> products = await householdProductService.GetAllProductsForHouseholdAsync(id, User.GetUserId());

            ViewBag.HouseholdName = householdName;
            ViewBag.HouseholdId = id;
            ViewBag.PageTitle = "Inventory";
            ViewBag.HeaderIcon = "bi bi-box-seam";
            ViewBag.EmptyMessage = NoProductsForHousehold;

            return View("Index", products);
        }

        public async Task<IActionResult> LowStockProducts(int id, string householdName)
        {
            List<HouseholdProductServiceModel> lowStockProducts = await householdProductService.GetLowStockAsync(id, User.GetUserId());

            ViewBag.HouseholdName = householdName;
            ViewBag.HouseholdId = id;
            ViewBag.PageTitle = "Low-Stock Items";
            ViewBag.HeaderIcon = "bi bi-exclamation-triangle";
            ViewBag.EmptyMessage = NoLowStockProducts;

            return View("Index", lowStockProducts);
        }

        public async Task<IActionResult> Favourites(int id, string householdName)
        {
            List<HouseholdProductServiceModel> favouriteProducts = await householdProductService.GetFavouritesAsync(id, User.GetUserId());

            ViewBag.HouseholdName = householdName;
            ViewBag.HouseholdId = id;
            ViewBag.PageTitle = "Favourite Items";
            ViewBag.HeaderIcon = "bi bi-star";
            ViewBag.EmptyMessage = NoFavouriteProducts;

            return View("Index", favouriteProducts);
        }

        public async Task<IActionResult> Expiring(int id, string householdName)
        {
            List<HouseholdProductServiceModel> expiringProducts = await householdProductService.GetExpiringAsync(id, User.GetUserId());

            ViewBag.HouseholdName = householdName;
            ViewBag.HouseholdId = id;
            ViewBag.PageTitle = "Expiring Items";
            ViewBag.HeaderIcon = "bi bi-tags";
            ViewBag.EmptyMessage = NoExpiringProducts;

            return View("Index", expiringProducts);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavourite(int id)
        {
            try
            {
                await householdProductService.ToggleFavouriteAsync(id, User.GetUserId());
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = GeneralError;
            }

            TempData["SuccessMessage"] = FavouriteUpdateSuccess;

            string? referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Household");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            HouseholdProductEditModel? model = await householdProductService.GetProductForEditAsync(id, User.GetUserId());

            if (model == null)
            {
                TempData["ErrorMessage"] = HouseholdProductNotFound;
                return RedirectToAction("Index", "Household");
            }

            ViewBag.Categories = new SelectList(GetCategoriesList(), model.Category);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(HouseholdProductEditModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(GetCategoriesList(), model.Category);
                return View(model);
            }

            bool isUpdated = await householdProductService.UpdateHouseholdProductAsync(model, User.GetUserId());

            if (isUpdated)
            {
                TempData["SuccessMessage"] = UpdateProductSuccess;
            }
            else
            {
                TempData["ErrorMessage"] = GeneralError;
            }

            return RedirectToAction(nameof(MyProducts), new { id = model.HouseholdId, householdName = model.HouseholdName });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await householdProductService.DeleteHouseholdProductAsync(id, User.GetUserId());

            if (isDeleted)
            {
                TempData["SuccessMessage"] = DeleteProductSuccess;
            }
            else
            {
                TempData["ErrorMessage"] = GeneralError;
            }

            string? referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index", "Household");
        }

        [HttpGet]
        public IActionResult Scan(int id, string householdName)
        {
            ViewBag.HouseholdId = id;
            ViewBag.HouseholdName = householdName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> HandleScannedBarcode(int householdId, string householdName, string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                TempData["ErrorMessage"] = "Invalid barcode scanned.";
                return RedirectToAction(nameof(MyProducts), new { id = householdId, householdName });
            }

            try
            {
                int? productId = await productService.GetProductIdByBarcodeAsync(barcode);

                if (productId == null)
                {
                    ProductServiceModel? externalProduct = await productService.FetchProductFromExternalApiAsync(barcode);

                    if (externalProduct == null)
                    {
                        TempData["ErrorMessage"] = $"Product with barcode {barcode} not found anywhere. Please report it for manual registration.";
                        return RedirectToAction("Index", "Product");
                    }

                    productId = await productService.CreateGlobalProductFromExternalAsync(externalProduct);
                }

                await householdProductService.AddProductToHouseholdAsync(householdId, productId.Value, User.GetUserId());
                TempData["SuccessMessage"] = ProductAddedToHousehold;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while processing the scanned barcode.";
            }

            return RedirectToAction(nameof(MyProducts), new { id = householdId, householdName });
        }

        private static List<string> GetCategoriesList()
        {
            return typeof(HouseholdProductCategory)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                .Select(f => (string)f.GetValue(null)!)
                .ToList();
        }
    }
}