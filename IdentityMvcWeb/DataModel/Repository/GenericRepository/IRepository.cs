using Kloud21.DataModel;

namespace Kloud21.GenericRepository
{
    //public interface IRepository<TKey, TEntity> : IPersistRepository<TEntity>, IReadOnlyRepository<TKey, TEntity>, IRawRepository<TKey, TEntity>
    //    where TEntity : class, IIdentityKey<TKey>
    //{

    //}

    public interface IRepository<TKey, TEntity> : IPersistRepository<TEntity>, IReadOnlyRepository<TKey, TEntity>
        where TEntity : class, IIdentityKey<TKey>
    {

    }

}
