using Domain.Enums;
using Domain.Models.Entities.Base;
using System.ComponentModel;

namespace Domain.Models.Entities
{
    [Description("Movie")]
    public class Movie : Entity
    {
        #region Fields

        public string Name { get; set; }

        public string Synopsis { get; set; }

        public int Duration { get; set; }  

        public MovieGenre Genre { get; set; }

        #endregion

        #region References

        public ICollection<Session> Sessions { get; set;}

        #endregion
    }
}
