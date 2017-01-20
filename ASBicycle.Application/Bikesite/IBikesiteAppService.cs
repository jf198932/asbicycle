using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Bikesite.Dto;

namespace ASBicycle.Bikesite
{
    public interface IBikesiteAppService : IApplicationService
    {
        /// <summary>
        /// 获取桩点车辆列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        Task<BikesiteOutput> GetOneBikesiteInfo([FromUri]BikesitePageInput input);
        /// <summary>
        /// 根据经纬度获取10公里之内的桩点信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        Task<List<BikesiteListOutput>> GetNearbyBikesites([FromUri]BikesiteInput input);
        /// <summary>
        /// 根据经纬度获取1公里之内的车辆信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        Task<List<BikegpsOutput>> GetNearbyBikes([FromUri]BikesiteInput input);
    }
}