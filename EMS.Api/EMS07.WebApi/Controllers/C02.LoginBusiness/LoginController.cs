using AutoMapper;
using EMS.Models;
using EMS01.Foundation.Enum;
using EMS01.Foundation.Tools;
using EMS02.EntityModels.EM02.LoginBusiness;
using EMS03.EntityModels.ORM;
using EMS05.ModelsDTO.EMD02.LoginBusinessDTO;
using EMS06.DataService;
using EMS07.WebApi.BaseController;
using Foundation.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EMS07.WebApi.Controllers.C02.LoginBusiness
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class LoginController:ApiBaseController<Guid,Login,LoginDTO>
    {
        private readonly EMSContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IApiEntityService<Guid, Login, LoginDTO> _apiEntityService;
        public LoginController(EMSContext context,IMapper mapper, IConfiguration configuration, IApiEntityService<Guid,Login,LoginDTO> apiEntityService):base(apiEntityService)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _apiEntityService = apiEntityService;
            SetIncludeExpressions(x => x.User);
        }

        #region 登录相关
        [HttpPost]
        public async Task<SecurityToken> Login(LoginDTO login)
        {
            var resultData = new ResultData<LoginDTO>();
            var pass = MD5Encrypt32(login.Password);
            var loginUser = await _context.Set<Login>().Include(x => x.User).Where(x => x.Account == login.Account && x.Password == pass).FirstOrDefaultAsync();
            //loginUser.User =  _context.Set<User>().Where(x => x.Id == loginUser.User.Id).FirstOrDefault();
            if (loginUser != null)
            {
                resultData.Message = "登录成功";
                resultData.ResultEnum = ResultEnum.操作成功;
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                var jwt = new SecurityTokenDescriptor
                {
                    Issuer = _configuration["JWT:Issuer"],
                    Audience = _configuration["JWT:Audience"],
                    Subject = new ClaimsIdentity(new[]
                       {
                        new Claim("accid", loginUser.User.Id.ToString()),
                        new Claim("id", loginUser.Id.ToString()),
                            new Claim("name", loginUser.User.Name),
                            new Claim(ClaimTypes.Role, loginUser.Name)
                        }),
                    //有效时间
                    Expires = DateTime.Now.AddDays(30),
                    SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                };
                SecurityToken securityToken = new JwtSecurityTokenHandler().CreateToken(jwt);
                return securityToken;
            }
            else
            {
                resultData.Message = "账号或者密码错误";
                resultData.ResultEnum = ResultEnum.操作失败;
                return new JwtSecurityTokenHandler().CreateToken(new SecurityTokenDescriptor { Issuer="登录失败"});
            }

        }
        [HttpPost]
        public async Task<string> PassEdit(LoginDTO loginDTO)
        {
            var login = await _context.Set<Login>().Include(x => x.User).Where(x => x.Id == loginDTO.Id).FirstOrDefaultAsync();
            var pass = MD5Encrypt32(loginDTO.Password);
            login.Password = pass;
            try
            {
                _context.Update(login);
                await _context.SaveChangesAsync();
                return "修改成功";
            }
            catch (Exception)
            {

                return "修改失败";
            }

            return "";
        }
        [HttpPost]

        public async Task<ResultData<LoginDTO>> CreateLogin(LoginDTO loginDTO)
        {
            var resultData = new ResultData<LoginDTO>();
            loginDTO.Password = MD5Encrypt32(loginDTO.Password);
            var loginUser = await _context.Set<Login>().Include(x => x.User).Where(x => x.Account == loginDTO.Account).FirstOrDefaultAsync();
            var sex = SexEnum.女;
            if (loginDTO.UserSex == "男")
                sex = SexEnum.男;
            if (loginUser != null)
            {
                loginUser.User.Sex = sex;
                loginUser.User.Name = loginDTO.UserName;
                loginUser.User.Phone = loginDTO.UserPhone;
                _context.Update(loginUser);
                await _context.SaveChangesAsync();
                resultData.Message = "更新成功";
                resultData.ResultEnum = ResultEnum.更新成功;
            }
            else
            {
                
                loginDTO.CreateTime = DateTime.Now;
                loginDTO.IsRoot = false;
                var login = _mapper.Map<Login>(loginDTO);
                login.User = new User() { 
                    Id=Guid.NewGuid() ,
                    Name = loginDTO.UserName,
                    Sex = sex,
                    Phone = loginDTO.UserPhone
                };
                login.Name = "学生";
                _context.Add(login);
                await _context.SaveChangesAsync();
                resultData.Message = "账号创建成功";
                resultData.ResultEnum = ResultEnum.新增成功;
            }

            return resultData;
        }
        public static string MD5Encrypt32(string password)
        {
            string cl = password;
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }
        public override async Task<List<LoginDTO>> GetBySearch(SelectParameter selectParameter)
        {
           
            if( !string.IsNullOrEmpty(selectParameter.searchValue))
            {
                var name = selectParameter.searchValue;
                selectParameter.searchValue = null;
                return await _apiEntityService.GetBoSearchAsync(selectParameter, x => x.User.Name == name && x.IsDelete == false, x => x.User);
            }
            else
            {
                return await _apiEntityService.GetBoSearchAsync(selectParameter, x=>x.IsDelete == false, x => x.User);
            }
           
        }

        public override async Task<ResultData<Login>> Delete(Guid id)
        {
            try
            {
                var login = await _context.Set<Login>().Include(x => x.User).Where(x => x.Id == id).FirstOrDefaultAsync();
                login.IsDelete = true;
                _context.Update(login);
                await _context.SaveChangesAsync();
                return new ResultData<Login>() { Message = "删除成功" };
            }
            catch (Exception es)
            {

                return new ResultData<Login>() { ErrorMessage="操作失败"};
            }
            
        }

        public override async Task<ResultData<Login>> DeleteRange(List<Guid> ids)
        {
            try
            {
                foreach (var item in ids)
                {
                    var user = await _apiEntityService.GetOneBoAsync(x => x.Id == item, x => x.User);
                    user.IsDelete = true;
                    _context.Update(user);
                }
                await _context.SaveChangesAsync();
                return new ResultData<Login> { Message = "删除成功" ,Code=200};
            }
            catch (Exception)
            {

                return new ResultData<Login>() { ErrorMessage = "操作失败" };
            }
            
            
        }
        #endregion

    }
}
