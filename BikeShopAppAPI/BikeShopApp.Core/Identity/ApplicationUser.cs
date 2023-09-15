using BikeShopApp.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeShopApp.Core.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        //Other classes can refer to the Id as UserId to make it easier to read.
        [NotMapped]
        public int UserId { get { return Id; } set { Id = value; } }

        [StringLength(255)]
        public string Name { get; set; } = string.Empty;

        public int CartId { get; set; }

        [StringLength(500)]
        public string ImgPath { get; set; } = string.Empty;

        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;

        public DateTime RefreshTokenExpirationDateTime { get; set; }

        public Cart Cart { get; set; }

        public List<Order> Orders { get; set; }

        public List<Review> Reviews { get; set; }
    }
}
