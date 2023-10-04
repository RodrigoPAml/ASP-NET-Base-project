using System.Linq.Expressions;

namespace Domain.Query
{
    /// <summary>
    /// Represents a selection of an entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Select<T> where T : class
    {
        private Expression<Func<T, dynamic>> _select;

        public Select(Expression<Func<T, dynamic>> select)
        {
            _select = select;
        }

        public Expression<Func<T, dynamic>> GetSelect()
        {
            return _select;
        }

        public void SetSelect(Expression<Func<T, dynamic>> select)
        {
            _select = select;
        }
    }
}
