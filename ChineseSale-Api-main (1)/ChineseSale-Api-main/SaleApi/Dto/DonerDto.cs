using SaleApi.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static SaleApi.Dto.GiftDto;

namespace SaleApi.Dto
{
    public class DonerDto
    {
        public class CreateDonerDto
        {
            [Required]
            [MaxLength(100)]
            public string FirstName { get; set; }
            [Required]
            [MaxLength(100)]
            public string LastName { get; set; }
            [Required]
            [EmailAddress]
            [MaxLength(200)]
            [JsonPropertyName("eMail")]
            public string EMail { get; set; }
        }

        public  class NewGiftFromDoner
        {
            public Gift DonerGift { get; set; }
        }

        public class DeleteDonerDto
        {
            [Required]
            public int Id { get; set; }
        }

        public class UpdateDonerDto
        {
            [Required]
            public int Id { get; set; }
            [MaxLength(100)]
            public string? FirstName { get; set; } = null;
            [MaxLength(100)]
            public string? LastName { get; set; } = null;
            [EmailAddress]
            [MaxLength(100)]
            [JsonPropertyName("eMail")]
            public string? EMail { get; set; } = null;
        }

        public class GiftDto
        {
            [Required]
            public int Id { get; set; }
            [Required]
            [MaxLength(150)]
            public string Name { get; set; }
            [MaxLength(500)]
            public string? Description { get; set; }
            [MaxLength(1000)]
            public string? Img { get; set; }
            [Required]
            public int Price { get; set; }
            [Required]
            [ForeignKey(nameof(Doner))]
            public int IdDoner { get; set; }

        }
        public class GetDonerDtoWithGift
        {
            [Required]
            public int Id { get; set; }
            [MaxLength(100)]
            public string? FirstName { get; set; } = null;
            [MaxLength(100)]
            public string? LastName { get; set; } = null;
            [EmailAddress]
            [MaxLength(200)]
            public string? EMail { get; set; } = null;

           public ICollection<GetShortGiftDto> Gifts { get; set; } = new List<GetShortGiftDto>();
        }
    }
}
