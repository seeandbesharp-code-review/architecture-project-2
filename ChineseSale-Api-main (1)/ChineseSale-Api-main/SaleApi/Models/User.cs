using System.ComponentModel.DataAnnotations;

namespace SaleApi.Models
{
    public enum UserRole
    {
        Admin,
        User,
    }

    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required , EmailAddress]
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; } = string.Empty;
        [Required]
        public UserRole Role { get; set; } = UserRole.User;

    }
}
