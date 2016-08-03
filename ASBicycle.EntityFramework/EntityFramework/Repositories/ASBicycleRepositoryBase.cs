using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace ASBicycle.EntityFramework.Repositories
{
    public abstract class ASBicycleRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<ASBicycleDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected ASBicycleRepositoryBase(IDbContextProvider<ASBicycleDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class ASBicycleRepositoryBase<TEntity> : ASBicycleRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected ASBicycleRepositoryBase(IDbContextProvider<ASBicycleDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
