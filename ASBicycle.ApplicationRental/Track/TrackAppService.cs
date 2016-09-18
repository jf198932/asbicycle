using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.AutoMapper;
using Abp.UI;
using ASBicycle.Rental.Track.Dto;
using ASBicycle.Track;
using ASBicycle.User;
using AutoMapper;

namespace ASBicycle.Rental.Track
{
    public class TrackAppService : ASBicycleAppServiceBase, ITrackAppService
    {
        private readonly ITrackRepository _trackRepository;
        private readonly IUserRepository _userRepository;

        public TrackAppService(ITrackRepository trackRepository, IUserRepository userRepository)
        {
            _trackRepository = trackRepository;
            _userRepository = userRepository;
        }

        public async Task<TrackOutput> GetAllTrack([FromUri]TrackInput trackInput)
        {
            var user =
                await
                    _userRepository.FirstOrDefaultAsync(
                        u => u.Id == trackInput.User_id && u.Remember_token == trackInput.Token);
            if (user == null)
                throw new UserFriendlyException("请先登录");
            var track = _trackRepository.GetAll()
                .Where(t => t.Bike.User_id == trackInput.User_id)
                .OrderBy(t=>t.Id)
                .Skip((trackInput.Index-1)* trackInput.Pagesize)
                .Take(trackInput.Pagesize);
            return track.MapTo<TrackOutput>();
        }

        public async Task<TrackOutput> GetPendingTrack([FromUri]TrackInput trackInput)
        {
            var user =
                await
                    _userRepository.FirstOrDefaultAsync(
                        u => u.Id == trackInput.User_id && u.Remember_token == trackInput.Token);
            if (user == null)
                throw new UserFriendlyException("请先登录");
            var track = _trackRepository.GetAll()
                .Where(t => t.Bike.User_id == trackInput.User_id && t.Pay_status == 2)
                .OrderBy(t => t.Id)
                .Skip((trackInput.Index - 1) * trackInput.Pagesize)
                .Take(trackInput.Pagesize);
            return track.MapTo<TrackOutput>();
        }
        [HttpPost]
        public async Task CreateNewTrack(TrackInsertInput trackInsertInput)
        {
            Mapper.CreateMap<TrackInsertInput, Entities.Track> ();
            var track = trackInsertInput.MapTo<Entities.Track>();
            track.Created_at = DateTime.Now;
            track.Updated_at = DateTime.Now;
            track.Pay_status = 2;
            await _trackRepository.InsertAsync(track);
        }
        [HttpPost]
        public async Task UpdateTrack(TrackUpdateInput trackUpdateInput)
        {
            var track = await _trackRepository.FirstOrDefaultAsync(t => t.Id == trackUpdateInput.Id);
            track.End_point = trackUpdateInput.End_point;
            track.End_site_id = trackUpdateInput.End_stie_id;
            track.Updated_at = DateTime.Now;
            await _trackRepository.UpdateAsync(track);
        }
    }
}