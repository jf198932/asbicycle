using Abp.EntityFramework;
using ASBicycle.Sitemonitor;

namespace ASBicycle.EntityFramework.Repositories.Sitemonitor
{
    public class SitemonitorWriteRepository : ASBicycleRepositoryBase<Entities.Sitemonitor, int>, ISitemonitorWriteRepository
    {
        public SitemonitorWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}