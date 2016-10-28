using Abp.Domain.Repositories;

namespace ASBicycle.Bike
{
    public interface IBikeWriteRepository : IRepository<Entities.Bike, int>
    {
         
    }
}