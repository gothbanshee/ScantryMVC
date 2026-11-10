using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Scantry.Core.Contracts;
using Scantry.Core.Models;
using Scantry.Data.Models;
using Scantry.GCommon.Enums;
using static Scantry.GCommon.Messages.GeneralMessages;
using static Scantry.GCommon.Messages.ProductMessages;

namespace Scantry.Controllers
{
    [Authorize]
    public class ProductController(IProductService productService, UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            List<ProductServiceModel> products = await productService.GetAllProductsAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ProductServiceModel? product = await productService.GetProductForEditAsync(id);

            if (product == null)
            {
                TempData["ErrorMessage"] = GlobalProductNotFound;
                return RedirectToAction("Index", "Product");
            }

            ViewBag.SizeUnits = new SelectList(GetSizeUnitsList(), product?.SizeUnit);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductServiceModel product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SizeUnits = new SelectList(GetSizeUnitsList(), product.SizeUnit);
                return View(product);
            }

            bool isUpdated = await productService.UpdateProductAsync(product);
            if (isUpdated)
            {
                TempData["SuccessMessage"] = productEditSuccess;
            }
            else
            {
                TempData["ErrorMessage"] = productEditFail;
            }

            return RedirectToAction("Index", "Product");
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