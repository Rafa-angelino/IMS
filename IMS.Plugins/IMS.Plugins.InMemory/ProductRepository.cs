using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public ProductRepository()
        {
            _products =
            [
                new() { ProductId = 1, ProductName = "Bike1", Quantity = 10, Price = 100 },
                new() { ProductId = 2, ProductName = "Car", Quantity = 20, Price = 200 },
                new() { ProductId = 3, ProductName = "Skate", Quantity = 30, Price = 300 },
            ];
        }

        public Task AddProductAsync(Product product)
        {
            if (_products.Any(x => x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var maxId = _products.Count != 0 ? _products.Max(x => x.ProductId) : 0;

            product.ProductId = maxId + 1;

            _products.Add(product);
            return Task.CompletedTask;  
        }

        public Task DeleteProductAsync(int productId)
        {
            var product = _products.FirstOrDefault(i => i.ProductId == productId);
            if (product != null)
            {
                _products.Remove(product);
            }
            return Task.CompletedTask;
        }

        public async Task<Product?> GetInvenryByIdAsync(int productId)
        {
            return await Task.FromResult(_products.FirstOrDefault(i => i.ProductId == productId));   

        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return await Task.FromResult(_products);

            return _products.Where(i => i.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public Task UpdateProductAsync(Product product)
        {
            if(_products.Any(x => x.ProductId != product.ProductId &&
                x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var invToUpdate = _products.FirstOrDefault(x => x.ProductId == product.ProductId);

            if (invToUpdate != null)
            {
                invToUpdate.ProductName = product.ProductName;
                invToUpdate.Quantity = product.Quantity;
                invToUpdate.Price = product.Price;
            }
           return Task.CompletedTask;
        }
    }
}
