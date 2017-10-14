using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Kloud21.DataModel;

namespace Kloud21.GenericRepository
{
    public interface IReadOnlyRepository<TKey, TEntity> where TEntity : class, IIdentityKey<TKey>
    {
        IList<TEntity> All();

        TEntity FindBy(TKey id);
    }

}
