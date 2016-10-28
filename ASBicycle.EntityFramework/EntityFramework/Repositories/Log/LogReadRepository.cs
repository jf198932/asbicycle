using System;
using Abp.EntityFramework;
using ASBicycle.Log;

namespace ASBicycle.EntityFramework.Repositories.Log
{
    public class LogReadRepository : ReadonlyASBicycleRepositoryBase<Entities.Log, int>, ILogReadRepository
    {
        public LogReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}