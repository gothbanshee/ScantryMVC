using Scantry.Core.Models;

namespace Scantry.Core.Contracts
{
    public interface IProductService
    {
        Task<List<ProductServiceModel>> GetAllProductsAsync();
        Task<int?> GetProductIdByBarcodeAsync(string barcode);
        Task<ProductServiceModel?> FetchProductFromExternalApiAsync(string barcode);
        Task<int> CreateGlobalProductFromExternalAsync(ProductServiceModel model);
        Task<ProductServiceModel?> GetProductForEditAsync(int productId);
        Task<bool> UpdateProductAsync(ProductServiceModel model);
    }
}