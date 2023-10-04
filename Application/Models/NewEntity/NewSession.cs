using Domain.Models.Entities.Base;

namespace Application.Models.NewEntity
{
    public class NewSession : Entity
    {
        public ulong? MovieId { get; set; }

        public DateTime? Date { get; set; }
    }
}
