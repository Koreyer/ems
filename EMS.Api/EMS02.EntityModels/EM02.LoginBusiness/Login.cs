using EMS.Models;
using EMS01.Foundation.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS02.EntityModels.EM02.LoginBusiness
{
    public class Login : Data<Guid>
    {
        public User User { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "账号不能为空"), MaxLength(16)]
        public string Account { get; set; }
        /// <summary>
        /// 密
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        /// <summary>
        /// 是否管理员
        /// </summary>
        public bool IsRoot { get; set; }
        /// <summary>
        /// 用户创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 上一次密码修改时间
        /// </summary>
        public DateTime? PassTime { get; set; }
        public bool IsDelete { get; set; }
    }
}
