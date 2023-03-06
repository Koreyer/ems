using EMS.Models;
using EMS05.ModelsDTO.EMD01.SystemDTO;
using EMS06.DataService;
using EMS07.WebApi.BaseController;
using Foundation.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS07.WebApi.Controllers.C01.SystenBusiness
{
   

    public class EquipmentCategoryController:ApiBaseController<Guid, EquipmentCategory, EquipmentCategoryDTO>
    {
        private readonly IApiEntityService<Guid, EquipmentCategory, EquipmentCategoryDTO> _apiEntityService;
        public EquipmentCategoryController(IApiEntityService<Guid, EquipmentCategory, EquipmentCategoryDTO> apiEntityService) : base(apiEntityService) { 
            _apiEntityService = apiEntityService;
        }
        [Authorize(Roles = "管理员")]
        [HttpPost]
        public async Task<ResultData<EquipmentCategory>> Deletes(List<string> ids)
        {
            var i = ids.ConvertAll(x => Guid.Parse(x));
            return await _apiEntityService.DeleteRangeAsync(i);
        }
    }
    
}
