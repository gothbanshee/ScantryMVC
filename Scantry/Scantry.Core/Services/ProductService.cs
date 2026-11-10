using Microsoft.EntityFrameworkCore;
using Scantry.Core.Contracts;
using Scantry.Core.Extensions;
using Scantry.Core.Models;
using Scantry.Core.Models.External;
using Scantry.Data.Models;
using Scantry.Data.Repositories;
using System.Net.Http.Json;
using static Scantry.Core.Extensions.MappingExtensions;

namespace Scantry.Core.Services
{
    public class ProductService(IRepository repository, HttpClient httpClient) : IProductService
    {
        public async Task<List<ProductServiceModel>> GetAllProductsAsync()
        {
            List<Product> products = await repository.All<Product>()
                                                     .OrderBy(p => p.ProductType)
                                                     .ThenBy(p => p.Brand)
                                                     .ToListAsync();

            return products.Count != 0 ? [.. products.Select(p => p.ToServiceModel())]
                                       : [];
        }

        public async Task<int?> GetProductIdByBarcodeAsync(string barcode)
        {
            Product? product = await repository.FirstOrDefaultAsync<Product>(p => p.Barcode == barcode);

            if (product == null)
                return null;
            else
                return product?.ProductId;
        }

        public async Task<ProductServiceModel?> FetchProductFromExternalApiAsync(string barcode)
        {
            ProductServiceModel? product = await QueryOpenFactsApiAsync($"https://world.openproductsfacts.org/api/v0/product/{barcode}.json", barcode);

            product ??= await QueryOpenFactsApiAsync($"https://world.openfoodfacts.org/api/v0/product/{barcode}.json", barcode);

            product ??= await QueryOpenFactsApiAsync($"https://world.openbeautyfacts.org/api/v0/product/{barcode}.json", barcode);

            product ??= await QueryOpenFactsApiAsync($"https://world.openpetfoodfacts.org/api/v0/product/{barcode}.json", barcode);

            return product;
        }

        private async Task<ProductServiceModel?> QueryOpenFactsApiAsync(string url, string barcode)
        {
            try
            {
                using HttpResponseMessage response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                OFFResponse? result = await response.Content.ReadFromJsonAsync<OFFResponse>();

                if (result != null && result.Status == 1 && result.Product != null)
                {
                    return new ProductServiceModel
                    {
                        Barcode = barcode,
                        Brand = result.Product.Brands
                                ?? "Unknown Brand",
                        Model = result.Product.Model
                                ?? "Unknown Product",
                        ProductType = !string.IsNullOrWhiteSpace(result.Product.Category)
                                      ? result.Product.Category
                                      : result.Product.ProductType
                                      ?? "Unknown type",
                        SizeValue = result.Product.SizeValue
                                    ?? 1,
                        SizeUnit = result.Product.SizeUnit
                                   ?? "pcs"
                    };
                }
            }
            catch (TaskCanceledException)
            {
                System.Diagnostics.Debug.WriteLine("The request to the external API timed out.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<int> CreateGlobalProductFromExternalAsync(ProductServiceModel product)
        {
            await repository.AddAsync(product.ToEntity());
            await repository.SaveChangesAsync();

            return product.ProductId;
        }

        public async Task<ProductServiceModel?> GetProductForEditAsync(int productId)
        {
            Product? product = await repository.FirstOrDefaultAsync<Product>(p => p.ProductId == productId);

            if (product == null)
                return null;

            return product.ToServiceModel();
        }

        public async Task<bool> UpdateProductAsync(ProductServiceModel model)
        {
            Product? product = await repository.FirstOrDefaultTrackedAsync<Product>(p => p.ProductId == model.ProductId);

            if (product == null)
                return false;

            product = model.ToEntity();

            return await repository.SaveChangesAsync() > 0;
        }
    }
}