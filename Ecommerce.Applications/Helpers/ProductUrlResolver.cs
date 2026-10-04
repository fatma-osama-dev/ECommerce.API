using AutoMapper;
using Ecommerce.Application.DTOs.ProductDtos;
using Ecommerce.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Helpers
{
    public class ProductUrlResolver: IValueResolver<Product, ProductGetDto, string>
    {
       private readonly IConfiguration _config;
        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }
        public string Resolve(Product source, ProductGetDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["BaseUrl"] + source.PictureUrl;
            }
            return string.Empty;
        }
    }
}
