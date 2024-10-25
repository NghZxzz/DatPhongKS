using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnDatPhongKS.Models
{
	public class CartItem
	{
		public int ProductId { get; set; }
		public string Name { get; set; }

		public string ImageUrl { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
		public int QuantityCustomer { get; set; }

		[NotMapped]
		public int TotalDays => (CheckOutDate - CheckInDate).Days;
		[NotMapped]
        public int Total => Convert.ToInt32(Price) * Quantity * TotalDays;

	}
}
