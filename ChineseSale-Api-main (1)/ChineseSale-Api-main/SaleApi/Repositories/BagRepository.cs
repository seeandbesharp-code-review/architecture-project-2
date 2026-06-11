using Microsoft.EntityFrameworkCore;
using SaleApi.Data;
using SaleApi.Models;

namespace SaleApi.Repositories
{
    public class BagRepository : IBagRepository
    {

        SaleContextDB _context;
        public BagRepository(SaleContextDB saleContextDB)
        {
            _context = saleContextDB;
        }


        ///כל הסלים
        public async Task<IEnumerable<Bag>> GetAllBag()
        {
            return await _context.Bags.Include(b => b.Gift).ToListAsync();
        }

        //סל חדש- הוספה לסל
        public async Task<Bag> NewGiftToBag(Bag bag)
        {
            // 1. בדיקה שהמתנה קיימת
            var giftExists = await _context.Gifts.AnyAsync(g => g.Id == bag.IdGift);
            if (!giftExists) return null;

            // 2. חיפוש פריט קיים בסל של המשתמש
            var existingItem = await _context.Bags
                .FirstOrDefaultAsync(b => b.IdUser == bag.IdUser && b.IdGift == bag.IdGift);

            if (existingItem != null)
            {
                // עדכון כמות לפריט קיים
                existingItem.Quantity += bag.Quantity;
                _context.Bags.Update(existingItem);
                await _context.SaveChangesAsync();
                return await _context.Bags.Include(b => b.Gift).FirstOrDefaultAsync(b => b.Id == existingItem.Id);
            }
            else
            {
                // הוספת פריט חדש - יצירת אובייקט נקי כדי למנוע שגיאות ID
                var newEntry = new Bag
                {
                    IdUser = bag.IdUser,
                    IdGift = bag.IdGift,
                    Quantity = bag.Quantity
                };

                _context.Bags.Add(newEntry);
                await _context.SaveChangesAsync(); // כאן נוצר ה-ID האוטומטי ב-DB

                return await _context.Bags.Include(b => b.Gift).FirstOrDefaultAsync(b => b.Id == newEntry.Id);
            }
        }

        // מחיקת כל הסל של משתמש מסוים
        public async Task ClearUserBag(int userId)
        {
            var userItems = await _context.Bags.Where(b => b.IdUser == userId).ToListAsync();
            if (userItems.Any())
            {
                _context.Bags.RemoveRange(userItems);
                await _context.SaveChangesAsync();
            }
        }
        //מחיקה סל

        public async Task DeleteBag(int id)
        {
            var bag = await _context.Bags.FindAsync(id);
            if (bag != null)
            {
                _context.Bags.Remove(bag);
                await _context.SaveChangesAsync();

            }

        }


        // חיפוש לפי ID
        public async Task<Bag?> GetBagById(int id)
        {
            return await _context.Bags.Include(b => b.Gift)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);

        }

        //חיפוש לפי ID משתמש
        // חיפוש לפי ID משתמש - מעודכן עם Include כדי שתוכלי לראות את פרטי המתנה
        public async Task<IEnumerable<Bag?>> GetBagByUser(int id)
        {
            // הוספנו .Include(b => b.Gift) כדי שהאובייקט של המתנה יחזור יחד עם השורה בסל
            var b = await _context.Bags
                .Include(b => b.Gift)
                .Where(b => b.IdUser == id)
                .ToListAsync();

            return b;
        }

        //חיפוש לפי ID מתנה
        public async Task<IEnumerable<Bag?>> GetBagByGift(int id)
        {
            var b = await _context.Bags.Where(b => b.IdGift == id).ToArrayAsync();
            return b;
        }


            // מוצא את כל הפריטים בסלים שמכילים את המתנה הזו ומוחק אותם
        public async Task RemoveGiftFromAllBags(int giftId)
        {
            var itemsToRemove = _context.Bags.Where(b => b.IdGift == giftId);

            _context.Bags.RemoveRange(itemsToRemove);
            await _context.SaveChangesAsync();
        }


    }
}



//using Microsoft.EntityFrameworkCore;
//using SaleApi.Data;
//using SaleApi.Models;

//namespace SaleApi.Repositories
//{
//    public class BagRepository : IBagRepository
//    {

//        SaleContextDB _context;
//        public BagRepository(SaleContextDB saleContextDB)
//        {
//            _context = saleContextDB;
//        }


//        ///כל הסלים
//        public async Task<IEnumerable<Bag>> GetAllBag()
//        {
//            return await _context.Bags.Include(b=> b.Gift).ToListAsync();
//        }

//        //סל חדש- הוספה לסל
//        public async Task<Bag> NewGiftToBag(Bag bag)
//        {
//            _context.Bags.Add(bag);
//            await _context.SaveChangesAsync();
//            return bag;
//        }

//        //// מחיקת כל הסל של משתמש מסוים
//        //     public async Task ClearUserBag(int userId)
//        //{
//        //    var userItems = await _context.Bags.Where(b => b.IdUser == userId).ToListAsync();
//        //    if (userItems.Any())
//        //    {
//        //        _context.Bags.RemoveRange(userItems);
//        //        await _context.SaveChangesAsync();
//        //    }
//        //}
//        //מחיקה סל

//        public async Task DeleteBag(int id)
//        {
//            var bag = await _context.Bags.FindAsync(id);
//            if (bag != null)
//            {
//                _context.Bags.Remove(bag);
//                await _context.SaveChangesAsync();

//            }

//        }


//        // חיפוש לפי ID
//        public async Task<Bag?> GetBagById(int id)
//        {
//            return await _context.Bags.Include(b=>b.Gift)
//                .Include(b=>b.User)
//                .FirstOrDefaultAsync(b=>b.Id==id);

//        }

//        //חיפוש לפי ID משתמש
//        public async Task<IEnumerable<Bag?>> GetBagByUser(int id)
//        {
//            var b = await _context.Bags.Where(b => b.IdUser == id).ToListAsync();

//            return b;
//        }

//        //חיפוש לפי ID מתנה
//        public async Task<IEnumerable<Bag?>> GetBagByGift(int id)
//        {
//            var b = await _context.Bags.Where(b => b.IdGift == id).ToArrayAsync();
//            return b;
//        }


//    }
//}
