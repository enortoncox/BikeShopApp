using AutoMapper;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.Identity;
using BikeShopApp.Core.Models;

namespace BikeShopApp.WebAPI
{
    public class MapperProfiles : Profile
    {
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
