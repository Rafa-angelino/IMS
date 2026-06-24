using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products;
        private readonly IInventoryRepository inventoryRepository;

        public ProductRepository(IInventoryRepository inventoryRepository)
        {
            _products =
            [
                new() { ProductId = 1, ProductName = "Bike1", Quantity = 10, Price = 100 },
                new() { ProductId = 2, ProductName = "Car", Quantity = 20, Price = 200 },
                new() { ProductId = 3, ProductName = "Skate", Quantity = 30, Price = 300 },
            ];
            this.inventoryRepository = inventoryRepository;
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

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var prod = _products.FirstOrDefault(i => i.ProductId == productId);

            Product? newProd = null;
            if (prod != null)
            {
                newProd = new Product();
                newProd.ProductId = prod.ProductId;
                newProd.ProductName = prod.ProductName;
                newProd.Quantity = prod.Quantity;
                newProd.Price = prod.Price;
                newProd.ProductInventories = [];
                if (prod.ProductInventories != null && prod.ProductInventories.Count > 0)
                {
                    foreach (var prodInv in prod.ProductInventories)
                    {
                        var newProdInv = new ProductInventory
                        {
                            ProductId = prodInv.ProductId,
                            Product = prod,
                            InventoryId = prodInv.InventoryId,
                            Inventory = new(),
                            InventoryQuantity = prodInv.InventoryQuantity
                        };

                        if(prodInv.Inventory  is not null)
                        {
                            var inv = await inventoryRepository.GetInvenryByIdAsync(prodInv.Inventory.InventoryId);

                            if (inv is not null)
                            {
                                newProdInv.Inventory.InventoryId = inv.InventoryId;
                                newProdInv.Inventory.InventoryName = inv.InventoryName;
                                newProdInv.Inventory.Price = inv.Price;
                                newProdInv.Inventory.Quantity = inv.Quantity;
                            }

                           
                        }
                        newProd.ProductInventories.Add(newProdInv);
                    }
                }
            }
            return await Task.FromResult(newProd);
        }

        public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return await Task.FromResult(_products);

            return _products.Where(i => i.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task UpdateProductAsync(Product product)
        {
            if(_products.Any(x => x.ProductId != product.ProductId &&
                x.ProductName.ToLower() == product.ProductName.ToLower()))
            {
                return;
            }
            

            var prodToUpdate = _products.FirstOrDefault(x => x.ProductId == product.ProductId);

            if (prodToUpdate != null)
            {
                prodToUpdate.ProductName = product.ProductName;
                prodToUpdate.Quantity = product.Quantity;
                prodToUpdate.Price = product.Price;
                prodToUpdate.ProductInventories = product.ProductInventories;   
            }
            return;
        }
    }
}
