using Abp.EntityFramework;
using ASBicycle.VersionUpdate;

namespace ASBicycle.EntityFramework.Repositories.VersionUpdate
{
    public class VersionUpdateWriteRepository : ASBicycleRepositoryBase<Entities.VersionUpdate, int>, IVersionUpdateWriteRespository
    {
        public VersionUpdateWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}