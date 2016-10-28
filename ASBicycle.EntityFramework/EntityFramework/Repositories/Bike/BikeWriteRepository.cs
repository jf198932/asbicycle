using Abp.EntityFramework;
using ASBicycle.Bike;

namespace ASBicycle.EntityFramework.Repositories.Bike
{
    public class BikeWriteRepository : ASBicycleRepositoryBase<Entities.Bike, int>, IBikeWriteRepository
    {
        public BikeWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}