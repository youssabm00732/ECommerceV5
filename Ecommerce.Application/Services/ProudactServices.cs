using AutoMapper;
using Ecommerce.Application.Common;
using Ecommerce.Application.Contracts;
using Ecommerce.Application.DTO_s.Proudacts;
using Ecommerce.Application.Params;
using Ecommerce.Application.Specifications;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.Proudacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class ProudactServices : IProudactServices
    {

        #region Dependancy Injection 
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProudactServices(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        #endregion
        public async Task<Result<PaginatedResult<ProudactDto>>> GetAllProudactsAsync(ProudactQueryParams queryParams, CancellationToken ct = default)
        {
            var spec = new ProudactSpecification(queryParams);
            // Get all Proudacts In database 
            var Proudacts = await unitOfWork.GetRepository<Proudact, int>().GetAllWithSpeceficationsAsync(spec, ct);

            // mapp from model -> Dto
            var mapperProudcats = mapper.Map<IReadOnlyList<Proudact>, IReadOnlyList<ProudactDto>>(Proudacts);

            // count Specifications 
            var CountSpec = new ProudactCountSpecifications(queryParams); // GetAll or Filter 
            var TotalCount = await unitOfWork.GetRepository<Proudact, int>().GetProudactCountWithSpecificationAsync(CountSpec, ct);
            // retrun list of Proudacts and return ok() as an a response tmam .. after update b2it arg3 list of proudacts + info about data like PageSize , PageIndex , Count (3mlo specification w mnady 3leh fel str aly fo2y)
            return Result<PaginatedResult<ProudactDto>>.Ok(new PaginatedResult<ProudactDto>(mapperProudcats , Proudacts.Count, queryParams.PageIndex, TotalCount));
        }

        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllProudactsBrandAsync(CancellationToken ct = default)
        {
            // Get all ProudactsBrand In database 
            var Brands = await unitOfWork.GetRepository<ProudactBrand, int>().GetAllAsync(ct);

            // mapp from model -> Dto
            var mapperBrands = mapper.Map<IReadOnlyList<ProudactBrand>, IReadOnlyList<BrandDto>>(Brands);

            // retrun list of ProudactsBrand and return ok() as an a response tmam 
            return Result<IReadOnlyList<BrandDto>>.Ok(mapperBrands); // Da al Generic Result class .. aly bya5od mny TEntity
        }

        public async Task<Result<IReadOnlyList<TypeDto>>> GetAllProudactsTypeAsync(CancellationToken ct = default)
        {
            // Get all ProudactsType In database 
            var Types = await unitOfWork.GetRepository<ProudactTypes, int>().GetAllAsync();

            // mapp from model -> Dto
            var mapperTypes= mapper.Map<IReadOnlyList<ProudactTypes>, IReadOnlyList<TypeDto>>(Types);

            // retrun list of ProudactsType and return ok() as an a response tmam 
            return Result<IReadOnlyList<TypeDto>>.Ok(mapperTypes);
        }

        public async Task<Result<ProudactDto>> GetProudactByIdAsync(int id, CancellationToken ct = default)
        {
            var spec = new ProudactSpecification(id);
            // Get all Proudacts and then by id 
            var Proudact = await unitOfWork.GetRepository<Proudact, int>().GetByIdWithSpecificationAsync(spec, ct);
            // make validation if proudact is null or not 
            if (Proudact is null)
                return Result<ProudactDto>.Fail(Error.NotFound("Proudact.NotFound", $"Proudact With Id :{id} is not found"));
            // mapp from model -> dto 
            var ProudactMapp = mapper.Map<Proudact, ProudactDto>(Proudact);
            // return result = ok() 
            return Result<ProudactDto>.Ok(ProudactMapp);
        }

        #region Continue in Result Pattern 
        /*
          al Services de btt3aml m3 al Result Class al Generic aly bya5od TEntity (Al class aly t7t fel Common)
          w ana bt3aml m3 Function Ok() aly hya ht3ml eh ? 
          bt5li IsSuccess = true and Error = Empty 
          tb fen al Data ?? fa ana fel Result Class bs h get al Data lw f 7alt anha success da aly hzwdo fel Class 

          =>  public TValue data => IsSuccessed ? _value : default; 
         */
        #endregion
    }
}
