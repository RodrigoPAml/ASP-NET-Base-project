using Domain.Exceptions;
using Domain.Query;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Query
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

        public static Filter<T> Compose<T>(string filterStr, Fields<T> allowedFields) where T : class, new()
        {
            var filters = Interpret(filterStr);

            if (filters == null || filters.Count == 0)
                return null;

            if (allowedFields == null || allowedFields.Count() == 0)
                return null;

            var allowedFieldsNames = allowedFields.GetNames();

            List<string> operators = new List<string>() { "=", "!=", ">", ">=", "<", "<=", "in" };

            foreach (var filter in filters)
            {
                if (!allowedFieldsNames.Contains(filter.Field.ToLower()))
                    throw new BusinessException("Filter field invalid or not allowed");

                if (!operators.Contains(filter.Operation))
                    throw new BusinessException("Filter operation invalid or not allowed");
            }

            return Mount<T>(filters);
        }

        private static Filter<T> Mount<T>(List<UserFilter> userFilters) where T : class, new()
        {
            try
            {
                Filter<T> filter = new Filter<T>();

                if (userFilters == null || userFilters.Count == 0)
                    return null;

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

                    // Nullable.Value adaption
                    if (left.Type.IsGenericType && left.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        left = Expression.Property(left, "Value");

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

                return filter;
            }
            catch (Exception)
            {
                throw new BusinessException("Internal error in filter composition");
            }
        }

        private static Type GetUnderlyingType(Type nullableType)
        {
            if (nullableType.IsGenericType && nullableType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return Nullable.GetUnderlyingType(nullableType);
            }
            else
            {
                return nullableType;
            }
        }
    }
}
