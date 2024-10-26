using DoAnWeb.Data;
using DoAnDatPhongKS.Extensions;
using DoAnDatPhongKS.Models;
using DoAnDatPhongKS.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnDatPhongKS.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
	{
		private readonly IProductRepository _productRepository;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;


		public ShoppingCartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IProductRepository productRepository)
		{
			_productRepository = productRepository;
			_context = context;
			_userManager = userManager;

		}
		public async Task<IActionResult> AddToCart(int productId, int quantity,DateTime checkInDate, DateTime checkOutDate)
		{
			if (checkOutDate <= checkInDate)
			{
				TempData["Error"] = "Ngày Check Out phải sau ngày Check In!";
				return RedirectToAction("Detail", "Shop", new { id = productId });
			}
			// Giả sử bạn có phương thức lấy thông tin sản phẩm từ productId
			var product = await GetProductFromDatabase(productId);
			var cartItem = new CartItem
			{
				ProductId = productId,
				Name = product.Name,
				ImageUrl = product.ImageUrl,
				Price = product.Price,
				QuantityCustomer = product.QuatityCustomer,
				Quantity = quantity,
				CheckInDate = checkInDate,
				CheckOutDate = checkOutDate
			};
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
			cart.AddItem(cartItem);

			HttpContext.Session.SetObjectAsJson("Cart", cart);

			return RedirectToAction("Index");
		}
		public IActionResult Index()
		{
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
			return View(cart);
		}
		// Các actions khác...
		private async Task<Product> GetProductFromDatabase(int productId)
		{
			// Truy vấn cơ sở dữ liệu để lấy thông tin sản phẩm
			var product = await _productRepository.GetByIdAsync(productId);
			return product;
		}

		public IActionResult RemoveFromCart(int productId,DateTime checkindate, DateTime checkoutdate)
		{
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");

			if (cart is not null)
			{
				cart.RemoveItem(productId, checkindate, checkoutdate);

				// Lưu lại giỏ hàng vào Session sau khi đã xóa mục
				HttpContext.Session.SetObjectAsJson("Cart", cart);
			}

			return RedirectToAction("Index");
		}
		public IActionResult Checkout()
		{
			return View(new Order());
		}
		[HttpPost]
		public async Task<IActionResult> ApplyDiscount(string discountCode)
		{
			// Tìm mã giảm giá trong cơ sở dữ liệu
			var discount = await _context.DiscountCodes.FirstOrDefaultAsync(d => d.Code == discountCode && d.ExpiryDate >= DateTime.Now);

			if (discount == null)
			{
				TempData["Error"] = "Discount code không tồn tại hoặc đã hết hạn.";
				return RedirectToAction("Index");
			}

			// Lấy giỏ hàng từ Session
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
			if (cart == null)
			{
				TempData["Error"] = "Giỏ hàng bạn đang trống không.";
				return RedirectToAction("Index");
			}

			// Tính toán tổng tiền giảm giá và lưu lại trong Session
			cart.DiscountPercentage = discount.DiscountPercentage;
			HttpContext.Session.SetObjectAsJson("Cart", cart);

			return RedirectToAction("Index");
		}

		[HttpPost]
		public async Task<IActionResult> Checkout(Order order)
		{
			var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
			if (cart == null || !cart.Items.Any())
			{
				// Xử lý giỏ hàng trống...
				return RedirectToAction("Index");
			}

			var user = await _userManager.GetUserAsync(User);
			order.UserId = user.Id;
			order.OrderDate = DateTime.UtcNow;
			order.TotalPrice = cart.Items.Sum(i => i.Total);
			order.OrderDetails = cart.Items.Select(i => new OrderDetail
			{
				ProductId = i.ProductId,
				Quantity = i.Quantity,
				Price = i.Price,
			}).ToList();

			_context.Orders.Add(order);
			await _context.SaveChangesAsync();

			HttpContext.Session.Remove("Cart");

			return View("OrderCompleted", order.Id); // Trang xác nhận hoàn thành đơn hàng
		}

		public async Task<IActionResult> OrderCompleted(int id)
		{

			return View();
		}
	}
}
