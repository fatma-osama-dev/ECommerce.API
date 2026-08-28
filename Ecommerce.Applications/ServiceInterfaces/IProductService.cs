using Ecommerce.Application.DTOs.ProductDtos;
using Ecommerce.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ServiceInterfaces
{
    public interface IProductService
    {
        Task<BaseResponse<IReadOnlyCollection<ProductGetDto>>> GetAllProductsAsync(int? brandId=null ,int? typeId=null);
        Task<BaseResponse<ProductGetDto>> GetProductByIdAsync(int productId);
        Task<BaseResponse<ProductGetDto>> CreateProductAsync(ProductSendDto dto);
        Task<BaseResponse<ProductGetDto>> UpdateProductAsync(int id, ProductUpdateDto dto);

        Task<BaseResponse<string>> DeleteProductAsync(int productId); 
    }
}
