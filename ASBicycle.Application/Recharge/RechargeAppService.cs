using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
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
            //var refound = await _refoundWriteRepository.FirstOrDefaultAsync(t => t.User_id == input.User_id);
            //if (refound != null)
            //{
            //    refound.Refound_status = 1;
            //    refound.Updated_at = DateTime.Now;
            //    await _refoundWriteRepository.UpdateAsync(refound);
            //}
            //else
            //{
            //    await _refoundWriteRepository.InsertAsync(new Entities.Refound
            //    {
            //        Created_at = DateTime.Now,
            //        Updated_at = DateTime.Now,
            //        Refound_status = 1,
            //        User_id = input.User_id,
            //        Refound_amount = input.Amount
            //    });
            //}

            var details =
                await
                    _rechargeDetailWriteRepository.GetAllListAsync(
                        t => t.User_id == input.User_id && t.Type == 1 && t.Recharge_type == input.Recharge_type && t.status == 0);


            foreach (var rechargeDetail in details)
            {
                if (rechargeDetail == null)
                {
                    throw new UserFriendlyException("没有可退款的订单");
                }
                if (rechargeDetail.Recharge_type == 1)
                {
                    rechargeDetail.status = 1;
                    rechargeDetail.Updated_at = DateTime.Now;
                    await _rechargeDetailWriteRepository.UpdateAsync(rechargeDetail);
                }
            }
            
            //var paydocno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
            //await _rechargeDetailWriteRepository.InsertAsync(new Entities.Recharge_detail
            //{
            //    Created_at = DateTime.Now,
            //    Updated_at = DateTime.Now,
            //    User_id = input.User_id,
            //    recharge_docno = paydocno,
            //    Type = 2,
            //    Recharge_type = input.Recharge_type,
            //});
            //throw new NotImplementedException();
        }
    }
}