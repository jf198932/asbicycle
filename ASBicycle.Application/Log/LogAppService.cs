using System;
using System.Threading.Tasks;
using Abp.UI;
using ASBicycle.Bike;
using ASBicycle.Log.Dto;

namespace ASBicycle.Log
{
    public class LogAppService : ASBicycleAppServiceBase, ILogAppService
    {
        private readonly IBikeWriteRepository _bikeWriteRepository;
        private readonly ILogWriteRepository _logWriteRepository;

        public LogAppService(IBikeWriteRepository bikeWriteRepository, ILogWriteRepository logWriteRepository)
        {
            _logWriteRepository = logWriteRepository;
            _bikeWriteRepository = bikeWriteRepository;
        }

        public async Task CreateLog(LogInput input)
        {
            var bike = await _bikeWriteRepository.FirstOrDefaultAsync(t => t.Ble_name == input.ble_name);
            if (bike == null)
            {
                throw new UserFriendlyException("没有该车辆");
            }

            await _logWriteRepository.InsertAsync(new Entities.Log
            {
                Bike_id = bike.Id,
                Op_Time = input.Op_Time,
                Type = input.Type,
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now
            });
        }
    }
}