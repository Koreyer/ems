using EMS01.Foundation.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS05.ModelsDTO.EMD01.SystemDTO
{
    /// <summary>
    /// 设备报废表
    /// </summary>
    public class EquipmentScrapDTO : Data<Guid>
    {
        /// <summary>
        /// 报废设备
        /// </summary>
        public string EquipmentId { get; set; }
        public string? EquipmentName { get; set; }
        public string EquipmentCategoryName { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
