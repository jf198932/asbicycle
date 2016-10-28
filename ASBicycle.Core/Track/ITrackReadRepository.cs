using Abp.Domain.Repositories;

namespace ASBicycle.Track
{
    public interface ITrackReadRepository : IRepository<Entities.Track, int>
    {
         
    }
}