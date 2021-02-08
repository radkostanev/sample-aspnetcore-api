using AutoMapper;
using SampleNetCoreApi.Services.DTOs;
using SampleNetCoreApi.Data.Entities;

namespace SampleNetCoreApi.Api.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Product, ProductCreateDTO>().ReverseMap();
            CreateMap<Product, ProductUpdateDTO>().ReverseMap();
        }
    }
}
