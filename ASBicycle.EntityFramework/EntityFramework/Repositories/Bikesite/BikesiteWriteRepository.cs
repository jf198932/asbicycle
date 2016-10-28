using Abp.EntityFramework;
using ASBicycle.Bikesite;

namespace ASBicycle.EntityFramework.Repositories.Bikesite
{
    public class BikesiteWriteRepository : ASBicycleRepositoryBase<Entities.Bikesite,int>, IBikesiteWriteRepository
    {
        public BikesiteWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}