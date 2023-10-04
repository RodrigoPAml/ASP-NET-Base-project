using System.Linq.Expressions;

namespace Domain.Query
{
    /// <summary>
    /// Represents a filter for a generic entity type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Filter<T> where T : class, new()
    {
        private Expression<Func<T, bool>> _filter = null;

        public Filter(Expression<Func<T, bool>> filter = null)
        {
            _filter = filter;

            if (filter == null)
                _filter = x => true;
        }

        public void And(Expression<Func<T, bool>> filter)
        {
            if(filter != null)
            {
                Expression body = Expression.AndAlso(filter.Body, _filter.Body);
                _filter = Expression.Lambda<Func<T, bool>>(body, filter.Parameters);
            }
            else
                _filter = filter;
        }

        public void Or(Expression<Func<T, bool>> filter)
        {
            if (filter != null)
            {
                Expression body = Expression.OrElse(filter.Body, _filter.Body);

                _filter = Expression.Lambda<Func<T, bool>>(body, filter.Parameters);
            }
            else
                _filter = filter;
        }

        public Expression<Func<T, bool>> GetExpression()
        {
            return _filter;
        }

        public override string ToString()
        {
            return _filter.Body.ToString();
        }
    }
}
