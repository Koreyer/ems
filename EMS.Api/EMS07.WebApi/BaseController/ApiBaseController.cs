using EMS01.Foundation.Data;
using EMS01.Foundation.Tools;
using EMS06.DataService;
using Foundation.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EMS07.WebApi.BaseController
{
    /// <summary>
    /// 一个简单的基控制器，所有控制器都应该继承这个，避免重复编写业务逻辑
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TApiEntity"></typeparam>
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[AllowAnonymous]
    public abstract class ApiBaseController<Tid,TEntity, TApiEntity>:Controller
        where TEntity : class,IData<Tid> , new()
        where TApiEntity : class,IData<Tid>, new()
       
    {
        private readonly IApiEntityService<Tid,TEntity, TApiEntity> _apiEntityService;
        private Expression<Func<TEntity, object>>[] _expressions;
        private Expression<Func<TEntity, bool>> _predicateExpressions;


        public ApiBaseController(IApiEntityService<Tid,TEntity, TApiEntity> apiEntityService)
        {
            _apiEntityService = apiEntityService;
        }
        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual async Task<int> GetBoCount()
        {
            var result = await _apiEntityService.GetBoAsync(null, null);
            return result.Count;
        }
        #region CRUD
        [HttpPost]
        public virtual async Task<ResultData<TEntity>> Update(TApiEntity entity)
        {
            var res = await _apiEntityService.SetAndSaveAsync(entity);
            return res;
        }
        //[HttpGet]
        //public virtual async Task<List<TApiEntity>> Get(Expression<Func<TEntity, bool>> predicateExpression, params Expression<Func<TEntity, object>>[] includePropertiesExpression)
        //{
        //    return await _apiEntityService.GetBoAsync(predicateExpression,includePropertiesExpression);
        //}
        [HttpPost]
        public virtual async Task<List<TApiEntity>> GetBySearch( SelectParameter selectParameter)
        {
            return await _apiEntityService.GetBoSearchAsync(selectParameter, _predicateExpressions, _expressions);
        }
        [HttpDelete]
        public virtual async Task<ResultData<TEntity>> Delete(Tid id)
        {
            return await _apiEntityService.DeleteBoAsync(id);
        }
        [HttpPost]
        public virtual async Task<ResultData<TEntity>> DeleteRange(List<Tid> ids)
        {
            return await _apiEntityService.DeleteRangeAsync(ids);
        }
        #endregion

        #region 辅助
        /// <summary>
        /// 关联关系
        /// </summary>
        /// <param name="expressions"></param>
        protected void SetIncludeExpressions(params Expression<Func<TEntity, object>>[] expressions) => this._expressions = expressions;
        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="expression"></param>
        protected void SetPredicateExpressions(Expression<Func<TEntity, bool>> expression) => this._predicateExpressions = expression;
        #endregion
    }
}
