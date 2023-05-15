using System.ComponentModel;

namespace API.Infra.Extensions
{
    public static class ObjectExtensions
    {
        public static string GetDescription(this object @object)
        {
            DescriptionAttribute attribute = @object.GetType()
               .GetField(@object.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false)
               .SingleOrDefault() as DescriptionAttribute;

                return attribute == null ? string.Empty : attribute.Description;
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
