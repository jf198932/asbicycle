using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.AutoMapper;
using ASBicycle.Bike;
using ASBicycle.Bikesite;
using ASBicycle.Rental.Bike.Dto;
using ASBicycle.Rental.BikeSite.Dto;
using ASBicycle.Rental.Common;

namespace ASBicycle.Rental.BikeSite
{
    public class BikesiteAppService : ASBicycleAppServiceBase, IBikesiteAppService
    {
        private readonly IBikesiteRepository _bikesiteRepository;
        private readonly IBikeRepository _bikeRepository;

        public BikesiteAppService(IBikesiteRepository bikesiteRepository, IBikeRepository bikeRepository)
        {
            _bikesiteRepository = bikesiteRepository;
            _bikeRepository = bikeRepository;
        }

        public async Task<BikesiteOutput> GetOneBikesiteInfo([FromUri]int id, int index, int pagesize)
        {
            //var bikesite = await _bikesiteRepository.GetAsync(id);
            //var bikes = _bikeRepository.GetAll();
            
           // var bike = bikesite.Bikes.OrderBy(b => b.Id).Skip(pagesize*(index - 1)).Take(pagesize);
            //var result = bikesite.MapTo<BikesiteOutput>();
            //result.Bikes = bike.MapTo<List<BikeDto>>();
            //return result;
            var bike = _bikeRepository.GetAll().Where(b => b.Bikesite_id == id).OrderBy(b => b.Id).Skip(pagesize * (index - 1)).Take(pagesize);

            return new BikesiteOutput {Bikes = bike.MapTo<List<BikeDto>>()};
        }

        public async Task<List<BikesiteListOutput>> GetNearbyBikesites([FromUri]double lat, double lon)
        {
            var model = _bikesiteRepository.GetAll().Where(t=>t.Sitemonitors.Count>0 && !string.IsNullOrEmpty(t.Gps_point)).ToList();
            List<BikesiteListOutput> result = new List<BikesiteListOutput>();
            foreach (var item in model)
            {
                var b = item.MapTo<BikesiteListOutput>();
                string gps = item.Gps_point;
                double _lon = 0;
                double.TryParse(gps.Split(',')[0],out _lon);
                double _lat = 0;
                double.TryParse(gps.Split(',')[1], out _lat);
                b.Distance = LatlonHelper.GetDistance(lat, lon, _lat, _lon);
                result.Add(b);
            }

            var res = result.Where(r => r.Distance < 5).ToList();
            return res;
        }
    }
}