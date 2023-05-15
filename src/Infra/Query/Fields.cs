using Microsoft.EntityFrameworkCore.Query;
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
                if (!_fields.Any(x => Equals(x, field)))
                    _fields.Add(field);
            }
        }

        public List<string> GetNames(bool lower = true)
        {
            List<string> names = new List<string>();

            foreach (var field in _fields)
            {
                if (field.Body.NodeType == ExpressionType.Convert)
                {
                    var operand = ((UnaryExpression)field.Body).Operand;
                    var member = (MemberExpression)operand;
                    names.Add(lower ? member.Member.Name.ToLower() : member.Member.Name);
                }
                else
                {
                    var expression = (MemberExpression)field.Body;
                    names.Add(lower ? expression.Member.Name.ToLower() : expression.Member.Name);
                }
            }

            return names;
        }

        public void AddField(Expression<Func<T, object>> field)
        {
            if (field == null)
                return;

            if (!_fields.Any(x => Equals(x, field)))
                _fields.Add(field);
        }

        public bool ContainsField(Expression<Func<T, object>> field)
        {
            return _fields.Any(x => Equals(x, field));
        }

        public int Count()
        {
            return _fields.Count();
        }

        public List<Expression<Func<T, object>>> GetFields()
        {
            return _fields;
        }

        static private bool Equals(Expression<Func<T, object>> left, Expression<Func<T, object>> right)
        {
            Func<Expression, Expression, bool> equals = ExpressionEqualityComparer.Instance.Equals;

            return equals(left, right);
        }
    }
}
