using System.ComponentModel;

namespace API.Enums
{
    /// <summary>
    /// Movie genre enum
    /// </summary>
    public enum MovieGenre
    {
        [Description("Action")]
        Action,

        [Description("Adventure")]
        Adventure,

        [Description("Comedy")]
        Comedy,

        [Description("Drama")]
        Drama,

        [Description("Fantasy")]
        Fantasy,

        [Description("Horror")]
        Horror,

        [Description("Romance")]
        Romance,

        [Description("SciFi")]
        SciFi,

        [Description("Thriller")]
        Thriller,

        [Description("Western")]
        Western
    }
}
