using Domain.Exceptions;
using Domain.Models.Entities.Base;
using Domain.Query;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Query
{
    /// <summary>
    /// Represents a order by passed to a controller by the user for a generic entity
    /// </summary>
    public class UserOrderBy
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }

        [JsonPropertyName("ascending")]
        public bool Ascending { get; set; }

        public static UserOrderBy Interpret(string? orderBy)
        {
            try
            {
                if (orderBy == null || orderBy.Count() == 0)
                    return null;

                if (orderBy.Length > 128)
                    throw new BusinessException("Internal error: Informed ordenation exceeds size limit");

                var order = JsonSerializer.Deserialize<UserOrderBy>(orderBy);

                if (order == null)
                    throw new Exception();

                return order;
            }
            catch(Exception)
            {
                throw new BusinessException("Internal error: Can't interpret recieved ordenation");
            }
        }

        private static void Validate<T>(UserOrderBy orderBy, Fields<T> allowedFields) where T : class
        {
            if (orderBy == null)
                return;

            if (allowedFields == null || allowedFields.Count() == 0)
                return;

            var allowedFieldsNames = allowedFields.GetNames(false);

            if (!allowedFieldsNames.Contains(orderBy.Field))
                throw new BusinessException("Internal error: Ordenation by this fields is invalid or not allowed");
        }

        public static OrderBy<T> Compose<T>(string orderByStr, Fields<T> allowedFields) where T : Entity
        {
            try
            {
                var orderBy = Interpret(orderByStr);

                Validate(orderBy, allowedFields);

                if (orderBy == null)
                    return new OrderBy<T>(null, true);

                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, orderBy.Field);

                Expression conversion = Expression.Convert(property, typeof(object));

                var exp = Expression.Lambda<Func<T, object>>(conversion, parameter);

                return new OrderBy<T>(exp, orderBy.Ascending);
            }
            catch(BusinessException)
            {
                throw;
            }
            catch(Exception)
            {
                throw new BusinessException("Internal error in ordenation composition");
            }
        }
    }
}
