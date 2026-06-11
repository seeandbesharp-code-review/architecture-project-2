using SaleApi.Models;

namespace SaleApi.Services
{
    public interface ITokenService
    {
        string GenerateToken(int userId, string email, string firstName, string lastName, UserRole role);
    }
}