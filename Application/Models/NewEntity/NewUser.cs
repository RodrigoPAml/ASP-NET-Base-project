using Domain.Models.Entities.Base;

namespace Application.Models.NewEntity
{
    public class NewUser : Entity
    {
        public string Password { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }
    }
}
