using Abp.EntityFramework;
using ASBicycle.Track;

namespace ASBicycle.EntityFramework.Repositories.Track
{
    public class TrackWriteRepository : ASBicycleRepositoryBase<Entities.Track, int>, ITrackWriteRepository
    {
        public TrackWriteRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}