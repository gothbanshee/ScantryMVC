using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Scantry.Core.Contracts;
using Scantry.Core.Models;
using Scantry.Data.Models;
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
    }
}
