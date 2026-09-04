using AutoMapper;
using Ecommerce.Application.DTOs.BrandDtos;
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
        }
    }
}
