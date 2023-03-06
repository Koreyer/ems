using EMS01.Foundation.Data;
using EMS01.Foundation.Enum;
using System;

namespace EMS.Models
{
    /// <summary>
    /// 设备表
    /// </summary>
    public class Equipment:Data<Guid>
    {
        /// <summary>
        /// 设备购买时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 设备类别
        /// </summary>
        public EquipmentCategory EquipmentCategory { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public SituationEnum SituationEnum { get; set; }
        /// <summary>
        /// 是否报废
        /// </summary>
        public bool IsScrap { get; set; }
        /// <summary>
        /// 是否借出
        /// </summary>
        public bool  Islend { get; set; }

    }
}
