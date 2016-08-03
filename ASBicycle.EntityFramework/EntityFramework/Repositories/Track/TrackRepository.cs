using Abp.EntityFramework;
using ASBicycle.Track;

namespace ASBicycle.EntityFramework.Repositories.Track
{
    public class TrackRepository : ASBicycleRepositoryBase<Entities.Track, int>, ITrackRepository
    {
        public TrackRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}