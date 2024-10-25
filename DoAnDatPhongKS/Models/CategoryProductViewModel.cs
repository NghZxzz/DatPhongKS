namespace DoAnDatPhongKS.Models
{
    public class CategoryProductViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
		public int? SelectedCategoryId { get; set; }
	}
}
