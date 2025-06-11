using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace WarehouseManagement.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kód lokace je povinný")]
        [StringLength(20)]
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int ShelfId { get; set; }
        public virtual Shelf Shelf { get; set; }

        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
    }
}
