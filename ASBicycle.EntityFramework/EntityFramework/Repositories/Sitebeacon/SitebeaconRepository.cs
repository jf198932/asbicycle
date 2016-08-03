using Abp.EntityFramework;
using ASBicycle.Sitebeacon;

namespace ASBicycle.EntityFramework.Repositories.Sitebeacon
{
    public class SitebeaconRepository : ASBicycleRepositoryBase<Entities.Sitebeacon,int>, ISitebeaconRepository
    {
        public SitebeaconRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}