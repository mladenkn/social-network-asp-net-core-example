using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationKernel.Domain.DataPersistance
{
    public interface IDatabaseTransaction
    {
        IDatabaseTransaction Save(IEntity entity);
        IDatabaseTransaction Update(IEntity entity);
        IDatabaseTransaction Delete(IEntity entity);
        Task Commit();
    }

    public delegate IDatabaseTransaction RunDatabaseTransaction();

    public static class DatabaseTransaction
    {
        public static IDatabaseTransaction SaveRange(this IDatabaseTransaction transaction, IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
                transaction.Save(entity);
            return transaction;
        }

        public static IDatabaseTransaction UpdateRange(this IDatabaseTransaction transaction, IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
                transaction.Update(entity);
            return transaction;
        }

        public static IDatabaseTransaction DeleteRange(this IDatabaseTransaction transaction, IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
                transaction.Delete(entity);
            return transaction;
        }

        public static IDatabaseTransaction SaveRange(this IDatabaseTransaction transaction, params IEntity[] entities)
        {
            return transaction.SaveRange((IEnumerable<IEntity>) entities);
        }

        public static IDatabaseTransaction UpdateRange(this IDatabaseTransaction transaction, params IEntity[] entities)
        {
            return transaction.UpdateRange((IEnumerable<IEntity>)entities);
        }

        public static IDatabaseTransaction DeleteRange(this IDatabaseTransaction transaction, params IEntity[] entities)
        {
            return transaction.DeleteRange((IEnumerable<IEntity>)entities);
        }
    }
}
