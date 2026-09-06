using AutoMapper;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.Identity;
using BikeShopApp.Core.Models;

namespace BikeShopApp.WebAPI
{
    /// <summary>
    /// Defines AutoMapper mappings used by the application.
    /// </summary>
    public class MapperProfiles : Profile
    {
        /// <summary>
        /// Creates the application's AutoMapper mappings.
        /// </summary>
        public MapperProfiles()
        {
            CreateMap<ApplicationUser, UserDto>().ReverseMap();
            CreateMap<ApplicationUser, UserRegisterDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Review, ReviewDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Cart, CartDto>().ReverseMap();
        }
    }
}
