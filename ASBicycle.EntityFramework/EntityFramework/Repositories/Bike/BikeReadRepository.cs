using Abp.EntityFramework;
using ASBicycle.Bike;

namespace ASBicycle.EntityFramework.Repositories.Bike
{
    public class BikeReadRepository : ReadonlyASBicycleRepositoryBase<Entities.Bike, int>, IBikeReadRepository
    {
        public BikeReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}