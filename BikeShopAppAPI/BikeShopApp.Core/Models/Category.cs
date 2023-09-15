using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Core.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; } = string.Empty;

        public List<Product> Products { get; set; }
    }
}
