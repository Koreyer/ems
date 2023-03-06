using EMS.Models;
using EMS01.Foundation.Data;
using EMS01.Foundation.Tools;
using EMS01.Foundation.Enum;
using EMS02.EntityModels.EM02.LoginBusiness;
using EMS03.EntityModels.ORM;
using Foundation.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository
{
    public class Repository<Tid,TEntity> : IRepository<Tid,TEntity> where TEntity : class,IData<Tid>, new()
    {
        private readonly EMSContext _context;

        public Repository(EMSContext context)
        {
            _context = context;
        }
        #region 增删改查

        
        public  async Task<ResultData<TEntity>> SetAndSaveAsync(TEntity entity)
        {
            var resultData = new ResultData<TEntity>();
            try
            {
                var boContext = _context.Set<TEntity>();
                if (! await _context.Set<TEntity>().AnyAsync(x => x.Id.Equals(entity.Id)))
                {
                    boContext.Add(entity);
                    resultData.ResultEnum = ResultEnum.新增成功;
                    resultData.Message = "新增成功";

                }
                else
                {
                    boContext.Update(entity);
                    resultData.ResultEnum = ResultEnum.更新成功;
                    resultData.Message = "更新成功";
                }
                
                await _context.SaveChangesAsync();
                resultData.Code = 200;
                
                
                return resultData;
            }
            catch (Exception e)
            {
                resultData.Message = "操作失败";
                resultData.ResultEnum = ResultEnum.操作失败;
                return resultData;
            }
        }
        public  async Task<List<TEntity>> GetBoSearchAsync(SelectParameter selectParameter, Expression<Func<TEntity, bool>> predicateExpression, params Expression<Func<TEntity, object>>[] includePropertiesExpression)
        {

            IQueryable<TEntity> entityBo = _context.Set<TEntity>();

            if (includePropertiesExpression != null)
            {
                foreach (var expression in includePropertiesExpression)
                {
                    entityBo = entityBo.Include(expression);
                }
            }
            if (predicateExpression != null)
                entityBo = entityBo.Where(predicateExpression);
            if(selectParameter != null)
            {
                if (selectParameter.Sort)
                    entityBo = entityBo.OrderBy(x => x.Name);
                else
                    entityBo = entityBo.OrderByDescending(x => x.Name);
                entityBo = entityBo.Skip(selectParameter.Start);
                //如果设置了长度的话那就这样咯
                if (selectParameter.Length > 0)
                {
                    entityBo = entityBo.Take(selectParameter.Length);
                }
                else
                {
                    entityBo = entityBo.Take(100);
                }
                if (!string.IsNullOrEmpty(selectParameter.searchValue))
                {
                    entityBo =  entityBo.Where(x => x.Name == selectParameter.searchValue);
                }
            }
            
            var result = await entityBo.ToListAsync();
            return result.AsQueryable<TEntity>().ToList();
        }
        
        public async Task<List<TEntity>> GetBoAsync(Expression<Func<TEntity, bool>> predicateExpression, params Expression<Func<TEntity, object>>[] includePropertiesExpression)
        {
            IQueryable<TEntity> entityBo =  _context.Set<TEntity>();

            if (includePropertiesExpression != null)
            {
                foreach (var expression in includePropertiesExpression)
                {
                    entityBo = entityBo.Include(expression);
                }
            }
            if (predicateExpression != null)
                entityBo = entityBo.Where(predicateExpression);
            var result =  entityBo;
            return result.AsQueryable<TEntity>().ToList();
        }
        public virtual async Task<TOther> GetOtherBoAsync<TOther>(Guid id)
            where TOther : class, IData<Tid>, new()
        {
            return await _context.Set<TOther>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<TEntity> GetOneBo(Expression<Func<TEntity, bool>> predicateExpression, params Expression<Func<TEntity, object>>[] includePropertiesExpression)
        {
            IQueryable<TEntity> entityBo = _context.Set<TEntity>();
            if (includePropertiesExpression != null)
            {
                foreach (var expression in includePropertiesExpression)
                {
                    entityBo = entityBo.Include(expression);
                }
            }
            if (predicateExpression != null)
                entityBo = entityBo.Where(predicateExpression);

            return await entityBo.FirstOrDefaultAsync();

        }
        public async Task<ResultData<TEntity>> DeleteBoAsync(Tid id)
        {
            ResultData<TEntity> resultData = new ResultData<TEntity>();
            try
            {
                //先找下这个id的数据
                var deleBo =await _context.Set<TEntity>().FirstOrDefaultAsync(x=>x.Id.Equals(id));
                if(deleBo != null)
                {
                    _context.Remove(deleBo);
                    await _context.SaveChangesAsync();
                    resultData.Message = "删除成功";
                    resultData.ResultEnum = ResultEnum.删除成功;

                }
                else
                {
                    resultData.ErrorMessage = "数据不存在";
                    resultData.ResultEnum = ResultEnum.删除失败;
                }

            }
            catch (Exception ex)
            {

                resultData.Message = ex.Message;
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE 语句与 REFERENCE 约束"))
                {
                    resultData.ErrorMessage = "该数据与其他数据之间有关联，不能直接删除";
                }

                resultData.ResultEnum = ResultEnum.操作失败;
            }
            return resultData;
        }
        public async Task<ResultData<TEntity>> DeleteRangeAsync(List<Tid> ids)
        {
            ResultData<TEntity> resultData = new ResultData<TEntity>();
            try
            {
                var deleRange = new List<TEntity>();
                foreach (var id in ids)
                {
                    deleRange.Add(await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id)));
                }
                _context.RemoveRange(deleRange);
                await _context.SaveChangesAsync();
                resultData.Message = "操作成功";
                resultData.ResultEnum = ResultEnum.操作成功;


            }
            catch (Exception ex)
            {

                resultData.ErrorMessage = ex.Message;
                resultData.ResultEnum = ResultEnum.操作失败;
            }
            return resultData;
        }
        #endregion

        

    }
}
