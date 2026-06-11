using SaleApi.Models;

namespace SaleApi.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategory();
        Task<Category> NewCategory(Category category);
        Task DeleteCategory(int id);
        Task<Category> GetCategoryById(int id);
        Task<Category> UpdateCategory(Category category);
        Task<List<Gift>> GetGiftByCategoryId(int categoryId);
    }
}