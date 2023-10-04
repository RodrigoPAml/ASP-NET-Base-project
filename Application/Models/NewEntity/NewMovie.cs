using Domain.Models.Entities.Base;

namespace Application.Models.NewEntity
{
    public class NewMovie : Entity
    {
        public string Name { get; set; }

        public string Synopsis { get; set; }

        public int? Duration { get; set; }
    }
}
