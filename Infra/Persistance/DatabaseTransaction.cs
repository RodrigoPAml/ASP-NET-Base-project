using Domain.Persistance;
using Infra.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infra.Persistance
{
    /// <summary>
    /// Controls the database transaction
    /// </summary>
    public class DatabaseTransaction : IDatabaseTransaction
    {
        private readonly DatabaseContext _db;

        private IDbContextTransaction _transaction;

        public DatabaseTransaction(DatabaseContext db)
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
