using EMS01.Foundation.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Tools
{
    public class ResultData<TEntity>:IResultData<TEntity> where TEntity : class , new()
    {
        public int? Code { get; set; }
        public string Message { get; set; }
        public ResultEnum ResultEnum { get; set; }
        public string? ErrorMessage { get; set; }
        public List<TEntity>? Data { get; set; }
    }
}
