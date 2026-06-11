using SaleApi.Dto;
using SaleApi.Models;
using SaleApi.Repositories;
using static SaleApi.Dto.GiftDto;
using static SaleApi.Dto.OrderDto;

namespace SaleApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBagRepository _bagRepository;

        public OrderService(IOrderRepository orderRepository, IBagRepository bagRepository)
        {
            _orderRepository = orderRepository;
            _bagRepository = bagRepository;
        }


        public async Task<IEnumerable<GetOrderDto>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            return orders.Select(o => MapToDto(o));
        }

        public async Task<AddOrderDto?> AddOrder(AddOrderDto dto)
        {
            var order = new Order { IdUser = dto.IdUser, IdGift = dto.IdGift, Win = false };
            var created = await _orderRepository.AddOrder(order);
            return created != null ? dto : null;
        }

        public async Task<IEnumerable<GetOrderDto>> GetOrdersByGiftId(int giftId)
        {
            var orders = await _orderRepository.GetOrdersByGiftId(giftId);
            return orders.Select(o => MapToDto(o));
        }

        private GetOrderDto MapToDto(Order o) => new GetOrderDto
        {
            Id = o.Id,
            IdUser = o.IdUser,
            Win = o.Win,
            User = o.User != null ? new UserShortDto { FirstName = o.User.FirstName, LastName = o.User.LastName } : null,
            Gift = o.Gift != null ? new OrderShortDto { Id = o.Gift.Id, Name = o.Gift.Name, Price = (int)o.Gift.Price } : null
        };


        public async Task<IEnumerable<object>> GetUserHistoryAsync(int userId)
        {
            // 1. שולפים את כל השורות הגולמיות מה-Repository
            var allOrders = await _orderRepository.GetOrdersByUserId(userId);

            // 2. כאן קורה השינוי - מקבצים לפי ה-OrderGroupId
            var groupedOrders = allOrders
                .GroupBy(o => o.OrderGroupId) // זה מה שיוצר את ההפרדה בין רכישה לרכישה
                .Select(group => new
                {
                    OrderNumber = group.Key, // זה ה-ID של הרכישה הספציפית
                    TotalAmount = group.Sum(o => o.Gift.Price), // סכום כולל של אותה הזמנה
                                                                // כאן אנחנו מכניסים את רשימת המתנות שהיו בתוך הרכישה הזו
                    Items = group.Select(o => new
                    {
                        GiftName = o.Gift.Name,
                        Price = o.Gift.Price,
                        Img = o.Gift.Img
                    }).ToList()
                })
                .OrderByDescending(g => g.OrderNumber) // שההזמנה האחרונה תהיה ראשונה
                .ToList();

            return groupedOrders;
        }

        public async Task<IEnumerable<GetGiftDto>> GetOrdersSortedByPopularity()
        {
            // 1. נשלוף את כל ההזמנות מה-Repository
            var allOrders = await _orderRepository.GetAllOrders();

            // 2. לוגיקה למניעת כפילויות ומיון:
            var popularGifts = allOrders
                .Where(o => o.Gift != null) // מוודא שאין הזמנות בלי מתנה
                .GroupBy(o => o.IdGift)     // מקבץ את כל הרכישות של אותה מתנה
                .OrderByDescending(g => g.Count()) // המתנה עם הכי הרבה רכישות תהיה ראשונה
                .Select(g => new GetGiftDto // יוצר אובייקט מתנה יחיד מכל קבוצה
                {
                    Id = g.First().Gift.Id,
                    Name = g.First().Gift.Name,
                    Description = g.First().Gift.Description,
                    Price = g.First().Gift.Price,
                    Img = g.First().Gift.Img
                    // ודאי ששמות השדות כאן תואמים ל-GetGiftDto שלך
                })
                .ToList();

            return popularGifts;
        }
        // 3. מיון לפי המתנה היקרה ביותר
        public async Task<IEnumerable<GetGiftDto>> GetOrdersSortedByPrice()
        {
            // 1. נמשוך את כל ההזמנות (שכוללות בתוכן את אובייקט המתנה)
            var allOrders = await _orderRepository.GetAllOrders();

            // 2. לוגיקה להוצאת רשימת מתנות ייחודית ומיונה לפי מחיר
            var sortedGifts = allOrders
                .Where(o => o.Gift != null)       // מוודא שיש מתנה מחוברת להזמנה
                .Select(o => o.Gift)              // עובר מהזמנה לאובייקט המתנה עצמו
                .GroupBy(g => g.Id)               // מקבץ לפי מזהה מתנה כדי למנוע כפילויות
                .Select(group => group.First())   // לוקח נציג אחד מכל מתנה שנרכשה
                .OrderByDescending(g => g.Price)  // ממיין לפי מחיר מהגבוה לנמוך
                .Select(g => new GetGiftDto       // ממפה ל-DTO שאת רוצה להחזיר
                {
                    Id = g.Id,
                    Name = g.Name,
                    Description = g.Description,
                    Price = g.Price,
                    Img = g.Img
                    // הוסיפי שדות נוספים אם קיימים ב-GetGiftDto שלך
                })
                .ToList();

            return sortedGifts;
        }

    }
}