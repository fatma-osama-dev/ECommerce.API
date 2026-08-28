using AutoMapper;
using Ecommerce.Application.DTOs.BrandDtos;
using Ecommerce.Application.Response;
using Ecommerce.Application.ServiceInterfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class BrandService : IBrandService         
    {
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IMapper _mapper;
        public BrandService(IGenericRepository<ProductBrand> brandRepo, IMapper mapper)
        {
            _brandRepo = brandRepo;
            _mapper = mapper;
        }

        public async Task<BaseResponse<BrandGetDto>> CreateProductBrandAsync(BrandSendDto dto)
        {
            try {
                var brandEntity = _mapper.Map<ProductBrand>(dto);
                await _brandRepo.AddAsync(brandEntity);
                await _brandRepo.SaveChangesAsync();
                return new BaseResponse<BrandGetDto>(true, "ProductBrand created successfully.", _mapper.Map<BrandGetDto>(brandEntity));    
            }
            catch (Exception ex) {
                return new BaseResponse<BrandGetDto>(false, "Error occurred during productBrand creation.", ex);
            }
        }

        public async  Task<BaseResponse<string>> DeleteBrandByIdAsync(int brandId)
        {
            try
            {
                var isDeleted = await _brandRepo.DeleteAsync(brandId);
               
                if (isDeleted)
                {
                    await _brandRepo.SaveChangesAsync();
                    return new BaseResponse<string>(true, "Brand deleted successfully.");
                }
                else
                {
                    return new BaseResponse<string>(false, "Brand not found.");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>(false, "Error occurred during brand deletion.", ex);
            }
        }



        public async Task<BaseResponse<IReadOnlyCollection<BrandGetDto>>> GetAllBrandsAsync()
        {
            try {
               var brandEntities = await _brandRepo.GetAllAsync();
                if (brandEntities == null){
                    return new BaseResponse<IReadOnlyCollection<BrandGetDto>>(false, "No brands found.");
                }
               
                var brandDtos = _mapper.Map<IReadOnlyCollection<BrandGetDto>>(brandEntities);
                 return new BaseResponse<IReadOnlyCollection<BrandGetDto>>(true, "Brands retrieved successfully.", brandDtos);
                
                    
            }
            catch (Exception ex) { 
                return new BaseResponse<IReadOnlyCollection<BrandGetDto>>(false, "Error occurred while retrieving brands.", ex);
            }
        }


        public async Task<BaseResponse<BrandGetDto>> GetBrandByBrandIdAsync(int brandId)
        {
            try{
                var brandEntity = await _brandRepo.GetByIdAsync(brandId);
                if (brandEntity==null)
                {
                    return new BaseResponse<BrandGetDto>(false, "Brand not found");
                }
                return new BaseResponse<BrandGetDto>(true, "Brand retrieved successfully.", _mapper.Map<BrandGetDto>(brandEntity));
            }
            catch (Exception ex) {
                return new BaseResponse<BrandGetDto>(false, "Error occurred while retrieving the brand.", ex);
            }
        }


        public async Task<BaseResponse<BrandGetDto>> UpdateBrandByIdAsync(int brandId, BrandSendDto dto)
        {
            try { 
                var existingBrand = await _brandRepo.GetByIdAsync(brandId);
                if(existingBrand == null)
                {
                    return new BaseResponse<BrandGetDto>(false, $"Brand with ID {brandId} not found for updating.");
                }
                existingBrand.Name = dto.Name;
                await _brandRepo.UpdateAsync(existingBrand);
                await _brandRepo.SaveChangesAsync();
                var result = _mapper.Map<BrandGetDto>(existingBrand);
                return new BaseResponse<BrandGetDto>(true, "Brand updated successfully.",result);
            }
            catch(Exception ex) { 
                return new BaseResponse<BrandGetDto>(false, "Error occurred while updating the brand.", ex);
            }
        }


    }
}
