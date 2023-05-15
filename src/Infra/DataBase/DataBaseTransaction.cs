using Microsoft.EntityFrameworkCore.Storage;

namespace API.Infra.Database
{
    /// <summary>
    /// Controls the database transaction
    /// </summary>
    public class DataBaseTransaction
    {
        private readonly DataBaseContext _db;

        private IDbContextTransaction _transaction;

        public DataBaseTransaction(DataBaseContext db)
        {
            _db = db;
        }

        public void Begin()
        {
            _transaction = _db.Database.BeginTransaction();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }
    }
}
