using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness
{
    public class Inventory
    {
        public int InventoryId { get; set; }

        [Required]
        [StringLength(150)]
        public string InventoryName { get; set; } = string.Empty;
        [Range(0,int.MaxValue, ErrorMessage = "Quantity must be a non-negative, greater or equal to 0 number.")]
        public int Quantity { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative, greater or equal to 0 number.")]
        public decimal Price { get; set; }
    }
}
