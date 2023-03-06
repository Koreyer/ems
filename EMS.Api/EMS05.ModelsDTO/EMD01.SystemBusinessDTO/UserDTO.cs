using EMS01.Foundation.Data;
using System;

namespace EMS05.ModelsDTO.EMD01.SystemDTO
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserDTO : Data<Guid>
    {
        public string Phone { get; set; }
        public string SexName { get; set; }
        //软删除
        public bool IsDelete { get; set; }
    }
}

