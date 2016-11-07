using System;
using System.Threading.Tasks;
using ASBicycle.Recharge.Dto;
using ASBicycle.Recharge_detail;

namespace ASBicycle.Recharge
{
    public class RechargeAppService : ASBicycleAppServiceBase, IRechargeAppService
    {
        private readonly IRechargeWriteRepository _rechargeWriteRepository;
        private readonly IRecharge_detailWriteRepository _rechargeDetailWriteRepository;

        public RechargeAppService(IRechargeWriteRepository rechargeWriteRepository,
            IRecharge_detailWriteRepository rechargeDetailWriteRepository)
        {
            _rechargeWriteRepository = rechargeWriteRepository;
            _rechargeDetailWriteRepository = rechargeDetailWriteRepository;
        }

        public async Task<RechargeOutput> CreateRecharge(RechargeInput input)
        {
            var recharge = await _rechargeWriteRepository.GetAllListAsync(t => t.User_id == input.user_id);
            if (recharge == null || recharge.Count == 0)
            {
                await _rechargeWriteRepository.InsertAsync(new Entities.Recharge
                {
                    Created_at = DateTime.Now,
                    Updated_at = DateTime.Now,
                    User_id = input.user_id,
                    //Deposit = input.deposit
                });
            }
            
            var paydocno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
            await _rechargeDetailWriteRepository.InsertAsync(new Entities.Recharge_detail
            {
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now,
                User_id = input.user_id,
                recharge_docno = paydocno,
                Type = input.type,
                Recharge_type = input.recharge_type,
            });
            return new RechargeOutput {out_trade_no = paydocno};
        }
    }
}