using Microsoft.EntityFrameworkCore;
using SaleApi.Data;
using SaleApi.Models;
using static SaleApi.Dto.CategoryDto;
using static SaleApi.Dto.GiftDto;

namespace SaleApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        SaleContextDB _context;
        public CategoryRepository(SaleContextDB saleContextDB)
        {
            _context = saleContextDB;
        }


        ///כל הקטגוריות
        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            return await _context.Categories.ToListAsync();
        }

        //קטגוריה חדשה
        public async Task<Category> NewCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

         //מחיקת קטגוריה
        public async Task DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

            }

        }

        //GetCategiryById
        public async Task<Category?> GetCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        //עידכון קטגורה
        public async Task<Category> UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        // את כל המוצרים של קטגוריה
        public async Task<List<Gift>> GetGiftByCategoryId(int categoryId)
        {
            return await _context.Gifts.Where(g=>g.CategoryId==categoryId)
                .ToListAsync();
        }
    }
}
