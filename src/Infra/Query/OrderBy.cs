using API.Infra.Base;
using System.Linq.Expressions;

namespace API.Infra.Query
{
    /// <summary>
    /// Represents an order by for a generic entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrderBy<T> where T : Entity
    {
        private Expression<Func<T, object>> _order = null;

        private bool _ascending = true;

        public OrderBy(Expression<Func<T, object>> expression, bool ascending)
        {
            _order = expression;
            _ascending = ascending;

            if (expression == null)
                _order = (x => x.Id);
        }

        public void SetField(Expression<Func<T, object>> expression, bool ascending)
        {
            _order = expression;
            _ascending = ascending;

            if (expression == null)
                _order = (x => x.Id);
        }

        public string GetName()
        {
            var expression = (MemberExpression)_order.Body;

            return expression.Member.Name.ToLower();
        }

        public Expression<Func<T, object>> GetExpression()
        {
            return _order;
        }

        public bool Ascending() => _ascending;
    }
}
