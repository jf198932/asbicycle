using Abp.Domain.Repositories;

namespace ASBicycle.Bike
{
    public interface IBikeRepository : IRepository<Entities.Bike, int>
    {
         
    }
}