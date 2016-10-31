using Abp.EntityFramework;
using ASBicycle.VersionUpdate;

namespace ASBicycle.EntityFramework.Repositories.VersionUpdate
{
    public class VersionUpdateWriteRepository : ASBicycleRepositoryBase<Entities.VersionUpdate, int>, IVersionUpdateWriteRepository
    {
        public VersionUpdateWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}