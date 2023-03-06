using AutoMapper;
using DataRepository;
using EMS01.Foundation.Data;
using EMS01.Foundation.Tools;
using Foundation.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EMS06.DataService
{

    public class ApiEntityService<Tid,TEntity, TApiEntity> : IApiEntityService<Tid,TEntity, TApiEntity>
        where TEntity : class,IData<Tid>, new()
        where TApiEntity : class, IData<Tid>, new()
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Tid,TEntity> _repository;
        public ApiEntityService(IMapper mapper, IRepository<Tid,TEntity> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        #region 增删改查
        public  async Task<ResultData<TEntity>> SetAndSaveAsync(TApiEntity entity)
        {
            return await _repository.SetAndSaveAsync(_mapper.Map<TEntity>(entity));
        }
        public async Task<ResultData<TEntity>> OverSetAndSaveAsync(TEntity entity)
        {
            return await _repository.SetAndSaveAsync(entity);
        }
        public  async Task<List<TApiEntity>> GetBoAsync(Expression<Func<TEntity, bool>> predicateExpression, params Expression<Func<TEntity, object>>[] includePropertiesExpression)
        {
            var res = await _repository.GetBoAsync(predicateExpression, includePropertiesExpression);
            return _mapper.Map<List<TApiEntity>>(res.ToList());
        }
        public async Task<TEntity> GetOneBoAsync(Expression<Func<TEntity, bool>> predicateExpression, params Expression<Func<TEntity, object>>[] includePropertiesExpression)
        {
            var res = await _repository.GetOneBo(predicateExpression, includePropertiesExpression);
            return res;
        }
        public  async Task<List<TApiEntity>> GetBoSearchAsync(SelectParameter selectParameter, Expression<Func<TEntity, bool>> predicateExpression, params Expression<Func<TEntity, object>>[] includePropertiesExpression)
        {
            
            var res = await _repository.GetBoSearchAsync(selectParameter, predicateExpression, includePropertiesExpression);
            return _mapper.Map<List<TApiEntity>>(res.ToList());
        }
        public async Task<TOther> GetOtherBoAsync<TOther>(Guid id)
            where TOther : class, IData<Tid>, new()
        {
            return await _repository.GetOtherBoAsync<TOther>(id);
        }

        public  async Task<ResultData<TEntity>> DeleteBoAsync(Tid id)
        {
            return await _repository.DeleteBoAsync(id);
        }
        public  async Task<ResultData<TEntity>> DeleteRangeAsync(List<Tid> ids)
        {
            return await _repository.DeleteRangeAsync(ids);
        }

        #endregion


    }
}
