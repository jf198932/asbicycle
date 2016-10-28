using Abp.EntityFramework;
using ASBicycle.Track;

namespace ASBicycle.EntityFramework.Repositories.Track
{
    public class TrackReadRepository : ReadonlyASBicycleRepositoryBase<Entities.Track, int>, ITrackReadRepository
    {
        public TrackReadRepository(IDbContextProvider<ReadonlyASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}