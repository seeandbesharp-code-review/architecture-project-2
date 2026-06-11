using Microsoft.EntityFrameworkCore;
using SaleApi.Models;
using System.Collections.Generic;

namespace SaleApi.Data
{
    public class SaleContextDB: DbContext
    {

        public SaleContextDB(DbContextOptions<SaleContextDB> options) : base(options) { }
        public DbSet<Bag> Bags { get; set; }
        public DbSet<Doner> Doners { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Winner> Winners { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("SeServer=DESKTOP-1PUSNIL;DataBase=SaleDB6679;Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=True;");
            }
        }

        internal object GetGiftById(int id)
        {
            throw new NotImplementedException();
        }
    }
}

