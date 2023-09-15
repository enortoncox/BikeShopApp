using System.ComponentModel.DataAnnotations.Schema;

namespace BikeShopApp.Core.Models
{
    public class OrdersProducts
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
