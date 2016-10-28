using Abp.Domain.Repositories;

namespace ASBicycle.Track
{
    public interface ITrackWriteRepository : IRepository<Entities.Track, int>
    {
         
    }
}