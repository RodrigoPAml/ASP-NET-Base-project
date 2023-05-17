using API.Enums;
using API.Infra.Base;
using System.ComponentModel;

namespace API.Models.Entities
{
    [Description("Movie")]
    public class Movie : Entity
    {
        #region Fields

        public string Name { get; set; }

        public string Synopsis { get; set; }

        public DateTime Duration { get; set; }  

        public MovieGenre Genre { get; set; }

        #endregion

        #region References

        public ICollection<Session> Sessions { get; set;}

        #endregion
    }
}
