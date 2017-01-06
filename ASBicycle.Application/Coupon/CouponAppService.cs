using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ASBicycle.Bike.Dto;
using ASBicycle.Coupon.Dto;

namespace ASBicycle.Coupon
{
    public class CouponAppService : ASBicycleAppServiceBase, ICouponAppService
    {
        private readonly ICouponReadRepository _couponReadRepository;
        private readonly ICouponWriteRepository _couponWriteRepository;
        private readonly ICouponPkgReadRepository _couponPkgReadRepository;
        private readonly ICouponPkgWriteRepository _couponPkgWriteRepository;
        private readonly ICouponPkgAssReadRepository _couponPkgAssReadRepository;
        private readonly ICouponPkgAssWriteRepository _couponPkgAssWriteRepository;
        private readonly ICouponUserAssReadRepository _couponUserAssReadRepository;
        private readonly ICouponUserAssWriteRepository _couponUserAssWriteRepository;

        public CouponAppService(ICouponReadRepository couponReadRepository, ICouponWriteRepository couponWriteRepository
            , ICouponPkgReadRepository couponPkgReadRepository, ICouponPkgWriteRepository couponPkgWriteRepository
            , ICouponPkgAssReadRepository couponPkgAssReadRepository, ICouponPkgAssWriteRepository couponPkgAssWriteRepository
            , ICouponUserAssReadRepository couponUserAssReadRepository, ICouponUserAssWriteRepository couponUserAssWriteRepository)
        {
            _couponReadRepository = couponReadRepository;
            _couponWriteRepository = couponWriteRepository;
            _couponPkgReadRepository = couponPkgReadRepository;
            _couponPkgWriteRepository = couponPkgWriteRepository;
            _couponPkgAssReadRepository = couponPkgAssReadRepository;
            _couponPkgAssWriteRepository = couponPkgAssWriteRepository;
            _couponUserAssReadRepository = couponUserAssReadRepository;
            _couponUserAssWriteRepository = couponUserAssWriteRepository;
        }


        public async Task<CouponListOutput> GetAllUserCouponList(CouPonInput input)
        {
            var output = new CouponListOutput();
            output.Canuse.Add(new CouponDto
            {
                CouponUserid = 1,
                Type = 1,
                Value = 0.1,
                StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Display = "1.test1\n2.test1",
                Couponpkgname = "礼包1"
            });
            output.Canuse.Add(new CouponDto { CouponUserid = 2, Type = 2, Value = 0.1, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test2\n2.test1", Couponpkgname = "礼包1" });
            output.Beused.Add(new CouponDto { CouponUserid = 3, Type = 1, Value = 0.1, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test3\n2.test1", Couponpkgname = "礼包1" });
            output.Beused.Add(new CouponDto { CouponUserid = 4, Type = 2, Value = 0.1, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test4\n2.test1", Couponpkgname = "礼包1" });
            output.Overdue.Add(new CouponDto { CouponUserid = 5, Type = 1, Value = 0.1, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test5\n2.test1", Couponpkgname = "礼包1" });
            output.Overdue.Add(new CouponDto { CouponUserid = 6, Type = 2, Value = 0.1, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test6\n2.test1", Couponpkgname = "礼包1" });
            return output;
        }

        public async Task<CouponOutput> GetUseUserCouponList(CouPonInput input)
        {
            var output = new CouponOutput();
            output.Canuse.Add(new CouponDto { CouponUserid = 1, Type = 1, Value = 0.1, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test1\n2.test1", Couponpkgname = "礼包1" });
            output.Canuse.Add(new CouponDto { CouponUserid = 2, Type = 2, Value = 0.2, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test2\n2.test1", Couponpkgname = "礼包1" });
            output.Canuse.Add(new CouponDto { CouponUserid = 3, Type = 1, Value = 0.3, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test3\n2.test1", Couponpkgname = "礼包1" });
            output.Canuse.Add(new CouponDto { CouponUserid = 4, Type = 2, Value = 0.4, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test4\n2.test1", Couponpkgname = "礼包1" });
            output.Canuse.Add(new CouponDto { CouponUserid = 5, Type = 1, Value = 0.5, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test5\n2.test1", Couponpkgname = "礼包1" });
            output.Canuse.Add(new CouponDto { CouponUserid = 6, Type = 2, Value = 0.6, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test6\n2.test1", Couponpkgname = "礼包1" });
            return output;
        }

        public async Task GetCouponByCode(CouPonInput input)
        {

        }
    }
}