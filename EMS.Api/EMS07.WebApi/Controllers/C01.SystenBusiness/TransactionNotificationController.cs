using AutoMapper;
using EMS.Models;
using EMS01.Foundation.Tools;
using EMS05.ModelsDTO.EMD01.SystemDTO;
using EMS06.DataService;
using EMS07.WebApi.BaseController;
using Foundation.Tools;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS07.WebApi.Controllers.C01.SystenBusiness
{
    public class TransactionNotificationController:ApiBaseController<Guid, TransactionNotification, TransactionNotificationDTO>
    {
        private readonly IMapper _mapper;
        private readonly IApiEntityService<Guid, TransactionNotification, TransactionNotificationDTO> _apiEntityService;
        public TransactionNotificationController(IApiEntityService<Guid, TransactionNotification, TransactionNotificationDTO> apiEntityServer,IMapper mapper):base(apiEntityServer)
        {
            SetIncludeExpressions(x=>x.User);
            _mapper = mapper;
            _apiEntityService = apiEntityServer;
        }

        public override async Task<ResultData<TransactionNotification>> Update(TransactionNotificationDTO entity)
        {
            var bo = _mapper.Map<TransactionNotification>(entity);
            bo.CreateTime = DateTime.Now;
            bo.Title = bo.Name;
            bo.User = await _apiEntityService.GetOtherBoAsync<User>(Guid.Parse(entity.UserId));
            return await _apiEntityService.OverSetAndSaveAsync(bo);
        }
        [HttpPost]
        public  async Task<ResultData<TransactionNotification>> Deletes(List<string> ids)
        {
            var i = ids.ConvertAll(x => Guid.Parse(x));
            return await _apiEntityService.DeleteRangeAsync(i);
        }
        [HttpGet]
        public async Task<List<TransactionNotificationDTO>> Get(int start, int length)
        {
            var result = await _apiEntityService.GetBoAsync(null,x=>x.User);
            result = result.Skip(start).Take(length).ToList();
            return result;
        }

    }
}
