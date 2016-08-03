using Abp.EntityFramework;
using ASBicycle.Bikesite;

namespace ASBicycle.EntityFramework.Repositories.Bikesite
{
    public class BikesiteRepository : ASBicycleRepositoryBase<Entities.Bikesite,int>, IBikesiteRepository
    {
        public BikesiteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}