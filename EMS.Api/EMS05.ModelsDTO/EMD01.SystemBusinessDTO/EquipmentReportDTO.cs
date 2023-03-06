using EMS01.Foundation.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS05.ModelsDTO.EMD01.SystemDTO
{
    /// <summary>
    /// 设备报修表
    /// </summary>
    public class EquipmentReportDTO : Data<Guid>
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
        public string? RepairUserId { get; set; }
        public string? RepairUserName { get; set; }
        /// <summary>
        /// 报修用户
        /// </summary>
        public string ReportUserId { get; set; }
        public string ReportUserName { get; set; }
        /// <summary>
        /// 报修设备
        /// </summary>
        public string EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        /// <summary>
        /// 留言
        /// </summary>
        public string Message { get; set; }
    }
}
