namespace ASBicycle.User.Dto
{
    public class BillOutput
    {
        /// <summary>
        /// 类型，充值，退款，租车
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 支付方式  支付宝，微信
        /// </summary>
        public string PayMethod { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public string Docno { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string Payment { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public string PayTime { get; set; }
    }
}