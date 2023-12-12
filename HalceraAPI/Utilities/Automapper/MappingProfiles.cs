using AutoMapper;
using Elfie.Serialization;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.ApplicationUser;
using HalceraAPI.Models.Requests.BaseAddress;
using HalceraAPI.Models.Requests.Category;
using HalceraAPI.Models.Requests.Composition;
using HalceraAPI.Models.Requests.Composition.CompositionData;
using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Models.Requests.OrderHeader;
using HalceraAPI.Models.Requests.OrderHeader.CustomerResponse;
using HalceraAPI.Models.Requests.OrderHeader.CustomerResponse.CustomerOrderDetails;
using HalceraAPI.Models.Requests.OrderHeader.CustomerResponse.PurchaseDetails;
using HalceraAPI.Models.Requests.PaymentDetails;
using HalceraAPI.Models.Requests.Price;
using HalceraAPI.Models.Requests.Product;
using HalceraAPI.Models.Requests.RefreshToken;
using HalceraAPI.Models.Requests.Role;
using HalceraAPI.Models.Requests.Shipping;
using HalceraAPI.Models.Requests.ShoppingCart;

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
            CreateMap<Category, CategoryLabelResponse>().ReverseMap();

            // Product Request
            CreateMap<CreateProductRequest, Product>()
                .ForMember(destination => destination.Categories, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<Product, ProductDetailsResponse>().ReverseMap();
            CreateMap<UpdateProductRequest, Product>()
                .ForMember(destination => destination.MediaCollection, opt => opt.Ignore())
                .ForMember(destination => destination.Prices, opt => opt.Ignore())
                .ForMember(destination => destination.ProductCompositions, opt => opt.Ignore())
                .ForMember(destination => destination.Categories, opt => opt.Ignore())
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
                .ForMember(destination => destination.CompositionDataCollection, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));
            CreateMap<Composition, CompositionResponse>().ReverseMap();

            // Composition Data Request
            CreateMap<CreateCompositionDataRequest, CompositionData>().ReverseMap();
            CreateMap<UpdateCompositionDataRequest, CompositionData>()
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));
            CreateMap<CompositionData, CompositionDataResponse>().ReverseMap();

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

            // Shopping Cart
            CreateMap<ShoppingCart, ShoppingCartDetailsResponse>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartResponse>().ReverseMap();

            // Application User
            CreateMap<RegisterRequest, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, UserResponse>().ReverseMap();
            CreateMap<ApplicationUser, CustomerDetailsResponse>().ReverseMap();

            // Refresh Token
            CreateMap<RefreshToken, RefreshTokenResponse>().ReverseMap();

            // Roles
            CreateMap<RoleRequest, Roles>().ReverseMap();
            CreateMap<Roles, RoleResponse>().ReverseMap();

            // Payment Details
            CreateMap<PaymentDetailsRequest, PaymentDetails>().ReverseMap();
            CreateMap<PaymentDetails, PaymentDetailsResponse>().ReverseMap();

            // Shipping Address
            CreateMap<AddressRequest, BaseAddress>().ReverseMap();
            CreateMap<ShippingDetails, ShippingDetailsResponse>().ReverseMap();
            CreateMap<BaseAddress, ShippingAddressResponse>().ReverseMap();
            CreateMap<UpdateShippingAddressRequest, BaseAddress>()
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));
            CreateMap<UpdateShippingDetailsRequest, ShippingDetails>()
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));

            // Order Header
            CreateMap<OrderHeader, CheckoutResponse>().ReverseMap();
            CreateMap<OrderHeader, OrderResponse>().ReverseMap();
            CreateMap<OrderDetails, OrderDetailsResponse>().ReverseMap();
            CreateMap<Product, ProductSummaryResponse>();

            // Order Purchase
            CreateMap<PurchaseDetails, PurchaseDetailsSummaryResponse>().ReverseMap();
        }
    }
}
