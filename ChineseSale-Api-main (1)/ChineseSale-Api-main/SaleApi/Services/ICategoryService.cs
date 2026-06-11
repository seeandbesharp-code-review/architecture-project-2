using SaleApi.Models;
using static SaleApi.Dto.CategoryDto;
using static SaleApi.Dto.GiftDto;

namespace SaleApi.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategory();
        Task<CreateCategoryDto> NewCategory(CreateCategoryDto categoryDto);
        Task DeleteCategory(int id);
        Task<Category> GetCategoryById(int id);
        Task<GetCategoryDto> UpdateCategory(GetCategoryDto CategoryDto);
        Task<List<GiftResponseDto>> GetGiftByCategoryId(int categoryId);

    }
}