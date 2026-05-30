using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative, greater or equal to 0 number.")]
        public int Quantity { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative, greater or equal to 0 number.")]
        public decimal Price { get; set; }

        public List<ProductInventory> ProductInventories { get; set; } = []; //one product can have many inventories

        public void AddInventory(Inventory inventory)
        {
            if(!this.ProductInventories.Any(
                x => x.Inventory is not null &&
                x.Inventory.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
            {
                this.ProductInventories.Add(new ProductInventory
                {
                    InventoryId = inventory.InventoryId,
                    Inventory = inventory,
                    InventoryQuantity = 1,
                    ProductId = this.ProductId,
                    Product = this
                });
            }
            
        }

        public void RemoveInventory(ProductInventory prodInv)
        {
            this.ProductInventories.Remove(prodInv);
        }
    }
}
