using System;
using System.Collections.Generic;
using System.Text;

namespace EMS01.Foundation.Tools
{
    public class SelectParameter
    {

        /// <summary>
        /// 是否排序
        /// </summary>
        public bool Sort { get; set; }
        /// <summary>
        /// 起始
        /// </summary>
        public int Start { get; set; }
        /// <summary>
        /// 个数
        /// </summary>
        public int Length { get; set; }

        public string? searchValue { get; set; }
    }
}
