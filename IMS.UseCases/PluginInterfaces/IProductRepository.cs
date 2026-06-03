using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int productId);
        Task<Product?> GetProductByIdAsync(int productId);
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        Task UpdateProductAsync(Product product);
       
    }
}
