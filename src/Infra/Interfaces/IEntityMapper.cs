using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Infra.Interfaces
{
    /// <summary>
    /// Base class for mapping entities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityMapper<T> where T : class
    {
        void OnBuild(EntityTypeBuilder<T> entity);
    }
}
