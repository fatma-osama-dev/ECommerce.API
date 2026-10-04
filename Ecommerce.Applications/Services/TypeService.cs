using AutoMapper;
using Ecommerce.Application.DTOs.TypeDtos;
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
    public class TypeService : ITypeService
    {
        private readonly IGenericRepository<ProductType> _typeRepo;
        private readonly IMapper _mapper;
        public TypeService(IGenericRepository<ProductType> typeRepo, IMapper mapper)
        {
            _typeRepo = typeRepo;
            _mapper = mapper;
        }

        public async Task<BaseResponse<TypeGetDto>> CreateTypeAsync(TypeSendDto dto)
        {
            try {
                var typeEntity = _mapper.Map<ProductType>(dto);
                await _typeRepo.AddAsync(typeEntity);
                await _typeRepo.SaveChangesAsync();
                return new BaseResponse<TypeGetDto>(true, "Type created successfully", _mapper.Map<TypeGetDto>(typeEntity));
            }
            catch (Exception ex) {
                return new BaseResponse<TypeGetDto>(false, "Error occurred during Type creation", ex);
            }
        }

        public async Task<BaseResponse<string>> DeleteTypeAsync(int typeId)
        {
            try
            {
                var isDeleted = await _typeRepo.DeleteAsync(typeId);
                if (isDeleted)
                {
                    await _typeRepo.SaveChangesAsync();
                    return new BaseResponse<string>(true, "Type deleted successfully");
                }
                return new BaseResponse<string>(false, "Type not found");

            }
            catch (Exception ex)
            {
                return new BaseResponse<string>(false, "Error occurred during Type deletion", ex);
            }
        }
        public async Task<BaseResponse<IReadOnlyCollection<TypeGetDto>>> GetAllTypesAsync()
        {
            try { 
                var types = await _typeRepo.GetAllAsync();
                if (types == null) { 
                    return new BaseResponse<IReadOnlyCollection<TypeGetDto>>(false, "No Types found");
                }
               
                    var typeDtos = _mapper.Map<IReadOnlyCollection<TypeGetDto>>(types);
                    return new BaseResponse<IReadOnlyCollection<TypeGetDto>>(true, "Types retrieved successfully", typeDtos); 
                

            }
            catch (Exception ex) { 
                return new BaseResponse<IReadOnlyCollection<TypeGetDto>>(false, "Error occurred while retrieving Types.", ex);
            }
        }

        public async Task<BaseResponse<TypeGetDto>> GetTypeByTypeIdAsync(int typeId)
        {
            try {
                var typeEntity = await _typeRepo.GetByIdAsync(typeId);
                if (typeEntity==null)
                {
                    return new BaseResponse<TypeGetDto>(false, "Type not found");
                }
                return new BaseResponse<TypeGetDto>(true, "Type retrieved successfully", _mapper.Map<TypeGetDto>(typeEntity));
            }
            catch (Exception ex) { 
                return new BaseResponse<TypeGetDto>(false, "Error occurred while retrieving the Type", ex);
            }
        }

        public async Task<BaseResponse<TypeGetDto>> UpdateTypeByTypeIdAsync(int typeId, TypeSendDto dto)
        {
            try { 
                var typeEntity = await _typeRepo.GetByIdAsync(typeId);
                if (typeEntity == null) {
                    return new BaseResponse<TypeGetDto>(false, "Type not found");
                }
                else
                {
                    typeEntity.Name = dto.Name;
                    await _typeRepo.SaveChangesAsync();
                    return new BaseResponse<TypeGetDto>(true, "Type updated successfully", _mapper.Map<TypeGetDto>(typeEntity));

                }
            }
            catch (Exception ex) {
                return new BaseResponse<TypeGetDto>(false, "Error occurred while updating the Type", ex);
            }
        }
    }
}
