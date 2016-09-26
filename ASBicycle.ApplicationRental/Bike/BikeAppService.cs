using System;
using System.Configuration;
using System.Web;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using ASBicycle.Bike;
using ASBicycle.Rental.Bike.Dto;

namespace ASBicycle.Rental.Bike
{
    public class BikeAppService : ASBicycleAppServiceBase, IBikeAppService
    {
        private readonly IBikeRepository _bikeRepository;
        private readonly IRepository<Entities.Track> _trackRepository; 
        private readonly ISqlExecuter _sqlExecuter; 

        public BikeAppService(IBikeRepository bikeRepository, IRepository<Entities.Track> trackRepository, ISqlExecuter sqlExecuter)
        {
            _bikeRepository = bikeRepository;
            _trackRepository = trackRepository;
            _sqlExecuter = sqlExecuter;
        }

        [HttpPost]
        public async Task<RentalBikeOutput> RentalBike(RentalBikeInput input)
        {
            var bike = await _bikeRepository.FirstOrDefaultAsync(t => t.Ble_name == input.Ble_name);
            if (bike.Bikesite_id == null)
            {
                throw new UserFriendlyException("该车不能出租!");
            }
            var paydocno = new Guid();
            await _trackRepository.InsertAsync(new Entities.Track
            {
                Bike_id = bike.Id,
                User_id = input.user_id,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now,
                Start_site_id = bike.Bikesite_id,
                Start_point = input.gps_point,
                Start_time = DateTime.Now,
                Pay_docno = paydocno.ToString()
            });
            bike.Bike_status = 0;//出租中
            await _bikeRepository.UpdateAsync(bike);
            return new RentalBikeOutput {out_trade_no = paydocno.ToString()};
        }

        [HttpPost]
        public async Task RentalBikeFinish(RentalBikeInput input)
        {
            var track = await _trackRepository.FirstOrDefaultAsync(
                    t => t.Pay_docno == input.out_trade_no && t.User_id == input.user_id);
            if(track == null)
            {
                throw new UserFriendlyException("无该订单号!");
            }
            var bike = await _bikeRepository.FirstOrDefaultAsync(t => t.Ble_name == input.Ble_name);
            if (bike.Bikesite_id == null)
            {
                throw new UserFriendlyException("请到桩点还车!");
            }
            track.End_point = input.gps_point;
            track.End_site_id = input.bikesiteid;
            track.End_time = DateTime.Now;
            track.Updated_at = DateTime.Now;
            track.Pay_status = 10;
            
            await _trackRepository.UpdateAsync(track);

            bike.Bike_status = 1;
            await _bikeRepository.UpdateAsync(bike);
        }
    }
}