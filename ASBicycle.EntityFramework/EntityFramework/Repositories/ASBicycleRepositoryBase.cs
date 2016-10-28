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


    public abstract class ReadonlyASBicycleRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<ReadonlyASBicycleDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected ReadonlyASBicycleRepositoryBase(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class ReadonlyASBicycleRepositoryBase<TEntity> : ReadonlyASBicycleRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected ReadonlyASBicycleRepositoryBase(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
