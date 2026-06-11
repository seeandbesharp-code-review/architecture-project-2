using Microsoft.EntityFrameworkCore;
using SaleApi.Data;
using SaleApi.Models;

namespace SaleApi.Repositories
{
    public class GiftRepository : IGiftRepository
    {
        SaleContextDB _context;
        public GiftRepository(SaleContextDB saleContextDB)
        {
            _context = saleContextDB;
        }

        ///כל המתנות
        public async Task<IEnumerable<Gift>> GetAllGift()
        {
            try
            {
                return await _context.Gifts.Include(g => g.Doner)
                     .AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllGift repository: {ex.Message}");
                throw;
            }
        }

        //מתנה חדשה
        public async Task<Gift> NewGift(Gift gift)
        {
            _context.Gifts.Add(gift);
            await _context.SaveChangesAsync();
            return gift;
        }


        //מחיקת מתנה
        public async Task DeleteGift(int id)
        {
            var gift = await _context.Gifts.FindAsync(id);
            if (gift != null)
            {
                _context.Gifts.Remove(gift);
                await _context.SaveChangesAsync();

            }

        }

        //GetGiftById
        public async Task<Gift?> GetGiftById(int id)
        {
            return await _context.Gifts.
                Include(c => c.Doner)
                .FirstOrDefaultAsync(c => c.Id == id);
        }


        //עידכון מתנה
        public async Task<Gift> UpdateGift(Gift gift)
        {
            _context.Gifts.Update(gift);
            await _context.SaveChangesAsync();
            return gift;
        }

        // מי התורם 
        public async Task<Doner?> GetGiftDoner(int id)
        {
            //var x = await _context.Gifts.Where(e => e.Id == id).FirstOrDefaultAsync();
            //Gift? gift = await GetGiftById(id);
            //return gift?.Doner;


            var giftWithDoner = await _context.Gifts
                .Include(g => g.Doner)
                .FirstOrDefaultAsync(g => g.Id == id);

            return giftWithDoner?.Doner;
        }

        //חיפוש לפי שם תורם
        public async Task<IEnumerable<Gift?>> GetGiftByDoner(string name)
        {
            var g = await _context.Gifts.Include(g => g.Doner)
                .Where(g=>g.Doner.FirstName.StartsWith(name)
                || g.Doner.LastName.StartsWith(name)).ToArrayAsync();
            return g;
        }


        //חיפוש לפי שם מתנה
        public async Task<IEnumerable<Gift?>> GetGiftByName(string name)
        {
            var g = await _context.Gifts.Include(g => g.Doner)
                .Where(g => g.Name.StartsWith(name))
             .ToArrayAsync();
            return g;
        }
    }
}

