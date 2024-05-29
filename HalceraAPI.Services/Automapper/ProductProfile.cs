using AutoMapper;
using HalceraAPI.Models;
using HalceraAPI.Services.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Automapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDetailsResponse>().ReverseMap();

            CreateMap<Product, ProductResponse>().ReverseMap();

            CreateMap<CreateProductRequest, Product>()
                .ForMember(destination => destination.Categories, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UpdateProductRequest, Product>()
                .ForMember(destination => destination.Active, opt => 
                    opt.MapFrom((src, dest) => src.Active == null ? dest.Active : src.Active))
                .ForMember(destination => destination.Compositions, opt => opt.Ignore())
                .ForMember(destination => destination.MaterialsAndDetails, opt => opt.Ignore())
                .ForMember(destination => destination.Categories, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));
        }
    }
}
