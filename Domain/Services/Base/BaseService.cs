using Domain.Enums.System;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Models.Entities.Base;
using Domain.Persistance;
using Domain.Query;
using Domain.Repositories.Base;
using Domain.Responses;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Domain.Services.Base
{
    /// <summary>
    /// Base service class with common operations and validations service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseService<T> where T : Entity, new()
    {
        protected readonly IRepository<T> _repo;
        
        protected readonly IServiceProvider _provider;

        protected readonly IDatabaseTransaction _transaction;

        private List<Action<T, ActionTypeEnum, object>> _addFuncs = new List<Action<T, ActionTypeEnum, object>>();
        private List<Action<T, ActionTypeEnum, object>> _updateFuncs = new List<Action<T, ActionTypeEnum, object>>();
        private List<Action<T, ActionTypeEnum, object>> _deleteFuncs = new List<Action<T, ActionTypeEnum, object>>();

        public BaseService(IRepository<T> repo, IServiceProvider provider)
        {
            _repo = repo;
            _transaction = provider.GetService<IDatabaseTransaction>();
            _provider = provider;
        }

        public IRepository<T> GetRepository()
        {
            return _repo;
        }

        public void InvokeAction(T entity, ActionTypeEnum type, object additionalData = null)
        {
            switch (type)
            {
                case ActionTypeEnum.Create:
                    foreach (var action in _addFuncs)
                        action.Invoke(entity, type, additionalData);
                    break;
                case ActionTypeEnum.Update:
                    foreach (var action in _updateFuncs)
                        action.Invoke(entity, type, additionalData);
                    break;
                case ActionTypeEnum.Delete:
                    foreach (var action in _deleteFuncs)
                        action.Invoke(entity, type, additionalData);
                    break;
            }
        }

        public void AddAction(Action<T, ActionTypeEnum, object> action, ActionTypeEnum type)
        {
            if (action == null)
                return;

            switch(type)
            {
                case ActionTypeEnum.Create:
                    if(!_addFuncs.Contains(action))
                        _addFuncs.Add(action);
                    break;
                case ActionTypeEnum.Update:
                    if (!_updateFuncs.Contains(action))
                        _updateFuncs.Add(action);
                    break;
                case ActionTypeEnum.Delete:
                    if (!_deleteFuncs.Contains(action))
                        _deleteFuncs.Add(action);
                    break;
            }
        }

        public void Create(T entity, object additionalData = null) 
        {
            if (entity == null)
                throw new InternalException($"Create null entity '{typeof(T).GetTypeDescription()}' is not allowed");

            foreach (var action in _addFuncs)
                action.Invoke(entity, ActionTypeEnum.Create, additionalData);

            _repo.Create(entity);
        }

        public void Update(T entity, Fields<T> fieldsToUpdate, object additionalData = null)
        {
            if (entity == null)
                throw new InternalException($"Update null entity '{typeof(T).GetTypeDescription()}' is not allowed permitido");

            if (fieldsToUpdate == null || fieldsToUpdate.Count() == 0)
                throw new BusinessException($"No modifications detected");

            if (!_repo.Any(x => x.Id == entity.Id))
                throw new BusinessException($"{entity.GetDescription()} not found");

            foreach (var action in _updateFuncs)
                action.Invoke(entity, ActionTypeEnum.Update, additionalData);

            _repo.Update(entity, fieldsToUpdate);
        }

        public void Delete(ulong id, object additionalData = null)
        {
            var filter = new Filter<T>(x => x.Id == id);

            if (!_repo.Any(filter))
                return;

            var entity = new T()
            {
                Id = id
            };

            foreach (var action in _deleteFuncs)
                action.Invoke(entity, ActionTypeEnum.Delete, additionalData);

            _repo.Delete(entity);
        }
     
        public List<dynamic> Get(Filter<T> filter, Select<T> select) 
        {
            return _repo
                .Where(filter)
                .Select(select.GetSelect())
                .ToList();
        }

        public List<dynamic> Get(Expression<Func<T, bool>> filter, Select<T> select)
        {
            return _repo
                .Where(filter)
                .Select(select.GetSelect())
                .ToList();
        }

        public PagedData GetPaged(uint page, uint pageSize, Filter<T> filter, Select<T> select, OrderBy<T> orderBy = null)
        {
            if (page == 0 || pageSize == 0)
                throw new BusinessException("Invalid pagination arguments");

            int rowsToSkip = (int)((page - 1) * pageSize);

            var partialFilter = _repo
                .Where(filter);

            var order = orderBy != null ? orderBy.GetExpression() : (x => x.Id);
            bool asc = orderBy != null ? orderBy.Ascending() : true;

            var partialOrderBy = partialFilter.OrderBy(order);

            if(!asc)
                partialOrderBy = partialFilter.OrderByDescending(order);

            var list = partialOrderBy
                .Skip(rowsToSkip)
                .Take((int)pageSize)
                .Select(select.GetSelect())
                .ToList();

            var count = _repo.Where(filter).Count();

            return new PagedData()
            {
                Data = list,
                Page = page,
                PageSize = pageSize,
                TotalRegisters = count
            };
        }
    }
}
