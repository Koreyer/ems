using EMS01.Foundation.Enum;
using EMS01.Foundation.Data;
using System;

namespace EMS.Models
{
    /// <summary>
    /// 设备使用申请表
    /// </summary>
    public class EquipmentUseWithUser : Data<Guid>
    {
        /// <summary>
        /// 申请人
        /// </summary>
        public User UseUser { get; set; }
        /// <summary>
        /// 借用的设备
        /// </summary>
        public Equipment Equipment { get; set; }
        /// <summary>
        /// 是否归还
        /// </summary>
        public bool IsReturn { get; set; }
        /// <summary>
        /// 归还时间
        /// </summary>
        public DateTime? ReturnTime { get; set; }
        /// <summary>
        /// 归还时状态
        /// </summary>
        public SituationEnum ReturnSituation { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>

        public ExamineEnum ExamineEnum { get; set; }
        /// <summary>
        /// 留言
        /// </summary>
        public string Message { get; set; }
    }
}
