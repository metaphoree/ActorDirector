using System.Collections.Generic;

namespace Kloud21.GenericRepository
{
    public interface IPersistRepository<TEntity> where TEntity : class
    {
        //TEntity Add(TEntity entity);
        TEntity Add(TEntity entity);
        TEntity AddProfile(TEntity entity);

        TEntity UpdateProfile(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
