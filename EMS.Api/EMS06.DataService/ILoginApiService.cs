using AutoMapper;
using DataRepository;
using EMS.Models;
using EMS04.DataRepository;
using EMS05.ModelsDTO.EMD02.LoginBusinessDTO;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EMS06.DataService
{
    public interface ILoginApiService
    {
        Task<SecurityToken> Login(LoginDTO logindto);
    }
}
