using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class ProductTransactionRepository : IProductTransactionRepository
    {
        private List<ProductTransaction> _productTransaction = [];
        private readonly IProductRepository productRepository;
        private readonly IInventoryTransactionRepository inventoryTransactionRepository;
        private readonly IInventoryRepository inventoryRepository;

        public ProductTransactionRepository(
            IProductRepository productRepository, 
            IInventoryTransactionRepository inventoryTransactionRepository,
            IInventoryRepository inventoryRepository)
        {
            this.productRepository = productRepository;
            this.inventoryTransactionRepository = inventoryTransactionRepository;
            this.inventoryRepository = inventoryRepository;
        }
        public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
        {
            //decrease the inventories
            var prod = await this.productRepository.GetProductByIdAsync(product.ProductId);

            if (prod != null)
            {
                foreach(var pi in prod.ProductInventories)
                {
                    //adding inventory transaction
                    if(pi.Inventory is not null)
                    {
                        this.inventoryTransactionRepository.ProduceAsync(productionNumber, pi.Inventory,
                        pi.InventoryQuantity * quantity,
                        doneBy,
                        -1);

                        //decrease the inventories
                        var inv = await this.inventoryRepository.GetInvenryByIdAsync(pi.InventoryId);
                        inv.Quantity -= pi.InventoryQuantity * quantity;

                       await this.inventoryRepository.UpdateInventoryAsync(inv);
                    }
                    
                }
            }

            //add product transaction
            this._productTransaction.Add(new ProductTransaction
            {
                ProductionNumber = productionNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransactionType.ProduceProduct,
                QuantityAfter = product.Quantity + quantity,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
            });
            //add product transaction
        }

        public  Task SellProductAsync(string salesOrderNumber, Product product, int quantity,double unitPrice, string doneBy)
        {
            this._productTransaction.Add(new ProductTransaction
            {
                ActivityType = ProductTransactionType.SellProduct,
                SONumber = salesOrderNumber,   
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                QuantityAfter = product.Quantity - quantity,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                UnitPrice = unitPrice
            });

            return Task.CompletedTask;
        }
    }
}
