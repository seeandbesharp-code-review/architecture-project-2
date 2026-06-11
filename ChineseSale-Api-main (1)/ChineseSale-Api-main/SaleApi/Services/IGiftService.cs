using Microsoft.AspNetCore.Mvc;
using SaleApi.Dto;
using SaleApi.Models;
using static SaleApi.Dto.GiftDto;

namespace SaleApi.Services
{
    public interface IGiftService
    {
        Task<IEnumerable<GetGiftDto>> GetAllGift();
        Task<GiftResponseDto> NewGift(CreateGiftDto giftDto);
        Task DeletGift(int id);
        Task<GetGiftDto> GetGiftById(int id);
        Task<GiftResponseDto> UpdateGift(UpdateGiftDto giftDto);
        Task<GiftDonerDto> GetGiftDoner(int id);
        Task<IEnumerable<GetGiftDto>> GetGiftByDoner(string name);
        Task<IEnumerable<GiftResponseDto>> GetGiftByName(string name);
    }
}