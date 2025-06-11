using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WarehouseManagement.Models
{
    public class Item
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Název zboží je povinný")]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "Množství je povinné")]
        [Range(0, double.MaxValue, ErrorMessage = "Množství musí být kladné číslo")]
        public decimal Quantity { get; set; }

        public int? LocationId { get; set; }
        public virtual Location Location { get; set; }

        [NotMapped]
        public int? NewLocationId { get; set; }
        [NotMapped]
        public List<Location>? AvailableLocations { get; set; }
        [NotMapped]
        public int SelectedLocationId { get; set; }
    }
}
