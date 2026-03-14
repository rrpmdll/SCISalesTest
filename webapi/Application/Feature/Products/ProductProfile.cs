using AutoMapper;
using SCISalesTest.Application.DTOs.Products;
using SCISalesTest.Application.Feature.Products.CreateProduct;
using SCISalesTest.Application.Feature.Products.UpdateProduct;
using SCISalesTest.Domain.Entities;

namespace SCISalesTest.Application.Feature.Products;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();

        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());

        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

        CreateMap<Product, ProductWithExchangeRateDto>()
            .ForMember(dest => dest.OriginalPrice, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.ConvertedPrice, opt => opt.Ignore())
            .ForMember(dest => dest.TargetCurrency, opt => opt.Ignore())
            .ForMember(dest => dest.ExchangeRate, opt => opt.Ignore())
            .ForMember(dest => dest.OriginalCurrency, opt => opt.Ignore());

        CreateMap<CreateProductCommand, CreateProductDto>();
        CreateMap<UpdateProductCommand, UpdateProductDto>();
    }
}
