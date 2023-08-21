using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace API.Infra.Query
{
    /// <summary>
    /// Represents the fields of a generic entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Fields<T> where T : class
    {
        private List<Expression<Func<T, object>>> _fields = new List<Expression<Func<T, object>>>();

        public Fields(params Expression<Func<T, object>>[] fields)
        {
            if (fields == null || fields.Count() == 0)
                return;

            foreach (var field in fields)
            {
                if (!_fields.Any(x => IsEquals(x, field)))
                    _fields.Add(field);
            }
        }

        static private string GetName(Expression<Func<T, object>> field, bool lower = true)
        {
            if (field.Body.NodeType == ExpressionType.Convert)
            {
                var operand = ((UnaryExpression)field.Body).Operand;
                var member = (MemberExpression)operand;
                return (lower ? member.Member.Name.ToLower() : member.Member.Name);
            }
            else
            {
                var expression = (MemberExpression)field.Body;
                return (lower ? expression.Member.Name.ToLower() : expression.Member.Name);
            }
        }

        public List<string> GetNames(bool lower = true)
        {
            List<string> names = new List<string>();

            foreach (var field in _fields)
            {
                names.Add(GetName(field, lower));
            }

            return names;
        }

        public void AddField(Expression<Func<T, object>> field)
        {
            if (field == null)
                return;

            if (!_fields.Any(x => IsEquals(x, field)))
                _fields.Add(field);
        }

        public void RemoveField(Expression<Func<T, object>> field)
        {
            if (field == null) 
                return;

            _fields.RemoveAll(x => IsEquals(x, field));
        }

        public void AddAllFields()
        {
            AddAllFieldsExcept();
        }

        public void AddAllFieldsExcept(params Expression<Func<T, object>>[] ignoreFields)
        {
            var allProperties = typeof(T).GetProperties();

            foreach (var property in allProperties)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var propertyExpression = Expression.Property(parameter, property);
                var lambdaExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(propertyExpression, typeof(object)), parameter);

                bool isEqual = ignoreFields.Any(x => IsEquals(x, lambdaExpression));

                 if (!_fields.Any(x => IsEquals(x, lambdaExpression)) && !isEqual)
                    _fields.Add(lambdaExpression);
            }
        }

        public void AddAllFieldsExcept<M>(params Expression<Func<T, object>>[] ignoreFields) where M : class
        {
            var modelProperties = typeof(M)
                .GetProperties()
                .Select(x => x.Name)
                .ToList();

            var allProperties = typeof(T).GetProperties();

            foreach (var property in allProperties)
            {
                if (!modelProperties.Contains(property.Name))
                    continue;

                var parameter = Expression.Parameter(typeof(T), "x");
                var propertyExpression = Expression.Property(parameter, property);
                var lambdaExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(propertyExpression, typeof(object)), parameter);

                if (!_fields.Any(x => IsEquals(x, lambdaExpression)) && !ignoreFields.Any(x => IsEquals(x, lambdaExpression)))
                    _fields.Add(lambdaExpression);
            }
        }

        public void AddFields<M>() where M : class
        {
            AddAllFieldsExcept<M>();
        }

        public void RemoveAllFields()
        {
            _fields.Clear();
        }

        public bool ContainsField(Expression<Func<T, object>> field)
        {
            return _fields.Any(x => IsEquals(x, field));
        }

        public int Count()
        {
            return _fields.Count();
        }

        public List<Expression<Func<T, object>>> GetFields()
        {
            return _fields;
        }

        static private bool IsEquals(Expression<Func<T, object>> left, Expression<Func<T, object>> right)
        {
            return GetName(left) == GetName(right);
        }
    }
}
