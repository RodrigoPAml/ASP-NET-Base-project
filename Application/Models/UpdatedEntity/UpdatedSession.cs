using Domain.Models.Entities.Base;

namespace Application.Models.UpdatedEntity
{
    public class UpdatedSession : Entity
    {
        public ulong? MovieId { get; set; }

        public DateTime? Date { get; set; }
    }
}
