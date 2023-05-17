using API.Infra.Base;
using System.ComponentModel;

namespace API.Models.Entities
{
    [Description("Session")]
    public class Session : Entity
    {
        #region Fields

        public DateTime Date { get; set; }

        public ulong MovieId { get; set; }

        public Movie Movie { get; set; }

        #endregion
    }
}
