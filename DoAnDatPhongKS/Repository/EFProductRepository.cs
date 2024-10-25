using DoAnWeb.Data;
using DoAnDatPhongKS.Models;
using Microsoft.EntityFrameworkCore;
namespace DoAnDatPhongKS.Repository
{
    public class EFProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public EFProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.Include(x => x.Category).ToListAsync();
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.Include(x => x.Category).SingleOrDefaultAsync(x => x.Id == id);
        }
		public async Task<IEnumerable<Product>> GetbyCategoryAsync(string cate)
		{
			return await _context.Products.Include(x => x.Category).Where(p=> p.Category.Name == cate).ToListAsync();
		}
		public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            product.Status = "Đã xóa";
            await _context.SaveChangesAsync();
        }
    }
}
