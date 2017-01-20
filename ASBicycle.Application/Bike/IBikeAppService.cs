using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Bike.Dto;

namespace ASBicycle.Bike
{
    public interface IBikeAppService : IApplicationService
    {
        /// <summary>
        /// 获取车辆信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        Task<BikeOutput> GetBikeInfo([FromUri] BikegetInput input);
        /// <summary>
        /// 更新车辆
        /// </summary>
        /// <param name="bikeInput"></param>
        /// <returns></returns>
        [HttpPost]
        Task UpdateBike(BikeInput bikeInput);
        /// <summary>
        /// 上传车辆图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        BikeUploadOutput UploadBikePic();
        /// <summary>
        /// 获取车辆异常信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        Task<AlarmBikeOutput> GetAlarmBikeWay([FromUri] BikegetInput input);
        /// <summary>
        /// 租车
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task<RentalBikeOutput> RentalBiketemp(RentalBikeInput input);
        /// <summary>
        /// 结束租车（正常）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task<RentalCostOutput> RentalBikeFinishtemp(RentalBikeInput input);
        /// <summary>
        /// 结束租车（强制）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task<RentalCostOutput> RentalBikeForcedFinishtemp(RentalBikeInput input);
        /// <summary>
        /// 结束租车（免费）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task<RentalFinishOutput> RentalBikeFinishtempo(RentalBikeInput input);
        /// <summary>
        /// 刷新租车信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task<RentalBikeOutput> RefreshBiketemp(RentalBikeInput input);
        /// <summary>
        /// 订单结束计算金额
        /// </summary>
        /// <param name="input"></param>
        [HttpGet]
        Task<RentalInfoOutput> RentalFinishInfo([FromUri] RentalBikeInput input);
        /// <summary>
        /// 订单支付状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        Task<TrackInfoOutput> RentalTrackInfo([FromUri] RentalBikeInput input);
        /// <summary>
        /// 能否租用车辆
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        Task<CanRentalOutput> CanReantalBike([FromUri] RentalBikeInput input);
        /// <summary>
        /// 余额结算
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task CostByRecharge(RentalRechargeInput input);
        /// <summary>
        /// 蓝牙锁开锁之后创建订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        Task<RentalBikeOutput> RentalBikeCreateTrack(RentalBikeInput input);
        /// <summary>
        /// 根据车辆编号获取车辆总共租用时间，租用次数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        Task<BikeUsedOutput> GetBikeUsedInfo([FromUri]RentalBikeInput input);
    }
}