using SaleApi.Dto;
using SaleApi.Models;
using static SaleApi.Dto.BagDto;

namespace SaleApi.Services
{
    public interface IBagService
    {
        Task<IEnumerable<GetBagDto>> GetAllBag();
        Task<Bag> NewGiftToBag(CreateBagDto bagDto);
        Task DeleteBag(int id);
        Task<Bag> GetBagById(int id);
        Task<IEnumerable<Bag>> GetBagByUser(int id);
        Task<IEnumerable<Bag>> GetBagByGift(int id);
        Task<bool> ProcessCheckout(int userId);
    }
}