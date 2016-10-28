using Abp.EntityFramework;
using ASBicycle.Sitemonitor;

namespace ASBicycle.EntityFramework.Repositories.Sitemonitor
{
    public class SitemonitorReadRepository : ReadonlyASBicycleRepositoryBase<Entities.Sitemonitor, int>, ISitemonitorReadRepository
    {
        public SitemonitorReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}