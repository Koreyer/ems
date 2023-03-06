using AutoMapper;
using EMS.Models;
using EMS01.Foundation.Enum;
using EMS01.Foundation.Tools;
using EMS03.EntityModels.ORM;
using EMS05.ModelsDTO;
using EMS05.ModelsDTO.EMD01.SystemDTO;
using EMS06.DataService;
using EMS07.WebApi.BaseController;
using Foundation.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS07.WebApi.Controllers.C01.SystenBusiness
{
    public class EquipmentUseWithUserController : ApiBaseController<Guid, EquipmentUseWithUser, EquipmentUseWithUserDTO>
    {
        private readonly IMapper _mapper;
        private readonly IApiEntityService<Guid, EquipmentUseWithUser, EquipmentUseWithUserDTO> _apiEntityService;
        private readonly EMSContext _context;
        public EquipmentUseWithUserController(IApiEntityService<Guid, EquipmentUseWithUser, EquipmentUseWithUserDTO> apiEntityServer, IMapper mapper, EMSContext context) : base(apiEntityServer)
        {
            SetIncludeExpressions(x => x.UseUser,x=>x.Equipment);
            _mapper = mapper;
            _apiEntityService = apiEntityServer;
            _context = context;
        }

        public override async Task<ResultData<EquipmentUseWithUser>> Update(EquipmentUseWithUserDTO entity)
        {
            var bo = _mapper.Map<EquipmentUseWithUser>(entity);
            bo.UseUser = await _apiEntityService.GetOtherBoAsync<User>(Guid.Parse(entity.UseUserId));
            
            bo.Equipment = await _apiEntityService.GetOtherBoAsync<Equipment>(Guid.Parse(entity.EquipmentId));
            if (bo.Name == "未通过")
                bo.ExamineEnum = ExamineEnum.未通过;
            else if (bo.Name == "已通过")
            {
                bo.ExamineEnum = ExamineEnum.已通过;
                bo.Equipment.Islend = true;
            }
            else
                bo.ExamineEnum = ExamineEnum.待审核;
            if (bo.IsReturn)
            {
                bo.ReturnTime = DateTime.Now;
                bo.Equipment.Islend = false;
            }
            return await _apiEntityService.OverSetAndSaveAsync(bo);
        }
        /// <summary>
        /// test
        /// </summary>
        /// <param name="id"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [HttpGet]
        public  async Task<List<EquipmentUseWithUserDTO>> Get( Guid id,int start,int length)
        {
            var result = await _apiEntityService.GetBoAsync(x => x.UseUser.Id == id, x => x.UseUser, x => x.Equipment);
            result = result.OrderBy(x => x.IsReturn).ThenByDescending(x=>x.ExamineEnumName).Skip(start).Take(length).ToList();
            return result;
        }
        public override async Task<List<EquipmentUseWithUserDTO>> GetBySearch(SelectParameter selectParameter)
        {
            return await _apiEntityService.GetBoSearchAsync(selectParameter, x => x.ExamineEnum == ExamineEnum.待审核, x => x.UseUser, x => x.Equipment);
        }
        [HttpGet]

        public async Task<int> GetBoCountById(Guid id)
        {
            var result =  await _apiEntityService.GetBoAsync(x => x.UseUser.Id == id);
            return result.Count();
        }
        //[HttpGet]
        public override async Task<int> GetBoCount()
        {
            var result = await _apiEntityService.GetBoAsync(x => x.ExamineEnum == ExamineEnum.待审核, null);
            return result.Count;
        }
        [HttpGet]
        public async Task<RateDTO> GetRate()
        {
            var ews = await _apiEntityService.GetBoAsync(null, x => x.Equipment);
            var equipmentName = ews.Select(x=>x.EquipmentName).Distinct().ToList();
            var  result = new RateDTO() { Data = new List<Datas>()};
            foreach (var item in equipmentName)
            {

                result.Data.Add(new Datas()
                {
                    Name = item,
                    Count = ews.Where(x => x.EquipmentName == item).Count()
                });

            }
            result.Count = ews.ToList().Count;
            return result;
        }

    }
}
