using Abp.EntityFramework;
using ASBicycle.Bikesite;

namespace ASBicycle.EntityFramework.Repositories.Bikesite
{
    public class BikesiteReadRepository : ReadonlyASBicycleRepositoryBase<Entities.Bikesite,int>, IBikesiteReadRepository
    {
        public BikesiteReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}