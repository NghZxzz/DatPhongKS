using DoAnWeb.Data;
using DoAnDatPhongKS.Models;
using DoAnDatPhongKS.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DoAnDatPhongKS.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = SD.Role_Admin)]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {

        
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ApplicationDbContext _context;

        public HomeAdminController(IProductRepository productRepository, ICategoryRepository categoryRepository, ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _context = context;
        }
            [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("quanlysp")]
        public async Task<IActionResult> QuanLySP(string SearchString = "")
        {
            if (SearchString != null)
            {
                var product = await _context.Products.Include(x => x.Category).Where(x=> x.Name.ToUpper().Contains(SearchString.ToUpper())).ToListAsync();
                return View(product);
            }
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }
        [Route("createsp")]
        [HttpGet]
        public async Task<IActionResult> CreateSP()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePart = Path.Combine("wwwroot/images", image.FileName);
            using (var fileStream = new FileStream(savePart, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName;
        }

        [Route("createsp")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSP(Product product, IFormFile ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null)
                {
                    product.ImageUrl = await SaveImage(ImageUrl);
                }
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(QuanLySP));
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }
        [Route("createcate")]
        public async Task<IActionResult> CreateCategory()
        {
            return View();
        }
        [Route("createcate")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (category.Name == null)
            {
                return View(category);
            }
            await _categoryRepository.AddAsync(category);
            return RedirectToAction(nameof(QuanLySP));
        }
        [Route("detailsproduct")]
        public async Task<IActionResult> DetailsProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Route("Editproduct")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        [Route("Editproduct")]
        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    product.ImageUrl = await SaveImage(imageUrl);
                }
                await _productRepository.UpdateAsync(product);
                return RedirectToAction(nameof(QuanLySP));
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        [Route("quanlydonhang")]
        public async Task<IActionResult> QuanLyDH()
        {

            var order = await _context.Orders.Include(x => x.ApplicationUser).ToListAsync();
            return View(order);
        }

        [Route("detailsdonhang")]
        public async Task<IActionResult> DetailsDH(int id)
        {
            var order = await _context.OrderDetails.Include(x => x.Order).Include(x => x.Product).Where(x => x.OrderId == id).ToListAsync();
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [Route("deletesp")]
        public async Task<IActionResult> DeleteSP(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [Route("deletesp")]
        [HttpPost, ActionName("Delete")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(QuanLySP));
        }

        [Route("doanhthu")]
        public async Task<IActionResult> DoanhThu()
        {
            ViewBag.Tongthunhap =  _context.Orders.Sum(x=> x.TotalPrice);
            ViewBag.SoHoaDon= _context.Orders.Count();
            return View();
        }
        [Route("quanlydiscount")]
        public async Task<IActionResult> QuanLyDC()
		{
			var discountCodes = await _context.DiscountCodes.ToListAsync();
			return View(discountCodes);
		}
        [Route("createdc")]
        // Tạo mã giảm giá mới
        public IActionResult CreateDC()
		{
			return View();
		}
        [Route("createdc")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateDC(DiscountCode discountCode)
		{
			if (ModelState.IsValid)
			{
				_context.DiscountCodes.Add(discountCode);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(QuanLyDC));
			}
			return View(discountCode);
		}
        [Route("editdc")]
        // Sửa mã giảm giá
        public async Task<IActionResult> EditDC(int id)
		{
			var discountCode = await _context.DiscountCodes.FindAsync(id);
			if (discountCode == null) return NotFound();
			return View(discountCode);
		}
        [Route("editdc")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditDC(int id, DiscountCode discountCode)
		{
			if (id != discountCode.Id) return NotFound();

			if (ModelState.IsValid)
			{
				_context.Update(discountCode);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(QuanLyDC));
			}
			return View(discountCode);
		}
        [Route("deletedc")]
        // Xóa mã giảm giá
        public async Task<IActionResult> DeleteDC(int id)
		{
			var discountCode = await _context.DiscountCodes.FindAsync(id);
			if (discountCode == null) return NotFound();
			return View(discountCode);
		}

		[HttpPost, ActionName("DeleteDC")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteDCConfirmed(int id)
		{
			var discountCode = await _context.DiscountCodes.FindAsync(id);
			_context.DiscountCodes.Remove(discountCode);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(QuanLyDC));
		}
	}
}
