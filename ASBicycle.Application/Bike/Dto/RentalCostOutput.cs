using System;
using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class RentalCostOutput : IOutputDto
    {
        public RentalCostOutput()
        {
            Coupon = new CouponDto();
            allpay = "0.00";
            discountamount = "0.00";
            shouldpay = "0.00";
        }

        public string out_trade_no { get; set; }
        public string start_site_name { get; set; }
        public string ble_name { get; set; }
        public string ble_serial { get; set; }
        public int ble_type { get; set; }
        public string start_time { get; set; }
        public string end_site_name { get; set; }
        public string end_time { get; set; }
        public string school_name { get; set; }

        public double rental_time { get; set; }
        //优惠后的金额
        public string allpay { get; set; }
        //优惠金额
        public string discountamount { get; set; }
        //优惠前的金额
        public string shouldpay { get; set; }
        public string recharge_count { get; set; }
        public CouponDto Coupon { get; set; }
    }

    public class CouponDto
    {
        /// <summary>
        /// 优惠券ID
        /// </summary>
        public int CouponUserid { get; set; }
        /// <summary>
        /// 1,抵用券 2,折扣券
        /// </summary>
        public int Type { get; set; }
        public double Value { get; set; }
        public string Display { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string UsedTime { get; set; }
        /// <summary>
        /// 礼包名称
        /// </summary>
        public string Couponpkgname { get; set; }
    }
}