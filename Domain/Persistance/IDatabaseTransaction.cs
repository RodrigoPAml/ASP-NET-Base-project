namespace Domain.Persistance
{
    /// <summary>
    /// Represents the database transaction interface
    /// </summary>
    public interface IDatabaseTransaction
    {
        void Begin();

        void Save();

        void Commit();

        void Rollback();
    }
}
