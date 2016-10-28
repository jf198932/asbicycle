using Abp.Domain.Repositories;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;
using ASBicycle.Log;

namespace ASBicycle.EntityFramework.Repositories.Log
{
    public class LogWriteRepository : ASBicycleRepositoryBase<Entities.Log,int>, ILogWriteRepository
    {
        public LogWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}