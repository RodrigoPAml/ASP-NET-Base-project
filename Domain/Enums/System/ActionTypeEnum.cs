using System.ComponentModel;

namespace Domain.Enums.System
{
    /// <summary>
    /// Action type used in services validations
    /// </summary>
    public enum ActionTypeEnum
    {
        [Description("Inserir")]
        Create,

        [Description("Atualizar")]
        Update,

        [Description("Deletar")]
        Delete,
    }
}
