using EMS.Models;
using EMS01.Foundation.Data;
using EMS01.Foundation.Tools;
using EMS02.EntityModels.EM02.LoginBusiness;
using Foundation.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository
{
    public interface IRepository<Tid,TEntity> where TEntity : class , IData<Tid>, new()
    {
        #region 增删改查
        Task<ResultData<TEntity>> SetAndSaveAsync(TEntity entity);
        Task<List<TEntity>> GetBoAsync(Expression<Func<TEntity, bool>> predicateExpression, params Expression<Func<TEntity, object>>[] includePropertiesExpression);
        Task<List<TEntity>> GetBoSearchAsync(SelectParameter selectParameter, Expression<Func<TEntity, bool>> predicateExpression, params Expression<Func<TEntity, object>>[] includePropertiesExpression);
        Task<TOther> GetOtherBoAsync<TOther>(Guid id) where TOther : class, IData<Tid>, new();
        Task<TEntity> GetOneBo(Expression<Func<TEntity, bool>> predicateExpression, params Expression<Func<TEntity, object>>[] includePropertiesExpression);
        
        Task<ResultData<TEntity>> DeleteBoAsync(Tid id);
        Task<ResultData<TEntity>> DeleteRangeAsync(List<Tid> ids);

        #endregion


    }
}
