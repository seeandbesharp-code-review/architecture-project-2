using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleApi.Models
{
    public class Gift
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
        public string? Img { get; set; }

        [Required]
        public int Price { get; set; }

        [Required, ForeignKey(nameof(Doner))]
        public int IdDoner { get; set; }
        public Doner Doner { get; set; } = null!;

        [ForeignKey(nameof(Category))]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
