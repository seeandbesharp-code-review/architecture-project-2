
using Microsoft.EntityFrameworkCore;

namespace SaleApi.Data
{
    public class SaleContextFactory
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=SaleDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;";        //private const string ConnectionString = "Server=DESKTOP-1L8084V\\SQLEXPRESS;DataBase=HomePoducts216234070;Integrated Security=SSPI;" +
        //   "Persist Security Info=False;TrustServerCertificate=true"; ---srv2\\pupils
        public static SaleContextDB CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<SaleContextDB>();
            optionsBuilder.UseSqlServer(ConnectionString);
            return new SaleContextDB(optionsBuilder.Options);
        }
    }
}
