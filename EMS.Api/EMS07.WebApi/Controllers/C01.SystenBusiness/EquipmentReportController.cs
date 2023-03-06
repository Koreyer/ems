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
    //[Authorize(Roles = "管理员")]
    public class EquipmentReportController: ApiBaseController<Guid, EquipmentReport, EquipmentReportDTO>
    {
        private readonly IMapper _mapper;
        private readonly IApiEntityService<Guid, EquipmentReport, EquipmentReportDTO> _apiEntityService;
        public EquipmentReportController(IApiEntityService<Guid, EquipmentReport, EquipmentReportDTO> apiEntityServer,IMapper mapper):base(apiEntityServer)
        {
            SetIncludeExpressions(x=>x.RepairUser,x=>x.ReportUser,x=>x.Equipment);
            _mapper = mapper;
            _apiEntityService = apiEntityServer;
        }

        public override async Task<ResultData<EquipmentReport>> Update(EquipmentReportDTO entity)
        {
            var bo = _mapper.Map<EquipmentReport>(entity);
            bo.ReportUser = await _apiEntityService.GetOtherBoAsync<User>(Guid.Parse(entity.ReportUserId));
            if(entity.RepairUserId !=null)
                bo.RepairUser = await _apiEntityService.GetOtherBoAsync<User>(Guid.Parse(entity.RepairUserId));
            bo.Equipment = await _apiEntityService.GetOtherBoAsync<Equipment>(Guid.Parse(entity.EquipmentId));
            if(entity.IsRepair == true)
                bo.UpdateTime = DateTime.Now;
            else
                bo.CreateTime = DateTime.Now;
            return await _apiEntityService.OverSetAndSaveAsync(bo);
        }
        [HttpGet]
        public async Task<List<EquipmentReportDTO>> GetById(Guid id, int start, int length)
        {
            var result = await _apiEntityService.GetBoAsync(x => x.ReportUser.Id == id, x => x.ReportUser, x => x.Equipment);
            result = result.Skip(start).Take(length).ToList();
            return result;
        }
        [HttpGet]
        public async Task<List<EquipmentReportDTO>> Get(int start, int length,string value)
        {
            var result = new List<EquipmentReportDTO>{ };
            if (value != null)
                result = await _apiEntityService.GetBoAsync(x => x.Equipment.Name == value &&x.IsRepair ==false , x => x.ReportUser, x => x.Equipment, x => x.RepairUser);
            else
                 result = await _apiEntityService.GetBoAsync( x=> x.IsRepair == false, x => x.ReportUser, x => x.Equipment,x=>x.RepairUser);
            result = result.Where(x => x.IsRepair== false).ToList();
            result = result.Skip(start).Take(length).ToList();
            return result;
        }
        public override async Task<List<EquipmentReportDTO>> GetBySearch(SelectParameter selectParameter)
        {
            return await _apiEntityService.GetBoSearchAsync(selectParameter, x=>x.IsRepair == false, x => x.ReportUser, x => x.Equipment, x => x.RepairUser);
        }

        public override async Task<int> GetBoCount()
        {
            var result = await _apiEntityService.GetBoAsync(x => x.IsRepair == false, null);
            return result.Count;
        }


    }
}
