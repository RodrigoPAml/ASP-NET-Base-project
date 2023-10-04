using System.ComponentModel;

namespace Domain.Extensions
{
    public static class ObjectExtensions
    {
        public static string GetDescription(this object @object)
        {
            return GetTypeDescription(@object.GetType());
        }

        public static string GetTypeDescription(this Type type)
        {
            DescriptionAttribute attribute = type
               .GetCustomAttributes(typeof(DescriptionAttribute), false)
               .SingleOrDefault() as DescriptionAttribute;

            return attribute == null ? string.Empty : attribute.Description;
        }
    }
}
