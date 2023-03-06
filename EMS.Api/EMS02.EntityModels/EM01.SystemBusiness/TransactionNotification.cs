using EMS01.Foundation.Data;
using System;

namespace EMS.Models
{
    /// <summary>
    /// 事务通报
    /// </summary>
    public class TransactionNotification : Data<Guid>
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
        public User User { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
