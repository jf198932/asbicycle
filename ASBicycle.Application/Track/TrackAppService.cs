using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using ASBicycle.Track.Dto;
using ASBicycle.User;
using AutoMapper;

namespace ASBicycle.Track
{
    public class TrackAppService : ASBicycleAppServiceBase, ITrackAppService
    {
        private readonly IRepository<Entities.Track> _trackRepository;
        private readonly IRepository<Entities.User> _userRepository;
        private readonly ISqlExecuter _sqlExecuter;

        public TrackAppService(IRepository<Entities.Track> trackRepository, IRepository<Entities.User> userRepository, ISqlExecuter sqlExecuter)
        {
            _trackRepository = trackRepository;
            _userRepository = userRepository;
            _sqlExecuter = sqlExecuter;
        }

        public async Task<List<TrackOutput>> GetAllTrack([FromUri]TrackInput trackInput)
        {
            var user =
                await
                    _userRepository.FirstOrDefaultAsync(
                        u => u.Id == trackInput.User_id);
            if (user == null)
                throw new UserFriendlyException("请先登录");
            var track = _trackRepository.GetAll()
                .Where(t => t.User_id == trackInput.User_id)
                .OrderByDescending(t=>t.Start_time)
                .Skip((trackInput.Index-1)* trackInput.Pagesize)
                .Take(trackInput.Pagesize)
                .Select(t=> new TrackOutput
                {
                    Start_point = t.Start_point.ToString(),
                    End_point = t.End_point.ToString(),
                    Start_site_id = t.Start_site_id,
                    Start_site_name = t.Bikesitestart == null ? "" : t.Bikesitestart.Name,
                    End_site_id = t.End_site_id,
                    End_site_name  = t.Bikesiteend == null ? "" : t.Bikesiteend.Name,
                    Start_time = t.Start_time.ToString(),
                    End_time = t.End_time.ToString(),
                    Payment = t.Payment,
                    Pay_status = t.Pay_status,
                    Remark = t.Remark.ToString(),
                    Remarkstatus = t.Remark.ToString() == "" ? 0: 1,
                    Bike_id = t.Bike_id,
                    out_trade_no = t.Pay_docno
                })
                .ToList();


            return track;
        }

        public async Task<TrackOutput> GetPendingTrack([FromUri]TrackInput trackInput)
        {
            var user =
                await
                    _userRepository.FirstOrDefaultAsync(
                        u => u.Id == trackInput.User_id);
            if (user == null)
                throw new UserFriendlyException("请先登录");
            var track = _trackRepository.GetAll()
                .Where(t => t.User_id == trackInput.User_id && t.Pay_status == 2)
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
        [HttpPost]
        public void UpdateTrackAlipay(TrackAlipay alipay)
        {
            var track = _trackRepository.GetAll().FirstOrDefault(t => t.Pay_docno == alipay.out_trade_no);
            track.Pay_status = alipay.pay_status;
            track.Pay_method = "支付宝";
            track.Payment = alipay.total_fee;
            _trackRepository.Update(track);
        }
        [HttpPost]
        public async Task UpdatePinlun(TrackInput trackInput)
        {
            var track =
                _trackRepository.GetAll().FirstOrDefault(t => t.Pay_docno == trackInput.out_trade_no && t.User_id == trackInput.User_id);

            if (track == null)
            {
                throw new UserFriendlyException("订单号错误");
            }

            track.Remark = trackInput.remark;

            await _trackRepository.UpdateAsync(track);
        }
    }
}