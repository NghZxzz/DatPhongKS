namespace DoAnDatPhongKS.Models
{
	public class ShoppingCart
	{
		public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal DiscountPercentage { get; set; } = 0;

        public decimal Subtotal => Items.Sum(i => i.Total);
        public decimal Discount => Subtotal * DiscountPercentage / 100;
        public decimal Total => Subtotal - Discount;

        public void AddItem(CartItem item)
		{
			var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId && i.CheckInDate == item.CheckInDate && i.CheckOutDate == item.CheckOutDate);
			if (existingItem != null )
			{
				existingItem.Quantity += item.Quantity;
			}
			else
			{
				Items.Add(item);
			}
		}
		public void RemoveItem(int productId,DateTime checkindate, DateTime checkoutdate)
		{
			Items.RemoveAll(i => i.ProductId == productId && i.CheckInDate == checkindate && i.CheckOutDate == checkoutdate);
		}

	}
}
