using EMS01.Foundation.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS05.ModelsDTO.EMD01.SystemDTO
{
    /// <summary>
    /// 事务通报
    /// </summary>
    public class TransactionNotificationDTO : Data<Guid>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 通报内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 通报人
        /// </summary>
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
