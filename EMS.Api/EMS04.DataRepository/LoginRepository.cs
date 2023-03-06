using EMS01.Foundation.Enum;
using EMS02.EntityModels.EM02.LoginBusiness;
using EMS03.EntityModels.ORM;
using Foundation.Tools;
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

namespace EMS04.DataRepository
{
    public class LoginRepository:ILoginRepository
    {
        private readonly EMSContext _context;
        private readonly IConfiguration _configuration;

        LoginRepository(EMSContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        #region 登录相关
        public async Task<SecurityToken> Login(Login login)
        {
            var resultData = new ResultData<Login>();
            var pass = MD5Encrypt32(login.Password);
            var loginUser = await _context.Set<Login>().Include(x => x.User).Where(x => x.Account == login.Account && x.Password == pass).FirstOrDefaultAsync();
            if (loginUser != null)
            {
                resultData.Message = "登录成功";
                resultData.ResultEnum = ResultEnum.操作成功;
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                var jwt = new SecurityTokenDescriptor
                {
                    Issuer = _configuration["JWT:Issuer"],
                    Audience = _configuration["JWT:Audience"],
                  Subject= new ClaimsIdentity(new[]
                       {
                            new Claim("昵称", loginUser.User.Name),
                            new Claim(ClaimTypes.Role, loginUser.Name)
                        }),
                    //有效时间
                    Expires =DateTime.Now.AddMinutes(30),
                  SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                };
                SecurityToken securityToken = new JwtSecurityTokenHandler().CreateToken(jwt);
                return securityToken;
            }
            else
            {
                resultData.Message = "账号或者密码错误";
                resultData.ResultEnum = ResultEnum.操作失败;
                return null;
            }

            
        }

        public async Task<ResultData<Login>> CreateLogin(Login login)
        {
            var resultData = new ResultData<Login>();
            login.Password = MD5Encrypt32(login.Password);
            var loginUser = await _context.Set<Login>().Include(x => x.User).Where(x => x.Account == login.Account).FirstOrDefaultAsync();
            if (loginUser != null)
            {
                resultData.ErrorMessage = "账号已存在";
                resultData.ResultEnum = ResultEnum.新增失败;
            }
            else
            {
                login.CreateTime = DateTime.Now;
                login.IsRoot = false;
                _context.Add(login);
                await _context.SaveChangesAsync();
                resultData.ErrorMessage = "账号创建成功";
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
        #endregion
    }
}
