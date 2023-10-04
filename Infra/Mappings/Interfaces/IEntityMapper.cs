using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings.Implementations
{
    /// <summary>
    /// Base class for mapping entities
    /// </summary>
    public interface IEntityMapper<T> where T : class
    {
        void OnBuild(EntityTypeBuilder<T> entity);
    }
}
