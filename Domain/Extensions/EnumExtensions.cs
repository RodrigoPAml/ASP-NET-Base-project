namespace Domain.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// If a enum is in a valid interval
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static bool IsInRange(this Enum @enum)
        {
            return Enum.IsDefined(@enum.GetType(), @enum);
        }
    }
}
