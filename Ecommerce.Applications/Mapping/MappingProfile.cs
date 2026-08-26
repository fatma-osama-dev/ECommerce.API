using AutoMapper;
using Ecommerce.Application.DTOs.BrandDtos;
using Ecommerce.Application.DTOs.ProductDtos;
using Ecommerce.Application.DTOs.TypeDtos;
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
            CreateMap<Product, ProductGetDto>();
            CreateMap<ProductSendDto,Product>();

            CreateMap<ProductBrand, BrandGetDto>();
            CreateMap<BrandSendDto, ProductBrand>();


            CreateMap<ProductType, TypeGetDto>();
            CreateMap<TypeSendDto, ProductType>();
        }
    }
}
