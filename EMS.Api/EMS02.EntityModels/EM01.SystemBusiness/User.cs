using EMS01.Foundation.Data;
using EMS01.Foundation.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace EMS.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class User:Data<Guid>
    {
        public string Phone { get; set; }
        public SexEnum Sex { get; set; }
        //软删除
        public bool IsDelete { get; set; }
    }
}
