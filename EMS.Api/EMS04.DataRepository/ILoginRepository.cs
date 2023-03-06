using EMS02.EntityModels.EM02.LoginBusiness;
using Foundation.Tools;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EMS04.DataRepository
{
    public   interface ILoginRepository
    {
        Task<SecurityToken> Login(Login login);
        Task<ResultData<Login>> CreateLogin(Login login);

    }
}
