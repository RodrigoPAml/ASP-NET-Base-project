using System.ComponentModel;
using Domain.Models.Entities.Base;

namespace Domain.Models.Entities
{
    [Description("Migration")]
    public class Migration : Entity
    {
        public string Name { get; set; }
    }
}
