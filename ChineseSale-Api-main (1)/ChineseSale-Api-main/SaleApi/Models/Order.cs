using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleApi.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required, ForeignKey("User")]
        public int IdUser { get; set; }

        public User User { get; set; }

        [Required, ForeignKey("Gift")]
        public int IdGift { get; set; }

        public Gift Gift { get; set; }

        public bool Win { get; set; }
        public int OrderGroupId { get; set; }

    }
}
