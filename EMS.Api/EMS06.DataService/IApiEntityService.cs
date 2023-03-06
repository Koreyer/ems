using EMS01.Foundation.Data;
using EMS01.Foundation.Tools;
using EMS02.EntityModels.EM02.LoginBusiness;
using EMS05.ModelsDTO.EMD02.LoginBusinessDTO;
using Foundation.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EMS06.DataService
{
    public interface IApiEntityService<Tid,TEntity, TApiEntity>
        where TEntity : class,IData<Tid>, new()
        where TApiEntity : class, IData<Tid>, new()
    {

        Task<ResultData<TEntity>> SetAndSaveAsync(TApiEntity entity);
        Task<ResultData<TEntity>> OverSetAndSaveAsync(TEntity entity);
        Task<List<TApiEntity>> GetBoAsync(Expression<Func<TEntity, bool>> predicateExpression, params Expression<Func<TEntity, object>>[] includePropertiesExpression);
        Task<List<TApiEntity>> GetBoSearchAsync(SelectParameter selectParameter, Expression<Func<TEntity, bool>> predicateExpression, params Expression<Func<TEntity, object>>[] includePropertiesExpression);
        Task<TEntity> GetOneBoAsync(Expression<Func<TEntity, bool>> predicateExpression, params Expression<Func<TEntity, object>>[] includePropertiesExpression);
        Task<TOther> GetOtherBoAsync<TOther>(Guid id) where TOther : class, IData<Tid>, new();
        Task<ResultData<TEntity>> DeleteBoAsync(Tid id);
        Task<ResultData<TEntity>> DeleteRangeAsync(List<Tid> ids);
    }
}
