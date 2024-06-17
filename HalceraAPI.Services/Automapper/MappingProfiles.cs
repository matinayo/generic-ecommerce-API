using AutoMapper;
using HalceraAPI.Models;
using HalceraAPI.Common.Enums;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.ApplicationUser;
using HalceraAPI.Services.Dtos.BaseAddress;
using HalceraAPI.Services.Dtos.Category;
using HalceraAPI.Services.Dtos.Composition;
using HalceraAPI.Services.Dtos.Composition.ComponentData;
using HalceraAPI.Services.Dtos.Media;
using HalceraAPI.Services.Dtos.OrderHeader;
using HalceraAPI.Services.Dtos.OrderHeader.CustomerResponse;
using HalceraAPI.Services.Dtos.OrderHeader.CustomerResponse.CustomerOrderDetails;
using HalceraAPI.Services.Dtos.OrderHeader.CustomerResponse.PurchaseDetails;
using HalceraAPI.Services.Dtos.Payment;
using HalceraAPI.Services.Dtos.PaymentDetails;
using HalceraAPI.Services.Dtos.Price;
using HalceraAPI.Services.Dtos.Product;
using HalceraAPI.Services.Dtos.RefreshToken;
using HalceraAPI.Services.Dtos.Role;
using HalceraAPI.Services.Dtos.Shipping;
using HalceraAPI.Services.Dtos.ShoppingCart;
using PayStack.Net;

namespace HalceraAPI.Services.Automapper
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
            
            //
            //CreateMap<UpdateProductRequest, Product>()
            //    //.ForMember(destination => destination.MediaCollection, opt => opt.Ignore())
            //    //.ForMember(destination => destination.Prices, opt => opt.Ignore())
            //    //.ForMember(destination => destination.ProductCompositions, opt => opt.Ignore())
            //    .ForMember(destination => destination.Categories, opt => opt.Ignore())
            //    .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));

            // Price Request
            CreateMap<CreatePriceRequest, Price>().ReverseMap();
            CreateMap<UpdatePriceRequest, Price>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));
            CreateMap<Price, PriceResponse>().ReverseMap();

            // Composition Data Request
            CreateMap<CreateComponentDataRequest, ComponentData>().ReverseMap();
            CreateMap<UpdateComponentDataRequest, ComponentData>()
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));
            CreateMap<ComponentData, ComponentDataResponse>().ReverseMap();

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
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember != null));
            CreateMap<Media, MediaResponse>().ReverseMap();


            // Application User
            CreateMap<RegisterRequest, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, UserAuthResponse>().ReverseMap();
            CreateMap<ApplicationUser, UserDetailsResponse>().ReverseMap();
            CreateMap<ApplicationUser, UserResponse>().ReverseMap();
            CreateMap<ApplicationUser, CustomerDetailsResponse>().ReverseMap();
            CreateMap<UpdateUserRequest, ApplicationUser>()
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember != null));

            // Refresh Token
            CreateMap<RefreshToken, RefreshTokenResponse>().ReverseMap();

            // Roles
            CreateMap<RoleRequest, Roles>().ReverseMap();
            CreateMap<Roles, RoleResponse>().ReverseMap();

            // Payment Details
            CreateMap<PaymentDetailsRequest, PaymentDetails>().ReverseMap();
            CreateMap<PaymentDetails, PaymentDetailsResponse>().ReverseMap();
            CreateMap<TransactionInitializeResponse, APIResponse<InitializePaymentResponse>>();
            CreateMap<TransactionInitialize.Data, InitializePaymentResponse>();
            CreateMap<TransactionVerifyResponse, APIResponse<TransactionVerifyResponse>>();
            CreateMap<TransactionVerify.Data, VerifyPaymentResponse>()
                .ForMember(destination => destination.PaymentProvider, opts => opts.MapFrom(src => PaymentProvider.Paystack))
                .ForMember(destination => destination.TransactionId, opts => opts.MapFrom(src => src.Reference))
                .ForMember(destination => destination.AmountPaid, opts => opts.MapFrom(src => src.Amount));

            // Address
            CreateMap<AddressRequest, BaseAddress>()
                .ForAllMembers(opts => opts.Condition((source, destination, srcMmber) => srcMmber is not null));
            CreateMap<ShippingDetails, ShippingDetailsResponse>().ReverseMap();
            CreateMap<BaseAddress, ShippingAddressResponse>().ReverseMap();
            CreateMap<BaseAddress, AddressResponse>().ReverseMap();
            CreateMap<UpdateShippingAddressRequest, BaseAddress>()
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));
            CreateMap<UpdateShippingDetailsRequest, ShippingDetails>()
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));

            // Order Header
            CreateMap<OrderHeader, CheckoutResponse>().ReverseMap();
            CreateMap<OrderHeader, OrderResponse>().ReverseMap();
            CreateMap<OrderHeader, OrderOverviewResponse>().ReverseMap();
            CreateMap<OrderDetails, OrderDetailsResponse>().ReverseMap();
            CreateMap<Product, ProductSummaryResponse>();

            // Order Purchase
            CreateMap<PurchaseDetails, PurchaseDetailsSummaryResponse>().ReverseMap();

            CreateMap<ProductSizeRequest, ProductSize>().ReverseMap();
            CreateMap<ProductSize, ProductSizeResponse>().ReverseMap();

            CreateMap<UpdateProductSizeRequest, ProductSize>()
                .ForMember(destination => destination.Id, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((source, destination, srcMember) => srcMember is not null));

        }
    }
}
