using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Track.Dto;

namespace ASBicycle.Track
{
    public interface ITrackAppService : IApplicationService
    {
        [HttpGet]
        Task<List<TrackOutput>> GetAllTrack([FromUri]TrackInput trackInput);
        [HttpGet]
        Task<TrackOutput> GetPendingTrack([FromUri]TrackInput trackInput);
        [HttpPost]
        Task CreateNewTrack(TrackInsertInput trackInsertInput);
        [HttpPost]
        Task UpdateTrack(TrackUpdateInput trackUpdateInput);
        [HttpPost]
        Task UpdatePinlun(TrackInput trackInput);

        [HttpPost]
        void UpdateTrackAlipay(TrackAlipay alipay);
    }
}