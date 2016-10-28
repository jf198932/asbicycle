using Abp.Domain.Repositories;

namespace ASBicycle.Bike
{
    public interface IBikeReadRepository : IRepository<Entities.Bike, int>
    {
         
    }
}