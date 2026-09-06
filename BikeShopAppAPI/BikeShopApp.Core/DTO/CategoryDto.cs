using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Core.DTO
{
    public class CategoryDto
    {
        public int? CategoryId { get; set; } = null;

        [StringLength(255, ErrorMessage = "Name is too long")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
    }
}
