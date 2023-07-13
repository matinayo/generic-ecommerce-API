using AutoMapper;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Category;
using HalceraAPI.Models.Requests.Composition;
using HalceraAPI.Models.Requests.Composition.CompositionData;
using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Models.Requests.Price;
using HalceraAPI.Models.Requests.Product;

namespace HalceraAPI.Utilities.Automapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Category Request
            CreateMap<CreateCategoryRequest, Category>().ReverseMap();
            CreateMap<UpdateCategoryRequest, Category>()
                .ForMember(destination => destination.MediaCollection, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));
            CreateMap<Category, CategoryResponse>().ReverseMap();

            // Product Request
            CreateMap<CreateProductRequest, Product>().ReverseMap();
            CreateMap<Product, ProductResponse>().ReverseMap();

            // Price Request
            CreateMap<CreatePriceRequest, Price>().ReverseMap();
            CreateMap<Price, PriceResponse>().ReverseMap();

            // Composition Request
            CreateMap<CreateCompositionRequest, Composition>().ReverseMap();
            CreateMap<Composition, CompositionResponse>().ReverseMap();

            // Composition Data Request
            CreateMap<CreateCompositionDataRequest, CompositionData>().ReverseMap();
            CreateMap<CompositionData, CompositionDataResponse>().ReverseMap();

            // Product Category Request
            CreateMap<ProductCategoryRequest, Category>()
                .ForMember(destination => destination.Id, opt => opt.MapFrom(source => source.CategoryId))
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));

            // Media
            CreateMap<CreateMediaRequest, Media>().ReverseMap();
            CreateMap<UpdateMediaRequest, Media>()
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));
            CreateMap<Media, MediaResponse>().ReverseMap();
        }
    }
}
