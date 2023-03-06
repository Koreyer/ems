using AutoMapper;
using EMS.Models;
using EMS01.Foundation.Tools;
using EMS05.ModelsDTO;
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
    [Authorize(Roles = "管理员")]
    public class EquipmentScrapController:ApiBaseController<Guid, EquipmentScrap, EquipmentScrapDTO>
    {
        private readonly IMapper _mapper;
        private readonly IApiEntityService<Guid, EquipmentScrap, EquipmentScrapDTO> _apiEntityService;
        public EquipmentScrapController(IApiEntityService<Guid, EquipmentScrap, EquipmentScrapDTO> apiEntityServer,IMapper mapper):base(apiEntityServer)
        {
            SetIncludeExpressions(x=>x.Equipment);
            _mapper = mapper;
            _apiEntityService = apiEntityServer;
        }

        public override async Task<ResultData<EquipmentScrap>> Update([FromBody] EquipmentScrapDTO entity)
        {
            var bo = _mapper.Map<EquipmentScrap>(entity);
            bo.Equipment = await _apiEntityService.GetOtherBoAsync<Equipment>(Guid.Parse(entity.EquipmentId));
            bo.Equipment.IsScrap = true;
            bo.CreateTime = DateTime.Now;
            return await _apiEntityService.OverSetAndSaveAsync(bo);
        }

        

    }
}
