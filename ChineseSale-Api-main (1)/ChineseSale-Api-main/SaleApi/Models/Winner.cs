using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleApi.Models
{
    public class Winner
    {
        public int Id { get; set; }

         [ForeignKey("User")]
        public int IdUser { get; set; }
        public User User { get; set; } = null!;

        [ForeignKey("Gift")]

        public int IdGift { get; set; }
        public Gift Gift { get; set; } = null!;
    }
}
