
using EMS01.Foundation.Data;
using EMS01.Foundation.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS05.ModelsDTO.EMD01.SystemDTO
{
    /// <summary>
    /// 设备使用申请表
    /// </summary>
    public class EquipmentUseWithUserDTO : Data<Guid>
    {
        /// <summary>
        /// 申请人
        /// </summary>
        public string? UseUserId { get; set; }
        public string? UseUserName { get; set; }
        /// <summary>
        /// 借用的设备
        /// </summary>
        public string? EquipmentId { get; set; }
        public string? EquipmentName { get; set; }
        /// <summary>
        /// 是否归还
        /// </summary>
        public bool? IsReturn { get; set; }
        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? ReturnTime { get; set; }
        /// <summary>
        /// 归还时状态
        /// </summary>
        public string? ReturnSituationName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>

        public string? ExamineEnumName { get; set; }
        /// <summary>
        /// 留言
        /// </summary>
        public string? Message { get; set; }
    }
}
