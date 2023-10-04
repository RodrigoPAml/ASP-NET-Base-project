using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings.Implementations
{
    public class MigrationMapping : IEntityMapper<Migration>
    {
        public void OnBuild(EntityTypeBuilder<Migration> entity)
        {
            entity.ToTable("migration", "public");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Name).HasColumnName("name");
        }
    }
}
