using System;
using System.Threading.Tasks;
using ASBicycle.Recharge.Dto;
using ASBicycle.Recharge_detail;
using ASBicycle.Refound;

namespace ASBicycle.Recharge
{
    public class RechargeAppService : ASBicycleAppServiceBase, IRechargeAppService
    {
        private readonly IRechargeWriteRepository _rechargeWriteRepository;
        private readonly IRecharge_detailWriteRepository _rechargeDetailWriteRepository;
        private readonly IRefoundWriteRepository _refoundWriteRepository;

        public RechargeAppService(IRechargeWriteRepository rechargeWriteRepository,
            IRecharge_detailWriteRepository rechargeDetailWriteRepository,
            IRefoundWriteRepository refoundWriteRepository)
        {
            _rechargeWriteRepository = rechargeWriteRepository;
            _rechargeDetailWriteRepository = rechargeDetailWriteRepository;
            _refoundWriteRepository = refoundWriteRepository;
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

        public async Task ApplyRefound(RefoundInput input)
        {
            await _refoundWriteRepository.InsertAsync(new Entities.Refound
            {
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now,
                Refound_status = 1,
                User_id = input.User_id,
                Refound_amount = input.Amount
            });
            //throw new NotImplementedException();
        }
    }
}