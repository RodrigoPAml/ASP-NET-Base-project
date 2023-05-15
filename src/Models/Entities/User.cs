using API.Enums;
using API.Infra.Base;
using System.ComponentModel;

namespace API.Models.Entities
{
    [Description("Usuário")]
    public class User : Entity
    {
        #region Fields

        public string Login { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public ProfileTypeEnum Profile { get; set; }

        #endregion
    }
}
