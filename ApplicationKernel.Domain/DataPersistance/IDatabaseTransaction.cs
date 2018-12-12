using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationKernel.Domain.DataPersistance
{
    public interface IUnitOfWork
    {
        void Add(IEntity entity);
        void Update(IEntity entity);
        void Delete(IEntity entity);
        void Delete<T>(T entity) where T : IDeletable, IEntity;
        Task PersistChanges();
    }

    public delegate IUnitOfWork RunDatabaseTransaction();

    public static class UnitOfWork
    {
        public static void SaveRange(this IUnitOfWork unitOfWork, IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
                unitOfWork.Add(entity);
        }

        public static void UpdateRange(this IUnitOfWork unitOfWork, IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
                unitOfWork.Update(entity);
        }

        public static IUnitOfWork DeleteRange(this IUnitOfWork unitOfWork, IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
                unitOfWork.Delete(entity);
            return unitOfWork;
        }

        public static void SaveRange(this IUnitOfWork unitOfWork, params IEntity[] entities)
        {
            unitOfWork.SaveRange((IEnumerable<IEntity>) entities);
        }

        public static void UpdateRange(this IUnitOfWork unitOfWork, params IEntity[] entities)
        {
            unitOfWork.UpdateRange((IEnumerable<IEntity>)entities);
        }

        public static void DeleteRange(this IUnitOfWork unitOfWork, params IEntity[] entities)
        {
            unitOfWork.DeleteRange((IEnumerable<IEntity>)entities);
        }
    }
}
