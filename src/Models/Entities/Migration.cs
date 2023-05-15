using API.Infra.Base;
using System.ComponentModel;

namespace API.Models.Entities
{
    [Description("Migração")]
    public class Migration : Entity
    {
        public string Name { get; set; }
    }
}
