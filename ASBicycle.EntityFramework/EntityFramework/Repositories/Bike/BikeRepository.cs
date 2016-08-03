using Abp.EntityFramework;
using ASBicycle.Bike;

namespace ASBicycle.EntityFramework.Repositories.Bike
{
    public class BikeRepository : ASBicycleRepositoryBase<Entities.Bike, int>, IBikeRepository
    {
        public BikeRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}