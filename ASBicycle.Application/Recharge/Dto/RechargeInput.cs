using Abp.Application.Services.Dto;

namespace ASBicycle.Recharge.Dto
{
    public class RechargeInput : IInputDto
    {
        /// <summary>
        /// 会员id
        /// </summary>
        public int user_id { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public double deposit { get; set; }
        /// <summary>
        /// 1.押金, 2,预充值
        /// </summary>
        public int recharge_type { get; set; }
        /// <summary>
        /// 1.充值，2.退款
        /// </summary>
        public int type { get; set; }
    }
}