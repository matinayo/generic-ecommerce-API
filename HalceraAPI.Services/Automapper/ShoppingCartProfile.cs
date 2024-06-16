using AutoMapper;
using HalceraAPI.Models;
using HalceraAPI.Services.Dtos.ShoppingCart;

namespace HalceraAPI.Services.Automapper
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<ShoppingCartRequest, ShoppingCart>()
                .ForMember(dest => dest.CompositionId, opt => opt.MapFrom(src => src.SelectedCompositionId))
                .ForMember(dest => dest.ProductSizeId, opt => opt.MapFrom(src => src.SelectedProductSizeId));
        }
    }
}
