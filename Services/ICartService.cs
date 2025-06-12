using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface ICartService
    {
        Task<IEnumerable<CartItem>> GetCartItemsAsync(string sessionId);
        Task AddToCartAsync(int productId, int quantity, string sessionId);
        Task UpdateCartItemAsync(int cartItemId, int quantity);
        Task RemoveFromCartAsync(int cartItemId);
        Task ClearCartAsync(string sessionId);
        Task<decimal> GetCartTotalAsync(string sessionId);
        Task<int> GetCartItemCountAsync(string sessionId);
    }
}