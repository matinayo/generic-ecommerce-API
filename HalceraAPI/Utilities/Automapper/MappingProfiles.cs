﻿using AutoMapper;
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
                .ForMember(destination => destination.MediaCollection, opt => opt.Ignore()).ReverseMap()
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));
            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<Category, CategoryLabelResponse>().ReverseMap();

            // Product Request
            CreateMap<CreateProductRequest, Product>().ReverseMap();
            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<Product, ProductDetailsResponse>().ReverseMap();
            CreateMap<UpdateProductRequest, Product>()
                .ForMember(destination => destination.MediaCollection, opt => opt.Ignore())
                .ForMember(destination => destination.Prices, opt => opt.Ignore())
                .ForMember(destination => destination.ProductCompositions, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));

            // Price Request
            CreateMap<CreatePriceRequest, Price>().ReverseMap();
            CreateMap<UpdatePriceRequest, Price>()
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));
            CreateMap<Price, PriceResponse>().ReverseMap();

            // Composition Request
            CreateMap<CreateCompositionRequest, Composition>().ReverseMap();
            CreateMap<UpdateCompositionRequest, Composition>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));
            CreateMap<Composition, CompositionResponse>().ReverseMap();

            // Composition Data Request
            CreateMap<CreateCompositionDataRequest, CompositionData>().ReverseMap();
            CreateMap<UpdateCompositionDataRequest, CompositionData>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));
            CreateMap<CompositionData, CompositionDataResponse>().ReverseMap();

            // Product Category Request
            CreateMap<ProductCategoryRequest, Category>()
                .ForMember(destination => destination.Id, opt => opt.MapFrom(source => source.CategoryId)).ReverseMap()
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));

            // Media
            CreateMap<CreateMediaRequest, Media>().ReverseMap();
            CreateMap<UpdateMediaRequest, Media>()
                .ForMember(destination => destination.Type, opt =>
                {
                    opt.PreCondition(source =>
                    {
                        if (source.Type != null)
                            return true;
                        return false;
                    });
                })
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember != null));
            CreateMap<Media, MediaResponse>().ReverseMap();
        }
    }
}
