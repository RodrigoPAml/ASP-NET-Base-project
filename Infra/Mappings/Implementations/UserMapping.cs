using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings.Implementations
{
    public class UserMapping : IEntityMapper<User>
    {
        public void OnBuild(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("user", "public");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id");
            entity.Property(x => x.Name).HasColumnName("name");
            entity.Property(x => x.Password).HasColumnName("password");
            entity.Property(x => x.Profile).HasColumnName("profile");
            entity.Property(x => x.Login).HasColumnName("login");
        }
    }
}
