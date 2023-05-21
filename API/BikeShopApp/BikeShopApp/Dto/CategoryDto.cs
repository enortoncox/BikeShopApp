using BikeShopApp.Models;
using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Dto
{
    public class CategoryDto
    {
        public int? CategoryId { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; } = string.Empty;
    }
}
