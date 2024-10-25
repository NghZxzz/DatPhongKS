using DoAnDatPhongKS.Models;

namespace DoAnDatPhongKS.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);

		Task<IEnumerable<Product>> GetbyCategoryAsync(string name);

        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(int id);
    }
}
