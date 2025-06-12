using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category);
        Task<IEnumerable<Product>> GetPopularProductsAsync(int count = 10);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task UpdateProductViewCountAsync(int productId);
        Task<IEnumerable<string>> GetCategoriesAsync();
    }
}