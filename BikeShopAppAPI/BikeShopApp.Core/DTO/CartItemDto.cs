using System.ComponentModel.DataAnnotations;

namespace BikeShopApp.Core.DTO
{
    /// <summary>
    /// Used for when a product is added to a cart
    /// </summary>
    public class CartItemDto
    {
        [Required(ErrorMessage = "CartId is required")]
        public int? CartId { get; set; } = null;

        [Required(ErrorMessage = "ProductId is required")]
        public int? ProductId { get; set; } = null;
    }
}
