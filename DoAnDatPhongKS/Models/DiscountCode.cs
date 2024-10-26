using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DoAnDatPhongKS.Models
{
    public class DiscountCode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; } // Mã giảm giá
        public decimal DiscountPercentage { get; set; } // % giảm giá
        public DateTime ExpiryDate { get; set; }
    }
}
