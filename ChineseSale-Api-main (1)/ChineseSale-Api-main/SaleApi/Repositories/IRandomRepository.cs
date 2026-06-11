
using SaleApi.Models;

namespace SaleApi.Repositories
{
    public interface IRandomRepository
    {
        Task<IEnumerable<int>> GetOrdersForGift(int giftId);
        Task<bool> IsGiftRandom(int giftId);
        Task SaveWinner(Winner winner, int orderId);
        Task<Order> GetOrderById(int orderId);
        Task<IEnumerable<Winner>> GetDrawnGiftIdsAsync();
        Task<bool> IsGiftDrawnAsync(int giftId);

    }
}