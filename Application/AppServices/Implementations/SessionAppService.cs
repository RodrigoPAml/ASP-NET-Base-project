using Application.AppServices.Interfaces;
using Application.Mapper;
using Application.Models.NewEntity;
using Application.Models.UpdatedEntity;
using Application.Query;
using Application.Responses;
using Domain.Exceptions;
using Domain.Models.Entities;
using Domain.Persistance;
using Domain.Query;
using Domain.Services.Interfaces;

namespace Application.AppServices.Implementations
{
    public class SessionAppService : ISessionAppService
    {
        private ISessionService _service;

        private IDatabaseTransaction _transaction;

        private IServiceProvider _provider;

        public SessionAppService(ISessionService service, IDatabaseTransaction transaction, IServiceProvider provider)
        {
            _service = service;
            _transaction = transaction;
            _provider = provider;
        }

        public ResponseBody GetPaged(uint page, uint pageSize, string filters, string orderBy)
        {
            try
            {
                // Fields allowed to get filtered from front-end
                Fields<Session> allowedFields = new Fields<Session>();
                allowedFields.AddAllFields();

                var filter = UserFilter.Compose(filters, allowedFields);
                var order = UserOrderBy.Compose(orderBy, allowedFields);

                var list = _service.GetPaged(
                    page,
                    pageSize,
                    filter,
                    order,
                    new Select<Session>(x => new
                    {
                        x.Id,
                        x.MovieId,
                        Movie = new
                        {
                            x.Movie.Name,
                        },
                        Date = x.Date.ToString("dd/MM/yyyy"),
                    })
                );

                return ResponseBody.WithContentSuccess("Records retrieved successfully", list);
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
        }

        public ResponseBody Get(ulong id)
        {
            try
            {
                var user = _service.Get(
                    id,
                    new Select<Session>(x => new
                    {
                        x.Id,
                        x.MovieId,
                        Movie = new
                        {
                            x.Movie.Name,
                        },
                        x.Date
                    })
                );

                if (user == null)
                    throw new BusinessException("Session not found");

                return ResponseBody.WithContentSuccess("Updated with success", user);
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
        }

        public ResponseBody Create(NewSession newEntity)
        {
            try
            {
                var entity = ClassMapper.Map<Session>(newEntity);

                if (entity == null)
                    throw new InternalException($"Mapping failure between {nameof(Session)} and {nameof(NewSession)}");

                _transaction.Begin();

                _service.Create(entity);

                _transaction.Save();
                _transaction.Commit();

                return ResponseBody.WithContentSuccess("Updated with success", entity.Id);
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
        }

        public ResponseBody Update(UpdatedSession updatedEntity)
        {
            try
            {
                Fields<Session> fields = new Fields<Session>();
                fields.AddAllFieldsExcept<UpdatedSession>(x => x.Id);

                var entity = ClassMapper.Map<Session>(updatedEntity);

                if (entity == null)
                    throw new InternalException($"Mapping failure between {nameof(Session)} and {nameof(UpdatedSession)}");

                _transaction.Begin();
                _service.Update(entity, fields);
                _transaction.Save();
                _transaction.Commit();

                return ResponseBody.NoContentSuccess("Updated with success");
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
        }

        public ResponseBody Delete(ulong id)
        {
            try
            {
                _transaction.Begin();

                _service.Delete(id);

                _transaction.Save();
                _transaction.Commit();

                return ResponseBody.NoContentSuccess("Deleted with success");
            }
            catch (BusinessException be)
            {
                return ResponseBody.HandledError(be);
            }
            catch (Exception e)
            {
                return ResponseBody.UnhandledError(e);
            }
        }
    }
}
