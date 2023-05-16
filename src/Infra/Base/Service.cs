using API.Infra.Database;
using API.Infra.Enums;
using API.Infra.Exceptions;
using API.Infra.Extensions;
using API.Infra.Query;
using API.Infra.Responses;
using System.Linq.Expressions;

namespace API.Infra.Base
{
    /// <summary>
    /// Base service class with common operations and validations service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseService<T> where T : Entity, new()
    {
        protected readonly Repository<T> _repo;

        protected readonly DataBaseTransaction _transaction;

        protected readonly IServiceProvider _provider;

        private List<Action<T, ActionTypeEnum, object>> _addFuncs = new List<Action<T, ActionTypeEnum, object>>();
        private List<Action<T, ActionTypeEnum, object>> _updateFuncs = new List<Action<T, ActionTypeEnum, object>>();
        private List<Action<T, ActionTypeEnum, object>> _deleteFuncs = new List<Action<T, ActionTypeEnum, object>>();

        public BaseService(Repository<T> repo, IServiceProvider provider)
        {
            _repo = repo;
            _provider = provider;
            _transaction = provider.GetService<DataBaseTransaction>();
        }

        public Repository<T> GetRepository()
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

        public void Create(T entity, object actionInfo = null) 
        {
            if (entity == null)
                throw new InternalException($"Create null entity '{typeof(T).GetTypeDescription()}' is not allowed");

            _transaction.Begin();

            foreach (var action in _addFuncs)
                action.Invoke(entity, ActionTypeEnum.Create, actionInfo);

            _repo.Create(entity);

            _transaction.Save();
            _transaction.Commit();
        }

        public void Update(T entity, Fields<T> fieldsToUpdate, object actionInfo = null)
        {
            if (entity == null)
                throw new InternalException($"Update null entity '{typeof(T).GetTypeDescription()}' is not allowed permitido");

            if (fieldsToUpdate == null || fieldsToUpdate.Count() == 0)
                throw new BusinessException($"No modifications detected");

            _transaction.Begin();

            foreach (var action in _updateFuncs)
                action.Invoke(entity, ActionTypeEnum.Update, actionInfo);

            _repo.Update(entity, fieldsToUpdate);

            _transaction.Save();
            _transaction.Commit();
        }

        public void Delete(ulong id, object actionInfo = null)
        {
            var filter = new Filter<T>(x => x.Id == id);

            if (!_repo.Any(filter))
                return;

            var entity = new T()
            {
                Id = id
            };

            _transaction.Begin();

            foreach (var action in _deleteFuncs)
                action.Invoke(entity, ActionTypeEnum.Delete, actionInfo);

            _repo.Delete(entity);

            _transaction.Save();
            _transaction.Commit();
        }
     
        public List<dynamic> Get(Filter<T> filter, Expression<Func<T, dynamic>> select) 
        {
            return _repo
                .Where(filter)
                .Select(select)
                .ToList();
        }

        public List<dynamic> Get(Expression<Func<T, bool>> filter, Expression<Func<T, dynamic>> select)
        {
            return _repo
                .Where(filter)
                .Select(select)
                .ToList();
        }

        public PagedData GetPaged(uint page, uint pageSize, Filter<T> filter, Expression<Func<T, dynamic>> select, OrderBy<T> orderBy = null, Expression<Func<T, bool>> additional = null)
        {
            if (page == 0 || pageSize == 0)
                throw new BusinessException("Invalid pagination arguments");

            int rowsToSkip = (int)((page - 1) * pageSize);

            var partialFilter = _repo
                .Where(filter);

            if(additional != null)
                partialFilter = partialFilter.Where(additional);

            var order = orderBy != null ? orderBy.GetExpression() : (x => x.Id);
            bool asc = orderBy != null ? orderBy.Ascending() : true;

            var partialOrderBy = partialFilter.OrderBy(order);

            if(!asc)
                partialOrderBy = partialFilter.OrderByDescending(order);

            var list = partialOrderBy
                .Skip(rowsToSkip)
                .Take((int)pageSize)
                .Select(select)
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
