using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnDatPhongKS.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Mã sản phẩm")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên phòng là bắt buộc"), StringLength(100, ErrorMessage = "Tên phòng không được quá 100 ký tự")]
        [DisplayName("Tên Phòng")]
        public string Name { get; set; }
        [Range(100000.000, 100000000.000, ErrorMessage = "Giá sản phẩm phải nằm trong khoản từ 100000.000 đến 100000000.000")]
        [DisplayName("Giá bán")]
        public decimal Price { get; set; }

        [DisplayName("Số lượng phòng")]
        public int Quatity { get; set; }


		[DisplayName("Số lượng khách")]
		public int QuatityCustomer { get; set; }

		[DisplayName("Mô tả")]
        public string Description { get; set; }
        [DisplayName("Mô tả ngắn")]
        public string? ShortDescription { get; set; }

        [DisplayName("Hình ảnh")]
        public string? ImageUrl { get; set; }

        [DisplayName("Trạng thái")]
        public string? Status { get; set; }

        [ForeignKey("Category")]
        [DisplayName("Mã danh mục")]
        public int CategoryId { get; set; }
        [DisplayName("Danh mục")]
        public Category? Category { get; set; }
    }
}
