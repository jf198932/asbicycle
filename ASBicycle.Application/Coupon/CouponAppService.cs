using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using ASBicycle.Bike.Dto;
using ASBicycle.Common;
using ASBicycle.Coupon.Dto;
using ASBicycle.Entities;
using ASBicycle.Track;

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
        private readonly ITrackWriteRepository _trackWriteRepository;

        public CouponAppService(ICouponReadRepository couponReadRepository, ICouponWriteRepository couponWriteRepository
            , ICouponPkgReadRepository couponPkgReadRepository, ICouponPkgWriteRepository couponPkgWriteRepository
            , ICouponPkgAssReadRepository couponPkgAssReadRepository, ICouponPkgAssWriteRepository couponPkgAssWriteRepository
            , ICouponUserAssReadRepository couponUserAssReadRepository, ICouponUserAssWriteRepository couponUserAssWriteRepository
            , ITrackWriteRepository trackWriteRepository)
        {
            _couponReadRepository = couponReadRepository;
            _couponWriteRepository = couponWriteRepository;
            _couponPkgReadRepository = couponPkgReadRepository;
            _couponPkgWriteRepository = couponPkgWriteRepository;
            _couponPkgAssReadRepository = couponPkgAssReadRepository;
            _couponPkgAssWriteRepository = couponPkgAssWriteRepository;
            _couponUserAssReadRepository = couponUserAssReadRepository;
            _couponUserAssWriteRepository = couponUserAssWriteRepository;
            _trackWriteRepository = trackWriteRepository;
        }


        public async Task<CouponListOutput> GetAllUserCouponList(CouPonInput input)
        {
            //获取用户的所有优惠券信息
            var couponlist = await _couponUserAssReadRepository.GetAllListAsync(t => t.user_id == input.user_id);
            //优惠券所属的包id 列表
            var couponpkgass_couponpkgids = couponlist.Select(t => t.coupon_pkg_id ?? 0).ToList();
            //优惠券包 下面的优惠券信息
            var couponpkgasslist =
                await
                    _couponPkgAssReadRepository.GetAllListAsync(
                        t => couponpkgass_couponpkgids.Contains(t.coupon_pkg_id ?? 0));
            //所有优惠券信息按照CouponDto格式
            var coupons = new List<CouponDto>();
            //返回实体
            var output = new CouponListOutput();

            foreach (var item in couponlist)
            {
                var temp =
                    couponpkgasslist.FirstOrDefault(
                        t => t.coupon_pkg_id == item.coupon_pkg_id && t.coupon_id == item.coupon_id);

                coupons.Add(new CouponDto
                {
                    CouponUserid = item.Id.ToString(),
                    Type = item.Coupon.coupon_type.ToString(),
                    Value = item.Coupon.coupon_value.ToString("F"),
                    StartTime = temp == null ? "" : temp.coupon_pkg_enable_time.ToString(),
                    EndTime = temp == null ? "" : temp.coupon_pkg_disable_time.ToString(),
                    UsedTime = item.coupon_use_time.ToString(),
                    Display = item.Coupon.coupon_rule,
                    Couponpkgname = item.CouponPackage.coupon_pkg_name
                });
            }


            output.Beused = coupons.Where(t => t.UsedTime != "").OrderByDescending(t => t.UsedTime).Take(20).ToList();
            output.Canuse =
                coupons.Where(t => t.UsedTime == "" && DateTime.Parse(t.EndTime) >= DateTime.Now)
                    .OrderBy(t => t.EndTime)
                    .Skip((input.index - 1)*20)
                    .Take(20)
                    .ToList();
            output.Overdue = coupons.Where(t => t.UsedTime == "" && DateTime.Parse(t.EndTime) < DateTime.Now).OrderByDescending(t => t.EndTime).Take(20).ToList();
            //output.Canuse.Add(new CouponDto
            //{
            //    CouponUserid = 1,
            //    Type = 1,
            //    Value = 0.1,
            //    StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            //    EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            //    UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            //    Display = "1.test1\n2.test1",
            //    Couponpkgname = "礼包1"
            //});
            //output.Canuse.Add(new CouponDto { CouponUserid = 2, Type = 2, Value = 0.1, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test2\n2.test1", Couponpkgname = "礼包1" });
            //output.Beused.Add(new CouponDto { CouponUserid = 3, Type = 1, Value = 0.1, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test3\n2.test1", Couponpkgname = "礼包1" });
            //output.Beused.Add(new CouponDto { CouponUserid = 4, Type = 2, Value = 0.1, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test4\n2.test1", Couponpkgname = "礼包1" });
            //output.Overdue.Add(new CouponDto { CouponUserid = 5, Type = 1, Value = 0.1, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test5\n2.test1", Couponpkgname = "礼包1" });
            //output.Overdue.Add(new CouponDto { CouponUserid = 6, Type = 2, Value = 0.1, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test6\n2.test1", Couponpkgname = "礼包1" });
            return output;
        }

        public async Task<CouponOutput> GetUseUserCouponList(CouPonInput input)
        {
            //获取用户没有使用过的优惠券信息
            var couponlist = await _couponUserAssReadRepository.GetAllListAsync(t => t.user_id == input.user_id && t.coupon_use_time == null);
            //用户的优惠券包集合
            var couponpkgass_couponpkgids = couponlist.Select(t => t.coupon_pkg_id ?? 0).ToList();
            //优惠券包 下面的优惠券信息
            var couponpkgasslist =
                await
                    _couponPkgAssReadRepository.GetAllListAsync(
                        t => couponpkgass_couponpkgids.Contains(t.coupon_pkg_id ?? 0));
            
            var output = new CouponOutput();

            var alllist = new List<CouponDto>();

            foreach (var item in couponlist)
            {
                var temp =
                    couponpkgasslist.FirstOrDefault(
                        t => t.coupon_pkg_id == item.coupon_pkg_id && t.coupon_id == item.coupon_id);
                if (temp != null && temp.coupon_pkg_disable_time >= DateTime.Now)
                {
                    alllist.Add(new CouponDto
                    {
                        CouponUserid = item.Id.ToString(),
                        Type = item.Coupon.coupon_type.ToString(),
                        Value = item.Coupon.coupon_value.ToString("F"),
                        StartTime = temp.coupon_pkg_enable_time.ToString(),
                        EndTime = temp.coupon_pkg_disable_time.ToString(),
                        UsedTime = item.coupon_use_time.ToString(),
                        Display = item.Coupon.coupon_rule,
                        Couponpkgname = item.CouponPackage.coupon_pkg_name
                    });
                }
                
            }
            CommonHelper.OrderByRule(alllist, input.pay);


            output.Canuse = alllist.Skip((input.index - 1)*20).Take(20).ToList();
            //output.Canuse.Add(new CouponDto { CouponUserid = 1, Type = 1, Value = 0.1, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test1\n2.test1", Couponpkgname = "礼包1" });
            //output.Canuse.Add(new CouponDto { CouponUserid = 2, Type = 2, Value = 0.2, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test2\n2.test1", Couponpkgname = "礼包1" });
            //output.Canuse.Add(new CouponDto { CouponUserid = 3, Type = 1, Value = 0.3, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test3\n2.test1", Couponpkgname = "礼包1" });
            //output.Canuse.Add(new CouponDto { CouponUserid = 4, Type = 2, Value = 0.4, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test4\n2.test1", Couponpkgname = "礼包1" });
            //output.Canuse.Add(new CouponDto { CouponUserid = 5, Type = 1, Value = 0.5, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test5\n2.test1", Couponpkgname = "礼包1" });
            //output.Canuse.Add(new CouponDto { CouponUserid = 6, Type = 2, Value = 0.6, StartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), EndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), UsedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Display = "1.test6\n2.test1", Couponpkgname = "礼包1" });
            return output;
        }

        public async Task GetCouponByCode(CouPonInput input)
        {
            var couponpkg = await _couponPkgWriteRepository.FirstOrDefaultAsync(t => t.redeem_code.Equals(input.code,StringComparison.CurrentCulture));

            if (couponpkg == null)
            {
                throw new UserFriendlyException("兑换码错误");
            }

            // && t.coupon_status == 0 && t.expire_date <= DateTime.Now
            if (couponpkg.coupon_status == 1)
            {
                throw new UserFriendlyException("礼包不可用");
            }
            if (couponpkg.expire_date < DateTime.Now)
            {
                throw new UserFriendlyException("礼包已过期");
            }
            if (couponpkg.upper_limit < 1)
            {
                throw new UserFriendlyException("礼包已领完");
            }
            couponpkg.upper_limit = couponpkg.upper_limit - 1;
            couponpkg.update_time = DateTime.Now;

            var couponpkgass = await _couponPkgAssWriteRepository.GetAllListAsync(t => t.coupon_pkg_id == couponpkg.Id);
            foreach (var item in couponpkgass)
            {
                await _couponUserAssWriteRepository.InsertAsync(new CouponUserAss
                {
                    create_time = DateTime.Now,
                    update_time = DateTime.Now,
                    coupon_id = item.coupon_id,
                    coupon_pkg_id = item.coupon_pkg_id,
                    lead_time = DateTime.Now,
                    user_id = input.user_id
                });
            }
        }

        public RefreshCouponOutput RefreshByCode(RefreshCouponInput input)
        {
            decimal discountDecimal = (decimal) input.discount;
            decimal oldpay = (decimal) input.oldpay;
            var output = new RefreshCouponOutput();
            if (input.type == 1)
            {
                output.newpay = ((oldpay - discountDecimal) < 0 ? 0 : (oldpay - discountDecimal)).ToString("F");
            }
            else if (input.type == 2)
            {
                //(Math.Floor((decimal) double.Parse(output.shouldpay) * decimal.Parse(cp.Value) * 100)/100)
                output.newpay = (Math.Floor(oldpay * discountDecimal*100)/100).ToString("F");
            }
            else
            {
                throw new UserFriendlyException("参数type 输入错误");
                //output.newpay = 0;
            }
            return output;

        }

        public async Task UseCoupon(UseCouponInput input)
        {
            var track = await _trackWriteRepository.FirstOrDefaultAsync(t => t.Pay_docno == input.out_trade_no);
            if (track == null)
            {
                throw new UserFriendlyException("订单不存在");
            }
            track.coupon_id = input.coupon_id;
            track.discount_amount = input.disamount;
            await _trackWriteRepository.UpdateAsync(track);
            var couponuserass = await _couponUserAssWriteRepository.FirstOrDefaultAsync(t => t.Id == input.coupon_id);
            if (couponuserass != null)
            {
                couponuserass.coupon_use_time = DateTime.Now;
                couponuserass.update_time = DateTime.Now;
                await _couponUserAssWriteRepository.UpdateAsync(couponuserass);
            }
        }
    }
}