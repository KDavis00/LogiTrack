using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogiTrack.Models
{
    public class InventoryItem
    {
        [Key]
        public int ItemId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; }

        [Required]
        public string Location { get; set; } = string.Empty;

        // Foreign key to Order
        public int? OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order? Order { get; set; }
    }
}
