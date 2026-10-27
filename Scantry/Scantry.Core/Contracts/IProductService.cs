using Scantry.Core.Models;

namespace Scantry.Core.Contracts
{
    public interface IProductService
    {
        Task<List<ProductServiceModel>> GetAllProductsAsync();
        Task<int?> GetProductIdByBarcodeAsync(string barcode);
        Task<ExternalProductModel?> FetchProductFromExternalApiAsync(string barcode);
        Task<int> CreateGlobalProductFromExternalAsync(ExternalProductModel model);
        Task<ProductServiceModel?> GetProductForEditAsync(int productId);
        Task<bool> UpdateProductAsync(ProductServiceModel model);
    }
}