using DoAnDatPhongKS.Models;
using DoAnDatPhongKS.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
namespace DoAnDatPhongKS.Controllers
{
	public class ShopController : Controller
	{
		private readonly IProductRepository _productRepository;
		private readonly ICategoryRepository _categoryRepository;

		public ShopController(IProductRepository productRepository, ICategoryRepository categoryRepository)
		{
			_productRepository = productRepository;
			_categoryRepository = categoryRepository;
		}
		public async Task<IActionResult> Index(int? categoryId)
		{
			var product = (await _productRepository.GetAllAsync()).ToList();
			var categories = (await _categoryRepository.GetAllAsync()).ToList();

            if (categoryId.HasValue)
            {
                product = product.Where(p => p.CategoryId == categoryId.Value).ToList(); 
            }
            var viewModel = new CategoryProductViewModel
            {
                Categories = categories,
                Products = product,
				SelectedCategoryId = categoryId ?? categories.FirstOrDefault()?.Id
			};

            return View(viewModel);
		}

		public async Task<IActionResult> Detail(int id)
		{
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
	}
}
