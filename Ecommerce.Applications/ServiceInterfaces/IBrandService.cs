using Ecommerce.Application.DTOs.BrandDtos;
using Ecommerce.Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.ServiceInterfaces
{
    public interface IBrandService
    {
       Task <BaseResponse<BrandGetDto>>CreateProductBrandAsync(BrandSendDto dto);
       Task <BaseResponse<BrandGetDto>>UpdateBrandByIdAsync(int brandId,  BrandSendDto dto);

        Task<BaseResponse<string>> DeleteBrandByIdAsync(int brandId);
        Task<BaseResponse<IReadOnlyCollection<BrandGetDto>>> GetAllBrandsAsync();
        Task<BaseResponse<BrandGetDto>> GetBrandByBrandIdAsync(int brandId);
    }
}
