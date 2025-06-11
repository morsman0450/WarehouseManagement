using System.ComponentModel.DataAnnotations;

namespace WarehouseManagement.Models
{
    public class Shelf
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Označení regálu je povinné")]
        [StringLength(20)]
        public string Code { get; set; }

        public string Description { get; set; }

        public int WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
    }
}
