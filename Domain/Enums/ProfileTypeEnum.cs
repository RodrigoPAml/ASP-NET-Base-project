using System.ComponentModel;

namespace Domain.Enums
{
    /// <summary>
    /// User profile type 
    /// </summary>
    public enum ProfileTypeEnum
    {
        [Description("User")]
        User = 0,

        [Description("Admin")]
        Admin = 1,
    }
}
