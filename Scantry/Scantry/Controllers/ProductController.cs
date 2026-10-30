using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Scantry.Core.Contracts;
using Scantry.Core.Models;
using Scantry.Data.Models;
using Scantry.GCommon.Enums;
using static Scantry.GCommon.Messages.GeneralMessages;

namespace Scantry.Controllers
{
    public class ProductController(IProductService productService, IHouseholdProductService householdProductService, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await productService.GetAllProductsAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await productService.GetProductForEditAsync(id);

            if (model == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.SizeUnits = new SelectList(GetSizeUnitsList(), model?.SizeUnit);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductServiceModel product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            bool isUpdated = await productService.UpdateProductAsync(product);
            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Global product catalog updated!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update product.";
            }

            return RedirectToAction("Index", "Product");
        }

        private string GetUserId()
        {
            return userManager.GetUserId(User)
                ?? throw new InvalidOperationException(UserIdNotFound);
        }

        private static List<string> GetSizeUnitsList()
        {
            return typeof(SizeUnits)
                .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
                .Select(f => (string)f.GetValue(null)!)
                .ToList();
        }
    }
}
