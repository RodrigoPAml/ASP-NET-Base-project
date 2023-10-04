using Domain.Enums;
using System.ComponentModel;
using Domain.Models.Entities.Base;

namespace Domain.Models.Entities
{
    [Description("User")]
    public class User : Entity
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public ProfileTypeEnum Profile { get; set; }
    }
}
