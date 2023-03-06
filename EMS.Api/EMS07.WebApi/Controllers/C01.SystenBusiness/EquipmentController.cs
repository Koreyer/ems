using AutoMapper;
using EMS.Models;
using EMS01.Foundation.Tools;
using EMS05.ModelsDTO.EMD01.SystemDTO;
using EMS06.DataService;
using EMS07.WebApi.BaseController;
using Foundation.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS07.WebApi.Controllers.C01.SystenBusiness
{
    
    public class EquipmentController : ApiBaseController<Guid, Equipment, EquipmentDTO>
    {
        private readonly IMapper _mapper;
        private readonly IApiEntityService<Guid, Equipment, EquipmentDTO> _apiEntityService;
        public EquipmentController(IApiEntityService<Guid, Equipment, EquipmentDTO> apiEntityService,IMapper mapper) : base(apiEntityService)
        {
            _mapper = mapper;
            _apiEntityService = apiEntityService;
        }
        //[Authorize(Roles = "管理员")]
        public override async Task<ResultData<Equipment>> Update(EquipmentDTO equipmentDTO)
        {
            var bo = _mapper.Map<Equipment>(equipmentDTO);
            if(equipmentDTO.EquipmentCategoryId != null)
                bo.EquipmentCategory = await _apiEntityService.GetOtherBoAsync<EquipmentCategory>(Guid.Parse(equipmentDTO.EquipmentCategoryId));
            return await  _apiEntityService.OverSetAndSaveAsync(bo);
        }
        /// <summary>
        /// sdsdsd
        /// </summary>
        /// <param name="selectParameter"></param>
        /// <returns></returns>
        public override async Task<List<EquipmentDTO>> GetBySearch(SelectParameter selectParameter)
        {
            var res =  await _apiEntityService.GetBoSearchAsync(selectParameter, x=>x.IsScrap == false, x=>x.EquipmentCategory);
            return res;
        }

        public override async Task<int> GetBoCount()
        {
            var result = await _apiEntityService.GetBoAsync(x=>x.IsScrap == false);
            //result = result.Where(x => x.IsScrap == false).ToList();
            return result.Count;
        }
        [HttpPost]
        public async Task<List<EquipmentDTO>> Get(SelectParameter selectParameter)
        {
            var res = await _apiEntityService.GetBoSearchAsync(selectParameter, x => x.IsScrap == false && x.Islend == false, x => x.EquipmentCategory);
            return res;
        }
        [HttpGet]
        public async Task<int> GetCount()
        {
            var result = await _apiEntityService.GetBoAsync(x => x.IsScrap == false && x.Islend == false);
            return result.Count;
        }



    }
}
