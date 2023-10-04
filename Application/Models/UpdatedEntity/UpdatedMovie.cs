using Domain.Enums;
using Domain.Models.Entities.Base;

namespace Application.Models.UpdatedEntity
{
    public class UpdatedMovie : Entity
    {
        public string Name { get; set; }

        public string? Synopsis { get; set; }

        public int? Duration { get; set; }

        public MovieGenre? Genre { get; set; }
    }
}
