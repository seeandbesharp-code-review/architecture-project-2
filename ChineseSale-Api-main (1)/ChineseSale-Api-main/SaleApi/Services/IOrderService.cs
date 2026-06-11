using SaleApi.Dto; // חשוב מאוד!
using static SaleApi.Dto.GiftDto;
using static SaleApi.Dto.OrderDto; // כדי שיכיר את GetOrderDto ו-AddOrderDto

namespace SaleApi.Services
{
    public interface IOrderService
    {
        //Task<bool> CloseBagToOrder(int userId);
        Task<IEnumerable<GetOrderDto>> GetAllOrders();
        Task<AddOrderDto?> AddOrder(AddOrderDto dto);
        Task<IEnumerable<GetOrderDto>> GetOrdersByGiftId(int giftId);
        Task<IEnumerable<object>> GetUserHistoryAsync(int userId);
        Task<IEnumerable<GetGiftDto>> GetOrdersSortedByPopularity();
        Task<IEnumerable<GetGiftDto>> GetOrdersSortedByPrice();
    }
}