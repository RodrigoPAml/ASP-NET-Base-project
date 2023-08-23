using API.Infra.Exceptions;
using AutoMapper.Internal;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Infra.Query
{
    /// <summary>
    /// Represents a user filter passed to a controller for a generic entity
    /// </summary>
    public class UserFilter
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("operation")]
        public string Operation { get; set; }

        public static List<UserFilter> Interpret(string? filter)
        {
            try
            {
                if (filter == null || filter.Count() == 0)
                    return null;

                if (filter.Length > 2048)
                    throw new BusinessException("Internal error: Filter exceeds the size limit");

                var filters = JsonSerializer.Deserialize<List<UserFilter>>(filter);

                if (filters == null)
                    throw new Exception("Fail to interpret filters");

                return filters;
            }
            catch(Exception e)
            {
                throw new BusinessException($"Internal error: Can't interpret recieved filters: {e?.Message}");
            }
        }

        public static void Validate<T>(List<UserFilter> filters, Fields<T> allowedFields) where T : class
        {
            if (filters == null || filters.Count == 0)
                return;

            if (allowedFields == null || allowedFields.Count() == 0)
                return;

            var allowedFieldsNames = allowedFields.GetNames();

            List<string> operators = new List<string>() { "=", "!=", ">", ">=", "<", "<=", "in" };

            foreach (var filter in filters)
            {
                if (!allowedFieldsNames.Contains(filter.Field.ToLower()))
                    throw new BusinessException("Filter field invalid or not allowed");

                if (!operators.Contains(filter.Operation))
                    throw new BusinessException("Filter operation invalid or not allowed");
            }
        }

        public static void Compose<T>(List<UserFilter> userFilters, Filter<T> filter) where T : class
        {
            try
            {
                if (userFilters == null || userFilters.Count == 0)
                    return;

                var parameter = Expression.Parameter(typeof(T), "x");

                foreach (var userFilter in userFilters)
                {
                    Type type = typeof(T)
                        .GetProperties()
                        .Where(x => x.Name.ToLower() == userFilter.Field.ToLower())
                        .FirstOrDefault()
                    .PropertyType;

                    ConstantExpression right = null;

                    if (type.IsEnum)
                    {
                        var enumValue = Int32.Parse(userFilter.Value);
                        object selectedEnumValue = Enum.ToObject(type, enumValue);
                        right = Expression.Constant(selectedEnumValue, type);
                    }
                    else
                    {
                        right = Expression.Constant(Convert.ChangeType(userFilter.Value, type), type);
                    }

                    var left = Expression.PropertyOrField(parameter, userFilter.Field);
                    BinaryExpression binaryExpression = null;

                    switch (userFilter.Operation)
                    {
                        case "=":
                            binaryExpression = Expression.Equal(left, right);
                            break;
                        case "!=":
                            binaryExpression = Expression.NotEqual(left, right);
                            break;
                        case ">":
                            binaryExpression = Expression.GreaterThan(left, right);
                            break;
                        case ">=":
                            binaryExpression = Expression.GreaterThanOrEqual(left, right);
                            break;
                        case "<":
                            binaryExpression = Expression.LessThan(left, right);
                            break;
                        case "<=":
                            binaryExpression = Expression.LessThanOrEqual(left, right);
                            break;
                        case "in":
                            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                            var call = Expression.Call(left, method, right);

                            filter.And(Expression.Lambda<Func<T, bool>>(call, parameter));
                            break;
                    }

                    if (binaryExpression != null)
                        filter.And(Expression.Lambda<Func<T, bool>>(binaryExpression, parameter));
                }
            }
            catch(Exception)
            {
                throw new BusinessException("Internal error in filter composition");
            }
        }
    }
}
