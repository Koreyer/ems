using EMS01.Foundation.Data;
using System;

namespace EMS.Models
{
    /// <summary>
    /// 设备报废表
    /// </summary>
    public class EquipmentScrap : Data<Guid>
    {
        /// <summary>
        /// 报废设备
        /// </summary>
        public Equipment Equipment { get; set; }
        public DateTime CreateTime { get;set; }
    }
}
