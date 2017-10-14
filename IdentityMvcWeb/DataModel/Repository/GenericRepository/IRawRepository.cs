using Kloud21.DataModel;

namespace Kloud21.GenericRepository
{
    public interface IRawRepository<TKey, TEntity> where TEntity : class, IIdentityKey<TKey>
    {
        void ExecuteRawQuery(string hql);

    }

}
