using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Abp.AutoMapper;
using ASBicycle.Bike;
using ASBicycle.Bikesite.Dto;
using ASBicycle.Bike.Dto;
using System.Linq;
using System.Web.Http;
using ASBicycle.Common;

namespace ASBicycle.Bikesite
{
    public class BikesiteAppService : ASBicycleAppServiceBase, IBikesiteAppService
    {
        private readonly IBikesiteReadRepository _bikesiteReadRepository;
        private readonly IBikeReadRepository _bikeReadRepository;

        public BikesiteAppService(IBikesiteReadRepository bikesiteReadRepository
            , IBikeReadRepository bikeReadRepository)
        {
            _bikesiteReadRepository = bikesiteReadRepository;
            _bikeReadRepository = bikeReadRepository;
        }

        public async Task<BikesiteOutput> GetOneBikesiteInfo([FromUri]BikesitePageInput input)
        {
            //var bikesite = await _bikesiteRepository.GetAsync(id);
            //var bikes = _bikeRepository.GetAll();
            
           // var bike = bikesite.Bikes.OrderBy(b => b.Id).Skip(pagesize*(index - 1)).Take(pagesize);
            //var result = bikesite.MapTo<BikesiteOutput>();
            //result.Bikes = bike.MapTo<List<BikeDto>>();
            //return result;
            var bike =
                _bikeReadRepository.GetAll()
                    .Where(b => b.Bikesite_id == input.id)
                    .OrderBy(b => b.Id)
                    .Skip(input.pagesize*(input.index - 1))
                    .Take(input.pagesize);

            return new BikesiteOutput {Bikes = bike.MapTo<List<BikeDto>>()};
        }

        public async Task<List<BikesiteListOutput>> GetNearbyBikesites([FromUri]BikesiteInput input)
        {
            var model = await _bikesiteReadRepository.GetAllListAsync(t=> !string.IsNullOrEmpty(t.Gps_point) && t.School != null && t.Enable);
            List<BikesiteListOutput> result = new List<BikesiteListOutput>();
            foreach (var item in model)
            {
                var b = item.MapTo<BikesiteListOutput>();
                string gps = item.Gps_point;
                var gpss = gps.Split(',');
                if (gpss.Length == 2)
                {
                    double _lon = 0;
                    double.TryParse(gpss[0], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"),
                        out _lon);
                    double _lat = 0;
                    double.TryParse(gpss[1], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"),
                        out _lat);
                    b.Distance = LatlonHelper.GetDistance(input.lat, input.lon, _lat, _lon);
                    result.Add(b);
                }
                else
                {
                    Logger.Debug("GetNearbyBikesites-67-" + item.Id);
                }
            }

            var res = result.Where(r => r.Distance < 10).ToList();
            return res;
        }
    }
}