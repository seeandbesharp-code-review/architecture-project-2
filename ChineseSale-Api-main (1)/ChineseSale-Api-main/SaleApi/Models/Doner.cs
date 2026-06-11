using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleApi.Models
{
    public class Doner
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required,EmailAddress]
        public string Email { get; set; }

        public ICollection<Gift> Gifts { get; set; } = new List<Gift>();
    }
}