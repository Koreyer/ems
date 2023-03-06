using EMS01.Foundation.Data;
using System;

namespace EMS.Models
{
    /// <summary>
    /// 设备报修表
    /// </summary>
    public class EquipmentReport:Data<Guid>
    {
        /// <summary>
        /// 报修时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 维修时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 是否已经维修
        /// </summary>
        public bool IsRepair { get; set; }
        /// <summary>
        /// 维修用户
        /// </summary>
        public User? RepairUser { get; set; }
        /// <summary>
        /// 报修用户
        /// </summary>
        public User ReportUser { get; set; }
        /// <summary>
        /// 报修设备
        /// </summary>
        public Equipment Equipment { get; set; }
        /// <summary>
        /// 留言
        /// </summary>
        public string Message { get; set; }
    }
}
