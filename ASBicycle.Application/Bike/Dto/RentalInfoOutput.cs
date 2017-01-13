using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class RentalInfoOutput : IOutputDto
    {
        //public RentalInfoOutput()
        //{
        //    Coupon = new CouponDto();
        //    allpay = "0.00";
        //    discountamount = "0.00";
        //    shouldpay = "0.00";
        //}

        public string out_trade_no { get; set; }
        public string pay_method { get; set; }

        public string start_site_name { get; set; }
        public string start_time { get; set; }
        public string end_site_name { get; set; }
        public string end_time { get; set; }

        public string school_name { get; set; }
        public string ble_name { get; set; }

        public string remark { get; set; }

        public double rental_time { get; set; }
        //优惠后的金额
        public string allpay { get; set; }
        //优惠金额
        public string discountamount { get; set; }
        //优惠前的金额
        public string shouldpay { get; set; }
        public string recharge_count { get; set; }

        public string pay_time { get; set; }

        public CouponDto Coupon { get; set; }
    }
}