using AutoMapper;
using Ecommerce.Application.DTOs.ProductDtos;
using Ecommerce.Application.Response;
using Ecommerce.Application.ServiceInterfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.RepositoryInterfaces;
using Ecommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class ProductService : IProductService
    {

        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }
        public async Task<BaseResponse<ProductGetDto>> CreateProductAsync(ProductSendDto dto)
        {
            try
            {
                var productEntity = _mapper.Map<Product>(dto);

                await _productRepo.AddAsync(productEntity);
                await _productRepo.SaveChangesAsync();

                var resultDto = _mapper.Map<ProductGetDto>(productEntity);
                return new BaseResponse<ProductGetDto>(true, "Product created successfully", resultDto);
            }
            catch (Exception ex)
            {
                return new BaseResponse<ProductGetDto>(false, "Error occurred during product creation", ex);
            }
        }



        public async Task<BaseResponse<string>> DeleteProductAsync(int productId)
        {
            try
            {
                var isDeleted = await _productRepo.DeleteAsync(productId);


                if (isDeleted)
                {
                    await _productRepo.SaveChangesAsync();
                    return new BaseResponse<string>(true, "Product deleted successfully");
                    
                }
                else
                    return new BaseResponse<string>(false, "Product not found");
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>(false, "Error occurred during product deletion", ex);
            }
        }

        public async Task<BaseResponse<IReadOnlyCollection<ProductGetDto>>> GetAllProductsAsync(int? brandId = null, int? typeId = null)
        {
            try
            {

                var products = await _productRepo.FindAsync(p =>
                    (!brandId.HasValue || p.BrandId == brandId) &&
                    (!typeId.HasValue || p.ProductTypeId == typeId)
                );

                var productDtos = _mapper.Map<IReadOnlyCollection<ProductGetDto>>(products);
                return new BaseResponse<IReadOnlyCollection<ProductGetDto>>(true, "Products retrieved successfully", productDtos);
            }
            catch (Exception ex)
            {
                return new BaseResponse<IReadOnlyCollection<ProductGetDto>>(false, "Error occurred while retrieving products", ex);
            }
        }


        public async Task<BaseResponse<ProductGetDto>> GetProductByIdAsync(int productId)
        {
            try
            {
                var product = await _productRepo.GetByIdAsync(productId);
                if (product == null)
                    return new BaseResponse<ProductGetDto>(false, "Product not found");
                else
                {
                    _mapper.Map<ProductGetDto>(product);
                    return new BaseResponse<ProductGetDto>(true, "Product retrieved successfully", _mapper.Map<ProductGetDto>(product));

                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<ProductGetDto>(false, "Error occurred while retrieving the product", ex);
            }
        }
        public async Task<BaseResponse<ProductGetDto>> UpdateProductAsync(int id, ProductUpdateDto dto)
        {
            try
            {
                var existingProduct = await _productRepo.GetByIdAsync(id);
                if (existingProduct == null)
                {
                    return new BaseResponse<ProductGetDto>(false, $"Product with ID {id} not found for updating");
                }
                
                    _mapper.Map(dto, existingProduct);
                    await _productRepo.SaveChangesAsync();
                    return new BaseResponse<ProductGetDto>(true, "Product updated successfully", _mapper.Map<ProductGetDto>(existingProduct));
                
            }
            catch (Exception ex)
            {
                return new BaseResponse<ProductGetDto>(false, "Error occurred during product update", ex);
            }
        }


    }
}