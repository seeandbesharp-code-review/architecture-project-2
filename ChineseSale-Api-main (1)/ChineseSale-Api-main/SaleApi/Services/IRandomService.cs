
using SaleApi.Models;

namespace SaleApi.Services
{
    public interface IRandomService
    {
        Task<int?> PickWinner(int giftId);
        Task<Winner?> ExecuteDraw(int giftId);
        Task<IEnumerable<Winner>> GetAllWinnersAsync();
            Task<bool> IsGiftDrawnAsync(int giftId);
    }
}