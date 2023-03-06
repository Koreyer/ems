using EMS01.Foundation.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS05.ModelsDTO.EMD01.SystemDTO
{
    /// <summary>
    /// 设备表
    /// </summary>
    public class EquipmentDTO : Data<Guid>
    {
        /// <summary>
        /// 设备购买时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 设备类别
        /// </summary>
        public string EquipmentCategoryId { get; set; }
        public string? EquipmentCategoryName { get; set; }
        public string? SituationEnumName { get; set; }
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
