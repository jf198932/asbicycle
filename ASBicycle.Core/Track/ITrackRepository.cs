using Abp.Domain.Repositories;

namespace ASBicycle.Track
{
    public interface ITrackRepository : IRepository<Entities.Track, int>
    {
         
    }
}