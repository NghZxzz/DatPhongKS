using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnDatPhongKS.Models
{
	public class OrderDetail
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal Price { get; set; }
        [NotMapped]
        public int Total => Convert.ToInt32(Price) * Quantity;

        [ForeignKey("OrderId")]
        [ValidateNever]
        public Order Order { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

	}
}
