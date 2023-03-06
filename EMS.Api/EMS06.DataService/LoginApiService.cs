using AutoMapper;
using DataRepository;
using EMS.Models;
using EMS02.EntityModels.EM02.LoginBusiness;
using EMS04.DataRepository;
using EMS05.ModelsDTO.EMD02.LoginBusinessDTO;
using Foundation.Tools;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EMS06.DataService
{
    public class LoginApiService:ILoginApiService
    {
        private readonly IRepository<Guid, User> _user;
        private readonly IMapper _mapper;
        private readonly ILoginRepository _repository;
        public LoginApiService(IMapper mapper, ILoginRepository repository, IRepository<Guid, User> user)
        {
            _mapper = mapper;
            _repository = repository;
            _user = user;
        }
        #region 登录相关
        public virtual async Task<SecurityToken> Login(LoginDTO logindto)
        {
            var login = _mapper.Map<Login>(logindto);
            //login.User = await _user.GetOneBo(x => x.Id == Guid.Parse(logindto.UserId));
            return await _repository.Login(login);
        }
        #endregion
    }
}
