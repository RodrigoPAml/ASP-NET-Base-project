using Domain.Models.Entities.Base;

namespace Application.Models.UpdatedEntity
{
    public class UpdatedUser : Entity
    {
        public string? Name { get; set; }

        public string? Password { get; set; }
    }
}
