using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly List<Inventory> _inventories;

        public InventoryRepository()
        {
            _inventories =
            [
                new() { InventoryId = 1, InventoryName = "Bike Seat", Quantity = 10, Price = 100 },
                new() { InventoryId = 2, InventoryName = "Bike Body", Quantity = 20, Price = 200 },
                new() { InventoryId = 3, InventoryName = "Bike Whells", Quantity = 30, Price = 300 },
                new() { InventoryId = 4, InventoryName = "Bike Pedels", Quantity = 30, Price = 300 },
            ];
        }

        public Task AddInventoryAsync(Inventory inventory)
        {
            if (_inventories.Any(x => x.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var maxId = _inventories.Count != 0 ? _inventories.Max(x => x.InventoryId) : 0;

            inventory.InventoryId = maxId + 1;  

            _inventories.Add(inventory);
            return Task.CompletedTask;  
        }

        public Task DeleteInventoryAsync(int inventoryId)
        {
            var inventory = _inventories.FirstOrDefault(i => i.InventoryId == inventoryId);
            if (inventory != null)
            {
                _inventories.Remove(inventory);
            }
            return Task.CompletedTask;
        }

        public async Task<Inventory?> GetInvenryByIdAsync(int inventoryId)
        {
            var inv = _inventories.First(x => x.InventoryId == inventoryId);
            var newInv = new Inventory
            {
                InventoryId = inv.InventoryId,
                InventoryName = inv.InventoryName,
                Quantity = inv.Quantity,
                Price = inv.Price
            };

            return await Task.FromResult(newInv);
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return await Task.FromResult(_inventories);

            return _inventories.Where(i => i.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public Task UpdateInventoryAsync(Inventory inventory)
        {
            if(_inventories.Any(x => x.InventoryId != inventory.InventoryId &&
                x.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return Task.CompletedTask;
            }

            var invToUpdate = _inventories.FirstOrDefault(x => x.InventoryId == inventory.InventoryId);

            if (invToUpdate != null)
            {
                invToUpdate.InventoryName = inventory.InventoryName;
                invToUpdate.Quantity = inventory.Quantity;
                invToUpdate.Price = inventory.Price;
            }
           return Task.CompletedTask;
        }
    }
}
