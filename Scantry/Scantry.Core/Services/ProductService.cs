using Microsoft.EntityFrameworkCore;
using Scantry.Core.Contracts;
using Scantry.Core.Models;
using Scantry.Data.Models;
using Scantry.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace Scantry.Core.Services
{
    public class ProductService(IRepository repository, HttpClient httpClient) : IProductService
    {
        public async Task<List<ProductServiceModel>> GetAllProductsAsync()
        {
            var products = await repository.All<Product>()
                .OrderBy(hp => hp.ProductType)
                .ToListAsync();

            if (products.Count == 0)
            {
                return [];
            }

            return products.Select(p => new ProductServiceModel
            {
                ProductId = p.ProductId,
                Barcode = p.Barcode,
                ProductType = p.ProductType,
                Brand = p.Brand,
                Model = p.Model,
                Variant = p.Variant,
                SizeValue = p.SizeValue,
                SizeUnit = p.SizeUnit
            }).ToList();
        }

        public async Task<int?> GetProductIdByBarcodeAsync(string barcode)
        {
            var product = await repository.All<Product>().FirstOrDefaultAsync<Product>(p => p.Barcode == barcode);

            return product?.ProductId;
        }

        public async Task<ExternalProductModel?> FetchProductFromExternalApiAsync(string barcode)
        {
            // 1. Try Open Beauty Facts first (for cosmetics, lotions, hygiene, diapers)
            var product = await QueryOpenFactsApiAsync($"https://world.openbeautyfacts.org/api/v0/product/{barcode}.json", barcode);

            // 2. Fallback to Open Food Facts if not found on Beauty
            if (product == null)
            {
                product = await QueryOpenFactsApiAsync($"https://world.openfoodfacts.org/api/v0/product/{barcode}.json", barcode);
            }

            return product;
        }

        private async Task<ExternalProductModel?> QueryOpenFactsApiAsync(string url, string barcode)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.UserAgent.ParseAdd("ScantryApp/1.0 (contact@scantry.com)");

                var response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode) return null;

                var result = await response.Content.ReadFromJsonAsync<OpenFoodFactsResponse>();

                if (result != null && result.Status == 1 && result.Product != null)
                {
                    return new ExternalProductModel
                    {
                        Barcode = barcode,
                        Brand = result.Product.Brands ?? "Unknown Brand",
                        Name = result.Product.ProductName ?? "Unknown Product",
                        ProductType = result.Product.Quantity ?? "Standard",
                        Category = "Hygiene"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public async Task<int> CreateGlobalProductFromExternalAsync(ExternalProductModel model)
        {
            var product = new Product
            {
                Barcode = model.Barcode,
                Brand = model.Brand,
                Model = model.Name,
                ProductType = model.ProductType,
                SizeValue = model.SizeValue ?? 1,
                SizeUnit = model.SizeUnit
            };

            await repository.AddAsync(product);
            await repository.SaveChangesAsync();

            return product.ProductId;
        }

        private class OpenFoodFactsResponse
        {
            [JsonPropertyName("status")]
            public int Status { get; set; }

            [JsonPropertyName("product")]
            public OFFProductDetails? Product { get; set; }
        }

        private class OFFProductDetails
        {
            [JsonPropertyName("brands")]
            public string? Brands { get; set; }

            [JsonPropertyName("product_name")]
            public string? ProductName { get; set; }

            [JsonPropertyName("quantity")]
            public string? Quantity { get; set; }
        }

        public async Task<ProductServiceModel?> GetProductForEditAsync(int productId)
        {
            var product = await repository.All<Product>()
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null) return null;

            return new ProductServiceModel
            {
                ProductId = product.ProductId,
                Barcode = product.Barcode,
                Brand = product.Brand,
                Model = product.Model,
                ProductType = product.ProductType,
                Variant = product.Variant,
                SizeValue = product.SizeValue,
                SizeUnit = product.SizeUnit
            };
        }

        public async Task<bool> UpdateProductAsync(ProductServiceModel model)
        {
            var product = await repository.AllTracked<Product>()
                .FirstOrDefaultAsync(p => p.ProductId == model.ProductId);

            if (product == null) return false;

            product.Barcode = model.Barcode;
            product.Brand = model.Brand;
            product.Model = model.Model;
            product.ProductType = model.ProductType;
            product.Variant = model.Variant;
            product.SizeValue = model.SizeValue;
            product.SizeUnit = model.SizeUnit;

            return await repository.SaveChangesAsync() > 0;
        }
    }
}
