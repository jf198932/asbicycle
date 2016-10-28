using Abp.EntityFramework;
using ASBicycle.VersionUpdate;

namespace ASBicycle.EntityFramework.Repositories.VersionUpdate
{
    public class VersionUpdateReadRepository : ReadonlyASBicycleRepositoryBase<Entities.VersionUpdate, int>, IVersionUpdateReadRespository
    {
        public VersionUpdateReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}