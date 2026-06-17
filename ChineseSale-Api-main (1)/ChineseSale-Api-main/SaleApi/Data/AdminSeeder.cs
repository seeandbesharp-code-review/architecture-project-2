using Microsoft.EntityFrameworkCore;
using SaleApi.Models;

namespace SaleApi.Data;

public static class AdminSeeder
{
    private const string AdminEmail = "admin@chinesesale.com";
    private const string AdminPassword = "Admin123!";

    public static async Task SeedAsync(SaleContextDB context, bool promoteAllUsersInDev = false)
    {
        var passwordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(AdminPassword));

        var admin = await context.Users.FirstOrDefaultAsync(u => u.Email == AdminEmail);
        if (admin == null)
        {
            context.Users.Add(new User
            {
                FirstName = "מנהל",
                LastName = "מערכת",
                Email = AdminEmail,
                Password = passwordHash,
                Role = UserRole.Admin
            });
        }
        else
        {
            admin.Role = UserRole.Admin;
            admin.Password = passwordHash;
        }

        if (promoteAllUsersInDev)
        {
            var users = await context.Users.Take(100).ToListAsync();
            foreach (var user in users)
            {
                user.Role = UserRole.Admin;
            }
        }

        await context.SaveChangesAsync();
    }
}
