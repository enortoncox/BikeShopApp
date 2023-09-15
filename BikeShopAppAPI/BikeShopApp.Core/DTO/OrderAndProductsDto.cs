using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Core.DTO
{
    public class OrderAndProductsDto
    {
        [Required(ErrorMessage = "Order is required")]
        public OrderDto? Order { get; set; } = null;

        [Required(ErrorMessage = "ProductIds is required")]
        public ICollection<int>? ProductsIds { get; set; } = null;
    }
}
