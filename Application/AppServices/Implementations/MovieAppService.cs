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
    public class MovieAppService : IMovieAppService
    {
        private IMovieService _service;

        private IDatabaseTransaction _transaction;

        private IServiceProvider _provider;

        public MovieAppService(IMovieService service, IDatabaseTransaction transaction, IServiceProvider provider)
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
                Fields<Movie> allowedFields = new Fields<Movie>();
                allowedFields.AddAllFields();

                var filter = UserFilter.Compose(filters, allowedFields);
                var order = UserOrderBy.Compose(orderBy, allowedFields);

                var list = _service.GetPaged(
                    page,
                    pageSize,
                    filter,
                    order,
                    new Select<Movie>(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Synopsis,
                        x.Genre,
                        x.Duration
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
                    new Select<Movie>(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Synopsis,
                        x.Genre,
                        x.Duration
                    })
                );

                if (user == null)
                    throw new BusinessException("Movie not found");

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

        public ResponseBody Create(NewMovie newEntity)
        {
            try
            {
                var entity = ClassMapper.Map<Movie>(newEntity);

                if (entity == null)
                    throw new InternalException($"Mapping failure between {nameof(Movie)} and {nameof(NewMovie)}");

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

        public ResponseBody Update(UpdatedMovie updatedEntity)
        {
            try
            {
                Fields<Movie> fields = new Fields<Movie>();
                fields.AddAllFieldsExcept<UpdatedMovie>(x => x.Id);

                var entity = ClassMapper.Map<Movie>(updatedEntity);

                if (entity == null)
                    throw new InternalException($"Mapping failure between {nameof(Movie)} and {nameof(UpdatedMovie)}");

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
