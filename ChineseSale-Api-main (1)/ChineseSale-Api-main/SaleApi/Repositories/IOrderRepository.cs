using SaleApi.Models;

namespace SaleApi.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order?> AddOrder(Order order);
        Task<IEnumerable<Order>> GetOrdersByGiftId(int giftId);
        Task<IEnumerable<Order>> GetOrdersSortedByPopularity();
        Task<IEnumerable<Order>> GetOrdersSortedByPrice();
        Task<IEnumerable<Order>> GetOrdersByUserId(int userId);
    }
}
