using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Models
{
    public class Warehouse
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();
    }
}
