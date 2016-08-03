using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Track.Dto;

namespace ASBicycle.Track
{
    public interface ITrackAppService : IApplicationService
    {
        [HttpGet]
        Task<TrackOutput> GetAllTrack([FromUri]TrackInput trackInput);
        [HttpGet]
        Task<TrackOutput> GetPendingTrack([FromUri]TrackInput trackInput);

        Task CreateNewTrack(TrackInsertInput trackInsertInput);

        Task UpdateTrack(TrackUpdateInput trackUpdateInput);
    }
}