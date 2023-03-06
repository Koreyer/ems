using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Tools
{
    internal interface IResultData<TEntity>
        where TEntity : class ,new()
    {
    }
}
