using API.Infra.Query;
using API.Infra.Base;
using System.Linq.Expressions;

namespace API.Infra.Utils
{
    public static class FieldUtils
    {
        public static bool IsUpdatingField<T>(object @object, Expression<Func<T, object>> field) where T : Entity
        {
            if (@object is Fields<T>)
            {
                Fields<T> fieldsUpdating = (Fields<T>)@object;

                return fieldsUpdating.ContainsField(field);
            }

            return false;
        }
    }
}
