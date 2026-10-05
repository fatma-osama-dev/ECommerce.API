using AutoMapper;
using Ecommerce.Application.DTOs.Basket;
using Ecommerce.Application.DTOs.BrandDtos;
using Ecommerce.Application.DTOs.Order;
using Ecommerce.Application.DTOs.ProductDtos;
using Ecommerce.Application.DTOs.TypeDtos;
using Ecommerce.Application.Helpers;
using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Mapping
{
   public  class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductGetDto>()
              .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductUrlResolver>());

            CreateMap<ProductSendDto,Product>();

            CreateMap<ProductUpdateDto, Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProductBrand, BrandGetDto>();
            CreateMap<BrandSendDto, ProductBrand>();


            CreateMap<ProductType, TypeGetDto>();
            CreateMap<TypeSendDto, ProductType>();

            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();

            CreateMap<BasketItem, BasketItemDto>().ReverseMap();

            CreateMap<OrderAddressDto, OrderAddress>().ReverseMap();
          
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Order, OrderGetDto>()
                 .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
                 .ForMember(d => d.DeliveryMethodName, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                 .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Cost))
                 .ForMember(d => d.TotalPrice, o => o.MapFrom(s => s.GetTotal()));

            CreateMap<DeliveryMethod, DeliveryMethodDto>();





        }
    }
}
