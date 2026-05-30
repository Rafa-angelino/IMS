using System.Text.Json.Serialization;

namespace IMS.CoreBusiness
{
    //this class will be a association class between Product and Inventory. Eventually we will have a many to many relationship between Product and Inventory, and this class will be the junction table for that relationship. 
    public class ProductInventory
    {
        public int ProductId { get; set; }
        [JsonIgnore] //to prevent circular reference when serializing to json
        public Product? Product { get; set; } //navigation property for ef core 
        public int InventoryId { get; set; }
        [JsonIgnore] //to prevent circular reference when serializing to json
        public Inventory? Inventory { get; set; } //navigation property for ef core 
        public int InventoryQuantity { get; set; }
    }
}
