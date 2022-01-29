using AutoMapper;
using ProductShop.Datasets.Dtos.InputModels;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserInputDto, User>();
            CreateMap<ProductInputDto, Product>();
            CreateMap<CategoryInputDto, Category>();
            CreateMap<CategoryProductDto, CategoryProduct>();
        }
    }
}
