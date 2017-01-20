using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Abp.AutoMapper;
using Abp.UI;
using ASBicycle.Bike.Dto;
using System.Linq;
using System.Text;
using Abp.Extensions;
using Abp.Logging;
using ASBicycle.Bikesite;
using ASBicycle.Common;
using ASBicycle.Coupon;
using ASBicycle.Recharge;
using ASBicycle.Track;

namespace ASBicycle.Bike
{
    public class BikeAppService : ASBicycleAppServiceBase, IBikeAppService
    {
        private readonly IBikeWriteRepository _bikeRepository;
        private readonly IBikeReadRepository _bikeReadRepository;
        private readonly ITrackWriteRepository _trackRepository;
        private readonly ITrackReadRepository _trackReadRepository;
        private readonly IRechargeWriteRepository _rechargeWriteRepository;
        private readonly ISqlExecuter _sqlExecuter;
        private readonly ISqlReadExecuter _sqlReadExecuter;
        private readonly IBikesiteWriteRepository _bikesiteRepository;
        private readonly IBikesiteReadRepository _bikesiteReadRepository;
        private readonly ICouponUserAssReadRepository _couponUserAssReadRepository;
        private readonly ICouponUserAssWriteRepository _couponUserAssWriteRepository;
        private readonly ICouponPkgAssReadRepository _couponPkgAssReadRepository;
        private readonly ICouponPkgAssWriteRepository _couponPkgAssWriteRepository;

        public BikeAppService(IBikeWriteRepository bikeRepository, IBikeReadRepository bikeReadRepository
            , ITrackWriteRepository trackRepository, ITrackReadRepository trackReadRepository
            , ISqlExecuter sqlExecuter, ISqlReadExecuter sqlReadExecuter
            , IBikesiteWriteRepository bikesiteRepository, IBikesiteReadRepository bikesiteReadRepository
            , IRechargeWriteRepository rechargeWriteRepository
            , ICouponPkgAssReadRepository couponPkgAssReadRepository, ICouponPkgAssWriteRepository couponPkgAssWriteRepository
            , ICouponUserAssReadRepository couponUserAssReadRepository, ICouponUserAssWriteRepository couponUserAssWriteRepository)
        {
            _bikeRepository = bikeRepository;
            _trackRepository = trackRepository;
            _trackReadRepository = trackReadRepository;
            _sqlExecuter = sqlExecuter;
            _bikesiteRepository = bikesiteRepository;
            _bikeReadRepository = bikeReadRepository;
            _rechargeWriteRepository = rechargeWriteRepository;
            _sqlReadExecuter = sqlReadExecuter;
            _bikesiteReadRepository = bikesiteReadRepository;
            _couponUserAssReadRepository = couponUserAssReadRepository;
            _couponUserAssWriteRepository = couponUserAssWriteRepository;
            _couponPkgAssReadRepository = couponPkgAssReadRepository;
            _couponPkgAssWriteRepository = couponPkgAssWriteRepository;
        }
        /// <summary>
        /// 获取车辆异常信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<AlarmBikeOutput> GetAlarmBikeWay([FromUri] BikegetInput input)
        {
            var bike = await _bikeReadRepository.FirstOrDefaultAsync(t => t.User.Phone == input.phone);
            var model = new AlarmBikeOutput();
            if (bike == null)
            {
                throw new UserFriendlyException("请先进行绑定");
            }
            model.bikename = bike.Ble_name;
            model.bikeimg = bike.Bike_img;

            var sqlstr =
                "select b.gps_point,b.`name` as sitename,DATE_FORMAT(op_time,'%Y-%m-%d %H:%i:%S') as alarmtime,bikesite_id,bike_id from log as l JOIN bikesite as b on l.bikesite_id = b.id WHERE op_time >= (select op_time from log where type = 3 and bike_id = " + bike.Id+" ORDER BY op_time DESC LIMIT 1) and l.bike_id="+bike.Id+" order by op_time desc";
            model.alarmlist = _sqlReadExecuter.SqlQuery<AlarmBikeDto>(sqlstr).ToList();
            foreach (var item in model.alarmlist)
            {
                if (!item.gps_point.IsNullOrEmpty())
                {
                    var tempgps = item.gps_point.Split(',');
                    if (tempgps.Length == 2)
                    {
                        item.lon = double.Parse(tempgps[0], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
                        item.lat = double.Parse(tempgps[1], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
                    }
                    else
                    {
                        Logger.Debug("GetAlarmBikeWay-79");
                    }
                }
            }
            return model;
        }
        /// <summary>
        /// 获取车辆信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BikeOutput> GetBikeInfo([FromUri]BikegetInput input)
        {
            var model = await _bikeReadRepository.FirstOrDefaultAsync(b => b.Ble_name == input.serial);
            if(model == null)
                throw new UserFriendlyException("没有该车辆");
            var result = model.MapTo<BikeOutput>();
            //result.School_name = model.User.School == null ? "":model.User.School.Name;
            result.Bikesite_name = model.Bikesite == null ? "":model.Bikesite.Name;

            return result;
        }
        /// <summary>
        /// 更新车辆
        /// </summary>
        /// <param name="bikeInput"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task UpdateBike(BikeInput bikeInput)
        {
            var bike = await _bikeRepository.FirstOrDefaultAsync(b => b.Ble_name == bikeInput.Serial);
            if(bikeInput.Vlock_status != null)
                bike.Vlock_status = bikeInput.Vlock_status;
            if(bikeInput.Bike_status != null)
                bike.Bike_status = bikeInput.Bike_status;
            if(!bikeInput.Bike_img.IsNullOrEmpty())
                bike.Bike_img = bikeInput.Bike_img;
            if(bikeInput.Battery > 0)
                bike.Battery = bikeInput.Battery;
            if(bikeInput.Bikesite_id != null)
                bike.Bikesite_id = bikeInput.Bikesite_id;
            bike.Position = bikeInput.Position;



            await _bikeRepository.UpdateAsync(bike);
        }
        /// <summary>
        /// 上传车辆图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public BikeUploadOutput UploadBikePic()
        {
            HttpPostedFile file = HttpContext.Current.Request.Files["filedata"];
            if (file != null)
            {
                try
                {
                    // 文件上传后的保存路径
                    string filePath = HttpContext.Current.Server.MapPath("~/Uploads/Bike/");
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fileName = Path.GetFileName(file.FileName);// 原始文件名称
                    string fileExtension = Path.GetExtension(fileName); // 文件扩展名
                    string saveName = Guid.NewGuid() + fileExtension; // 保存文件名称

                    file.SaveAs(filePath + saveName);



                    var img = ConfigurationManager.AppSettings["ServerPath"] + "Uploads/Bike/" + saveName;

                    var result = new BikeUploadOutput
                    {
                        ImgUrl = img
                    };
                    return result;
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(ex.Message);

                }
            }
            else
            {
                throw new UserFriendlyException("请选择一张图片上传");
            }

        }
        /// <summary>
        /// 租车
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RentalBikeOutput> RentalBiketemp(RentalBikeInput input)
        {
            var bike =await
                    _bikeRepository.FirstOrDefaultAsync(
                        t => t.Ble_name == input.Ble_name && t.rent_type == 1);
            
            if (bike == null || bike.Ble_type < 3)
            {
                throw new UserFriendlyException("没有该车辆或该车不可租!");
            }
            //if (bike.Bike_status == 0)
            //{
            //    throw new UserFriendlyException("该车辆出租中");
            //}

            if (bike.Bike_status == 0 && bike.Ble_type != 3)
            {
                throw new UserFriendlyException("被租赁中!");
            }

            var gpsinput = input.gps_point.Trim().Split(',');

            double ip_lon = 0;

            double ip_lat = 0;

            if (gpsinput.Length == 2)
            {
                ip_lon = double.Parse(gpsinput[0].Trim(), NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
                //LogHelper.Logger.Info(ip_lon.ToString);
                ip_lat = double.Parse(gpsinput[1].Trim(), NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
            }
            var bikesitelist = await _bikesiteRepository.GetAllListAsync(t => t.School_id == bike.School_id && t.Enable && t.Type == 3);

            Entities.Bikesite bsite = null;
            //Dictionary<string,string> temp = new Dictionary<string, string>();
            double mindistance = 99999999;
            //LogHelper.Logger.Info(mindistance.ToString);
            foreach (var bikesite in bikesitelist)
            {
                var bikesitegps = bikesite.Gps_point.Split(',');
                double bs_lon = 0;
                double bs_lat = 0;
                if (bikesitegps.Length == 2)
                {
                    bs_lon = double.Parse(bikesitegps[0].Trim(), NumberStyles.Any,
                        CultureInfo.CreateSpecificCulture("zh-cn"));
                    bs_lat = double.Parse(bikesitegps[1].Trim(), NumberStyles.Any,
                        CultureInfo.CreateSpecificCulture("zh-cn"));
                }
                else
                {
                    Logger.Debug("RentalBiketemp-490-" + bikesite.Id);
                }

                var distance = LatlonHelper.GetDistance(ip_lat, ip_lon, bs_lat, bs_lon) * 1000;//KM->M

                //LogHelper.Logger.Info(distance.ToString);

                if (distance < mindistance && distance <= bikesite.Radius)//
                {
                    mindistance = distance;
                    bsite = bikesite;
                }
            }

            if (bsite == null)
            {
                throw new UserFriendlyException("范围内没有桩点");
            }


            string bluetoothpwd = "";
            if (bike.Ble_type == 3)
            {
                //蓝牙锁密码逻辑//ble_searl  后2位 第一位转换10进制  *2，再除16 取余 再转16进制，第二位同
                var ts = bike.Ble_serial.Substring(bike.Ble_serial.Length - 2, 2);
                var first = ((int.Parse(ts.Substring(0, 1), System.Globalization.NumberStyles.AllowHexSpecifier) * 2) % 16).ToString("X");
                var second = ((int.Parse(ts.Substring(1, 1), System.Globalization.NumberStyles.AllowHexSpecifier) * 2) % 16).ToString("X");
                bluetoothpwd = first + second;

                var endtime = DateTime.Now;
                string ly_lock = "";
                string ctm = "";
                if (!input.isrstr.IsNullOrEmpty())
                {
                    //var ly_work = input.isrstr.Substring(15, 1);//作状态指示字符（N/S），日常状态；S表示selected，选择状态
                    ly_lock = input.isrstr.Substring(16, 1);//车锁状态指示字符（L/O/M）L表示locked，锁闭状态；O表示opened，打开状态；M表示malfunction，异常状态
                    var battery = input.isrstr.Substring(17, 2);//电量
                    ctm = input.isrstr.Substring(19, 4);//时间 分钟 
                    bike.Battery = int.Parse(battery);
                }
                //结算没完成的订单
                var no_pay_track =
                    await _trackRepository.FirstOrDefaultAsync(t => t.Bike_id == bike.Id && t.Pay_status == 1);
                if (no_pay_track != null)
                {
                    if (ly_lock != "" && ly_lock.ToUpper() == "L" && ctm != "")
                    {
                        endtime = no_pay_track.Start_time.Value.AddMinutes(int.Parse(ctm, System.Globalization.NumberStyles.AllowHexSpecifier));
                    }

                    no_pay_track.End_point = input.gps_point;
                    no_pay_track.End_site_id = bsite.Id;
                    no_pay_track.End_time = endtime;
                    no_pay_track.Updated_at = endtime;
                    no_pay_track.Pay_status = 2; //还车未支付
                    await _trackRepository.UpdateAsync(no_pay_track);

                    bike.Bike_status = 1;
                    await _bikeRepository.UpdateAsync(bike);

                    bsite.Available_count = bsite.Available_count + 1;
                    await _bikesiteRepository.UpdateAsync(bsite);

                    await CurrentUnitOfWork.SaveChangesAsync();
                }

                return new RentalBikeOutput
                {
                    ble_name = input.Ble_name,
                    ble_serial = bike.Ble_serial,
                    pwd = bike.Ble_type == 3 ? bluetoothpwd : bike.Lock_pwd,
                    BikesiteList = bikesitelist.MapTo<List<BikesiteEntity>>()
                };
            }
            

            var paydocno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
            var startdate = DateTime.Now;
            
            //pay_status 1  租车未还车
            var sqlstr =
                "insert into track(bike_id,user_id,created_at,updated_at,start_site_id,start_point,start_time,pay_docno,pay_status) values(" + bike.Id + "," + input.user_id + ",'" + startdate.ToString("yyyy/MM/dd HH:mm:ss") + "','" + startdate.ToString("yyyy/MM/dd HH:mm:ss") + "'," + bsite.Id + ",'" + input.gps_point + "','" + startdate.ToString("yyyy/MM/dd HH:mm:ss") + "','" + paydocno + "',1)";
            await _sqlExecuter.ExecuteAsync(sqlstr);

            

            var gps = bsite.Gps_point;
            var gpss = gps.Split(',');
            double gpss1 = 0;
            double gpss2 = 0;
            if (gpss.Length == 2)
            {
                gpss1 = double.Parse(gpss[0], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
                gpss2 = double.Parse(gpss[1], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
            }
            var output = new RentalBikeOutput
            {
                out_trade_no = paydocno,
                lon = gpss1,
                lat = gpss2,
                start_site_name = bsite.Name,
                ble_name = input.Ble_name,
                ble_serial = bike.Ble_serial,
                start_time = startdate.ToString("yyyy/MM/dd HH:mm:ss"),
                pwd = bike.Ble_type == 3 ? bluetoothpwd : bike.Lock_pwd,
                BikesiteList = bikesitelist.MapTo<List<BikesiteEntity>>()
            };
            bike.Bike_status = 0; //出租中
            
            await _bikeRepository.UpdateAsync(bike);
            bsite.Available_count = bsite.Available_count - 1;
            await _bikesiteRepository.UpdateAsync(bsite);
            return output;
        }
        /// <summary>
        /// 结束租车（正常）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RentalCostOutput> RentalBikeFinishtemp(RentalBikeInput input)
        {
            var track = await _trackRepository.FirstOrDefaultAsync(
                    t => t.Pay_docno == input.out_trade_no && t.User_id == input.user_id);
            if (track == null)
            {
                throw new UserFriendlyException("无该订单号!");
            }
            if (track.Pay_status > 1)
            {
                throw new UserFriendlyException("已经还过车了!");
            }

            var bike = await _bikeRepository.FirstOrDefaultAsync(t => t.Ble_name == input.Ble_name);

            var gpsinput = input.gps_point.Split(',');
            double ip_lon = 0;
            double ip_lat = 0;
            if (gpsinput.Length == 2)
            {
                ip_lon = double.Parse(gpsinput[0], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
                ip_lat = double.Parse(gpsinput[1], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
            }
            
            var bikesitelist = await _bikesiteRepository.GetAllListAsync(t => t.School_id == bike.School_id && t.Enable && t.Type == 3);

            Entities.Bikesite bsite = null;
            double mindistance = 99999999;
            foreach (var bikesite in bikesitelist)
            {
                var bikesitegps = bikesite.Gps_point.Split(',');
                double bs_lon = 0;
                double bs_lat = 0;
                if (bikesitegps.Length == 2)
                {
                    bs_lon = double.Parse(bikesitegps[0], NumberStyles.Any,
                        CultureInfo.CreateSpecificCulture("zh-cn"));
                    bs_lat = double.Parse(bikesitegps[1], NumberStyles.Any,
                        CultureInfo.CreateSpecificCulture("zh-cn"));
                }
                else
                {
                    Logger.Debug("RentalBikeFinishtemp-602-"+bikesite.Id);
                }
                var distance = LatlonHelper.GetDistance(ip_lat, ip_lon, bs_lat, bs_lon) * 1000;//KM->M
                if (distance < mindistance && distance <= bikesite.Radius)//
                {
                    mindistance = distance;
                    bsite = bikesite;
                }
            }
            
            if (bsite == null)
            {
                throw new UserFriendlyException("范围内没有桩点");
            }
            var endtime = DateTime.Now;
            track.End_point = input.gps_point;
            track.End_site_id = bsite.Id;
            track.End_time = endtime;
            track.Updated_at = endtime;
            track.Pay_status = 2;//还车未支付
            await _trackRepository.UpdateAsync(track);

            bike.Bike_status = 1;
            bike.Position = input.gps_point;
            await _bikeRepository.UpdateAsync(bike);

            bsite.Available_count = bsite.Available_count + 1;
            await _bikesiteRepository.UpdateAsync(bsite);

            await CurrentUnitOfWork.SaveChangesAsync();
            
            StringBuilder sb = new StringBuilder();
            sb.Append(
                "select a.id,a.created_at,a.updated_at,a.user_id,a.bike_id,e.ble_name,a.start_point,a.end_point,a.start_site_id,a.end_site_id" +
                ",a.start_time,a.end_time,a.payment,a.pay_status,a.pay_method,a.pay_docno,a.remark,b.`name` as start_site_name,d.`name` as end_site_name" +
                ",a.start_point, c.`name` as school_name,c.time_charge, e.Lock_pwd, c.free_time, c.fixed_amount, c.top_amount, e.ble_type, e.ble_serial");
            sb.Append(" from track as a left join bikesite as b on a.start_site_id = b.id");
            sb.Append(" left join school as c on b.school_id = c.id");
            sb.Append(" left join bikesite as d on a.end_site_id = d.id");
            sb.Append(" left join bike as e on a.bike_id = e.id");
            sb.AppendFormat(" where a.pay_docno='{0}'", input.out_trade_no);
            var tracktemp = _sqlExecuter.SqlQuery<TrackEntity>(sb.ToString()).ToList().FirstOrDefault();
            if (tracktemp == null)
            {
                throw new UserFriendlyException("没有行程单!");
            }
            var output = new RentalCostOutput();

            //var end_time = DateTime.Now;

            output.out_trade_no = tracktemp.Pay_docno;
            output.start_site_name = tracktemp.Start_site_name;
            output.ble_name = tracktemp.Ble_name;
            output.start_time = tracktemp.Start_time.ToString();
            output.end_site_name = tracktemp.End_site_name;
            output.end_time = tracktemp.End_time.ToString();
            output.school_name = tracktemp.School_name;
            output.ble_type = tracktemp.Ble_type;
            output.ble_serial = tracktemp.Ble_serial;
            
            //var recharge = await _rechargeWriteRepository.FirstOrDefaultAsync(t => t.User_id == track.User_id);
            //if (recharge == null)
            //{
            //    output.recharge_count = "0.00";
            //}
            //else
            //{
            //    if (recharge.Recharge_count != null)
            //    {
            //        output.recharge_count = ((double) recharge.Recharge_count).ToString("F");
            //    }
            //    else
            //    {
            //        output.recharge_count = "0.00";
            //    }
            //}


            TimeSpan costtime = endtime - DateTime.Parse(tracktemp.Start_time.ToString());
            TimeSpan costday = endtime.Date - DateTime.Parse(tracktemp.Start_time.ToString()).Date;

            //var ctm = (int)Math.Ceiling(costtime.TotalMinutes);//去掉多余的零头
            //var day = (int)Math.Ceiling(costday.TotalDays);

            //output.rental_time = ctm;

            var ctm = (int)Math.Floor(costtime.TotalMinutes);//去掉多余的零头
            var day = (int)Math.Ceiling(costday.TotalDays);
            if (costtime.TotalMinutes < 1)
            {
                output.rental_time = Math.Ceiling(costtime.TotalSeconds) / 100;
            }
            else
            {
                output.rental_time = ctm;
            }

            if (ctm < tracktemp.Free_time)
            {
                output.shouldpay = tracktemp.Fixed_amount.ToString("F");
                output.allpay = tracktemp.Fixed_amount.ToString("F");
                track.Should_pay = tracktemp.Fixed_amount;
            }
            //else if (ctm > tracktemp.Free_time && day == 0)
            //{
            //    var tpay = (ctm - tracktemp.Free_time) * tracktemp.time_charge / 100.00 + tracktemp.Fixed_amount;
            //    output.allpay = tpay <= tracktemp.Top_amount ? tpay.ToString() : tracktemp.Top_amount.ToString();
            //    track.Should_pay = tpay <= tracktemp.Top_amount ? tpay : tracktemp.Top_amount;
            //}
            else
            {
                //TimeSpan fcosttime = DateTime.Parse(tracktemp.Start_time.ToString()).Date.AddDays(1) - DateTime.Parse(tracktemp.Start_time.ToString());
                //TimeSpan ecosttime = endtime - endtime.Date;

                //var ftpay = ((int)Math.Ceiling(fcosttime.TotalMinutes) - tracktemp.Free_time) * tracktemp.time_charge / 100.00 + tracktemp.Fixed_amount;//分转元
                //var etpay = ((int)Math.Ceiling(ecosttime.TotalMinutes) - tracktemp.Free_time) * tracktemp.time_charge / 100.00 + tracktemp.Fixed_amount;//分转元
                //var firstpay = ftpay <= tracktemp.Top_amount ? ftpay : tracktemp.Top_amount;
                //var endpay = (int)Math.Ceiling(ecosttime.TotalMinutes) > tracktemp.Free_time ? tracktemp.Top_amount : etpay;

                //output.allpay = (firstpay + tracktemp.Top_amount * day + endpay).ToString();//分转元
                //track.Should_pay = firstpay + tracktemp.Top_amount * day + endpay;//分转元

                output.shouldpay = (((ctm - tracktemp.Free_time) * tracktemp.time_charge / 100.00) + tracktemp.Fixed_amount).ToString("F");//分转元
                output.allpay = (((ctm - tracktemp.Free_time) * tracktemp.time_charge / 100.00) + tracktemp.Fixed_amount).ToString("F");//分转元
                track.Should_pay = ((ctm - tracktemp.Free_time) * tracktemp.time_charge / 100.00) + tracktemp.Fixed_amount;//分转元
            }
            if (track.Should_pay > 0)
            {
                var coupons = new List<CouponDto>();
                //获取用户没有使用过的优惠券信息
                var couponlist =
                    await
                        _couponUserAssWriteRepository.GetAllListAsync(
                            t => t.user_id == input.user_id && t.coupon_use_time == null);
                //用户的优惠券包集合
                var couponpkgass_couponpkgids = couponlist.Select(t => t.coupon_pkg_id ?? 0).ToList();
                //优惠券包 下面的优惠券信息
                var couponpkgasslist =
                    await
                        _couponPkgAssWriteRepository.GetAllListAsync(
                            t => couponpkgass_couponpkgids.Contains(t.coupon_pkg_id ?? 0));
                foreach (var item in couponlist)
                {
                    var temp =
                        couponpkgasslist.FirstOrDefault(
                            t => t.coupon_pkg_id == item.coupon_pkg_id && t.coupon_id == item.coupon_id);
                    if (temp != null && temp.coupon_pkg_disable_time.Value.AddDays(1) >= DateTime.Now)
                    {
                        coupons.Add(new CouponDto
                        {
                            CouponUserid = item.Id.ToString(),
                            Type = item.Coupon.coupon_type.ToString(),
                            Value = item.Coupon.coupon_value.ToString("F"),
                            UsedTime = item.coupon_use_time.ToString(),
                            Display = item.Coupon.coupon_rule,
                            Couponpkgname = item.CouponPackage.coupon_pkg_name
                        });
                    }
                }
                CommonHelper.OrderByRule(coupons, double.Parse(output.shouldpay));
                var cp = coupons.FirstOrDefault();
                if (cp != null)
                {
                    output.Coupon = new CouponDto();
                    output.Coupon.Type = cp.Type;
                    output.Coupon.Value = cp.Value;
                    output.Coupon.CouponUserid = cp.CouponUserid;
                    output.Coupon.Couponpkgname = cp.Couponpkgname;
                    if (cp.Type == "1")
                    {
                        //output.allpay =
                        //    (((decimal) double.Parse(output.shouldpay) - (decimal) cp.Value) < 0
                        //        ? 0
                        //        : ((decimal) double.Parse(output.shouldpay) - (decimal) cp.Value)).ToString("F");
                        output.allpay = (Math.Floor(((decimal) double.Parse(output.shouldpay) - decimal.Parse(cp.Value))*
                                                    100) < 0
                            ? 0
                            : Math.Floor((decimal.Parse(output.shouldpay) - decimal.Parse(cp.Value))*100)/100).ToString(
                                "F");
                        output.discountamount =
                            ((decimal) double.Parse(output.shouldpay) - (decimal) double.Parse(output.allpay)).ToString(
                                "F");
                        //track.discount_amount =
                        //    (double) ((decimal) double.Parse(output.shouldpay) - (decimal) double.Parse(output.allpay));
                    }
                    else if (cp.Type == "2")
                    {
                        //output.allpay = ((decimal) double.Parse(output.shouldpay)*(decimal) cp.Value).ToString("F");
                        output.allpay =
                            (Math.Floor((decimal) double.Parse(output.shouldpay)*decimal.Parse(cp.Value)*100)/100)
                                .ToString("F");
                        output.discountamount =
                            ((decimal) double.Parse(output.shouldpay) - (decimal) double.Parse(output.allpay)).ToString(
                                "F");
                        //track.discount_amount =
                        //    (double) ((decimal) double.Parse(output.shouldpay) - (decimal) double.Parse(output.allpay));
                    }
                }
            }
            await _trackRepository.UpdateAsync(track);
            return output;
        }
        /// <summary>
        /// 结束租车（强制）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RentalCostOutput> RentalBikeForcedFinishtemp(RentalBikeInput input)
        {
            var track = await _trackRepository.FirstOrDefaultAsync(
                    t => t.Pay_docno == input.out_trade_no && t.User_id == input.user_id);
            if (track == null)
            {
                throw new UserFriendlyException("无该订单号!");
            }
            var bike = await _bikeRepository.FirstOrDefaultAsync(t => t.Ble_name == input.Ble_name);
            
            var endtime = DateTime.Now;
            track.End_point = input.gps_point;
            track.End_site_id = 0;
            track.End_time = endtime;
            track.Updated_at = endtime;
            track.Pay_status = 2;//还车未支付
            await _trackRepository.UpdateAsync(track);

            bike.Bike_status = 1;
            bike.Position = input.gps_point;
            await _bikeRepository.UpdateAsync(bike);

            //bsite.Available_count = bsite.Available_count + 1;
            //await _bikesiteRepository.UpdateAsync(bsite);

            await CurrentUnitOfWork.SaveChangesAsync();

            StringBuilder sb = new StringBuilder();
            sb.Append(
                "select a.id,a.created_at,a.updated_at,a.user_id,a.bike_id,e.ble_name,a.start_point,a.end_point,a.start_site_id,a.end_site_id" +
                ",a.start_time,a.end_time,a.payment,a.pay_status,a.pay_method,a.pay_docno,a.remark,b.`name` as start_site_name,d.`name` as end_site_name" +
                ",a.start_point, c.`name` as school_name,c.time_charge, c.free_time, c.fixed_amount, c.top_amount, e.ble_type, e.ble_serial");
            sb.Append(" from track as a left join bikesite as b on a.start_site_id = b.id");
            sb.Append(" left join school as c on b.school_id = c.id");
            sb.Append(" left join bikesite as d on a.end_site_id = d.id");
            sb.Append(" left join bike as e on a.bike_id = e.id");
            sb.AppendFormat(" where a.pay_docno='{0}'", input.out_trade_no);
            var tracktemp = _sqlExecuter.SqlQuery<TrackEntity>(sb.ToString()).ToList().FirstOrDefault();
            if (tracktemp == null)
            {
                throw new UserFriendlyException("没有行程单!");
            }
            var output = new RentalCostOutput();

            //var end_time = DateTime.Now;

            output.out_trade_no = tracktemp.Pay_docno;
            output.start_site_name = tracktemp.Start_site_name;
            output.ble_name = tracktemp.Ble_name;
            output.start_time = tracktemp.Start_time.ToString();
            output.end_site_name = tracktemp.End_site_name;
            output.end_time = tracktemp.End_time.ToString();
            output.school_name = tracktemp.School_name;
            output.ble_type = tracktemp.Ble_type;
            output.ble_serial = tracktemp.Ble_serial;
            //var recharge = await _rechargeWriteRepository.FirstOrDefaultAsync(t => t.User_id == track.User_id);
            //if (recharge == null)
            //{
            //    output.recharge_count = "0.00";
            //}
            //else
            //{
            //    if (recharge.Recharge_count != null)
            //    {
            //        output.recharge_count = ((double)recharge.Recharge_count).ToString("F");
            //    }
            //    else
            //    {
            //        output.recharge_count = "0.00";
            //    }
            //}

            TimeSpan costtime = endtime - DateTime.Parse(tracktemp.Start_time.ToString());
            TimeSpan costday = endtime.Date - DateTime.Parse(tracktemp.Start_time.ToString()).Date;

            var ctm = (int)Math.Floor(costtime.TotalMinutes);//去掉多余的零头
            var day = (int)Math.Ceiling(costday.TotalDays);
            if (costtime.TotalMinutes < 1)
            {
                output.rental_time = Math.Ceiling(costtime.TotalSeconds) / 100;
            }
            else
            {
                output.rental_time = ctm;
            }

            if (ctm < tracktemp.Free_time)
            {
                output.allpay = tracktemp.Fixed_amount.ToString("F");
                output.shouldpay = tracktemp.Fixed_amount.ToString("F");
                track.Should_pay = tracktemp.Fixed_amount;
            }
            //else if (ctm > tracktemp.Free_time && day == 0)
            //{
            //    var tpay = (ctm - tracktemp.Free_time) * tracktemp.time_charge / 100.00 + tracktemp.Fixed_amount;
            //    output.allpay = tpay <= tracktemp.Top_amount ? tpay.ToString() : tracktemp.Top_amount.ToString();
            //    track.Should_pay = tpay <= tracktemp.Top_amount ? tpay : tracktemp.Top_amount;
            //}
            else
            {
                //TimeSpan fcosttime = DateTime.Parse(tracktemp.Start_time.ToString()).Date.AddDays(1) - DateTime.Parse(tracktemp.Start_time.ToString());
                //TimeSpan ecosttime = endtime - endtime.Date;

                //var ftpay = ((int)Math.Ceiling(fcosttime.TotalMinutes) - tracktemp.Free_time) * tracktemp.time_charge / 100.00 + tracktemp.Fixed_amount;//分转元
                //var etpay = ((int)Math.Ceiling(ecosttime.TotalMinutes) - tracktemp.Free_time) * tracktemp.time_charge / 100.00 + tracktemp.Fixed_amount;//分转元
                //var firstpay = ftpay <= tracktemp.Top_amount ? ftpay : tracktemp.Top_amount;
                //var endpay = (int)Math.Ceiling(ecosttime.TotalMinutes) > tracktemp.Free_time ? tracktemp.Top_amount : etpay;

                //output.allpay = (firstpay + tracktemp.Top_amount * day + endpay).ToString();//分转元
                //track.Should_pay = firstpay + tracktemp.Top_amount * day + endpay;//分转元
                output.allpay = ((ctm - tracktemp.Free_time) * tracktemp.time_charge / 100.00).ToString("F");//分转元
                output.shouldpay = ((ctm - tracktemp.Free_time) * tracktemp.time_charge / 100.00).ToString("F");//分转元
                track.Should_pay = (ctm - tracktemp.Free_time) * tracktemp.time_charge / 100.00;//分转元
            }

            if (track.Should_pay > 0)
            {

                var coupons = new List<CouponDto>();
                //获取用户没有使用过的优惠券信息
                var couponlist =
                    await
                        _couponUserAssWriteRepository.GetAllListAsync(
                            t => t.user_id == input.user_id && t.coupon_use_time == null);
                //用户的优惠券包集合
                var couponpkgass_couponpkgids = couponlist.Select(t => t.coupon_pkg_id ?? 0).ToList();
                //优惠券包 下面的优惠券信息
                var couponpkgasslist =
                    await
                        _couponPkgAssWriteRepository.GetAllListAsync(
                            t => couponpkgass_couponpkgids.Contains(t.coupon_pkg_id ?? 0));
                foreach (var item in couponlist)
                {
                    var temp =
                        couponpkgasslist.FirstOrDefault(
                            t => t.coupon_pkg_id == item.coupon_pkg_id && t.coupon_id == item.coupon_id);
                    if (temp != null && temp.coupon_pkg_disable_time.Value.AddDays(1) >= DateTime.Now)
                    {
                        coupons.Add(new CouponDto
                        {
                            CouponUserid = item.Id.ToString(),
                            Type = item.Coupon.coupon_type.ToString(),
                            Value = item.Coupon.coupon_value.ToString("F"),
                            UsedTime = item.coupon_use_time.ToString(),
                            Display = item.Coupon.coupon_rule,
                            Couponpkgname = item.CouponPackage.coupon_pkg_name
                        });
                    }
                }
                CommonHelper.OrderByRule(coupons, double.Parse(output.shouldpay));
                var cp = coupons.FirstOrDefault();
                if (cp != null)
                {
                    output.Coupon = new CouponDto();
                    output.Coupon.Type = cp.Type;
                    output.Coupon.Value = cp.Value;
                    output.Coupon.CouponUserid = cp.CouponUserid;
                    output.Coupon.Couponpkgname = cp.Couponpkgname;
                    if (cp.Type == "1")
                    {
                        //output.allpay =
                        //    (((decimal)double.Parse(output.shouldpay) - decimal.Parse(cp.Value)) < 0
                        //        ? 0
                        //        : ((decimal)double.Parse(output.shouldpay) - decimal.Parse(cp.Value))).ToString("F");
                        output.allpay = (Math.Floor(((decimal) double.Parse(output.shouldpay) - decimal.Parse(cp.Value))*
                                                    100) < 0
                            ? 0
                            : Math.Floor((decimal.Parse(output.shouldpay) - decimal.Parse(cp.Value))*100)/100).ToString(
                                "F");

                        output.discountamount =
                            ((decimal) double.Parse(output.shouldpay) - (decimal) double.Parse(output.allpay)).ToString(
                                "F");
                        //track.discount_amount =
                        //    (double) ((decimal) double.Parse(output.shouldpay) - (decimal) double.Parse(output.allpay));
                    }
                    else if (cp.Type == "2")
                    {
                        //output.allpay = ((decimal)double.Parse(output.shouldpay) * decimal.Parse(cp.Value)).ToString("F");
                        output.allpay =
                            (Math.Floor((decimal) double.Parse(output.shouldpay)*decimal.Parse(cp.Value)*100)/100)
                                .ToString("F");
                        output.discountamount =
                            ((decimal) double.Parse(output.shouldpay) - (decimal) double.Parse(output.allpay)).ToString(
                                "F");
                        //track.discount_amount =
                        //    (double) ((decimal) double.Parse(output.shouldpay) - (decimal) double.Parse(output.allpay));
                    }
                }
            }

            await _trackRepository.UpdateAsync(track);
            return output;
        }
        /// <summary>
        /// 刷新租车信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RentalBikeOutput> RefreshBiketemp(RentalBikeInput input)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                "select a.id,a.created_at,a.updated_at,a.user_id,a.bike_id,e.ble_name,a.start_point,a.end_point,a.start_site_id,a.end_site_id" +
                ",a.start_time,a.end_time,a.payment,a.should_pay,a.pay_status,a.pay_method,a.pay_docno,a.remark" +
                ",b.`name` as start_site_name,b.gps_point as start_gps_point,d.`name` as end_site_name,a.start_point,b.school_id,c.`name` as school_name" +
                ",c.time_charge,e.Lock_pwd, c.free_time, e.ble_type, e.ble_serial");
            sb.Append(" from track as a left join bikesite as b on a.start_site_id = b.id");
            sb.Append(" left join school as c on b.school_id = c.id");
            sb.Append(" left join bikesite as d on a.end_site_id = d.id");
            sb.Append(" left join bike as e on a.bike_id = e.id");
            if (input.out_trade_no.IsNullOrEmpty())
            {
                sb.AppendFormat(" where a.user_id={0} and a.pay_status < 2", input.user_id);
            }
            else
            {
                sb.AppendFormat(" where a.pay_docno='{0}'", input.out_trade_no);
            }

            var track = _sqlReadExecuter.SqlQuery<TrackEntity>(sb.ToString()).ToList().FirstOrDefault();
            if (track == null)
            {
                throw new UserFriendlyException("没有行程单!");
            }
            var gpsinput = input.gps_point.Split(',');
            double ip_lon = 0;
            double ip_lat = 0;
            if (gpsinput.Length == 2)
            {
                ip_lon = double.Parse(gpsinput[0], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
                ip_lat = double.Parse(gpsinput[1], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
            }
            else
            {
                Logger.Error("RefreshBiketemp-794");
            }
            var bikesitelist = await _bikesiteReadRepository.GetAllListAsync(t => t.School_id == track.School_id && t.Enable && t.Type == 3);

            Entities.Bikesite bsite = null;
            double mindistance = 99999999;
            foreach (var bikesite in bikesitelist)
            {
                var bikesitegps = bikesite.Gps_point.Split(',');
                double bs_lon = 0;
                double bs_lat = 0;
                if (bikesitegps.Length == 2)
                {
                    bs_lon = double.Parse(bikesitegps[0], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
                    bs_lat = double.Parse(bikesitegps[1], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
                }
                else
                {
                    Logger.Error("RefreshBiketemp-803-"+ bikesite.Id);
                }

                var distance = LatlonHelper.GetDistance(ip_lat, ip_lon, bs_lat, bs_lon) * 1000;//KM->M
                if (distance < mindistance && distance <= bikesite.Radius)//
                {
                    mindistance = distance;
                    bsite = bikesite;
                }
            }

            var output = new RentalBikeOutput();
            if (bsite != null)
            {
                var gps = bsite.Gps_point;
                var gpss = gps.Split(',');
                if (gpss.Length == 2)
                {
                    output.lon_end = double.Parse(gpss[0], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
                    output.lat_end = double.Parse(gpss[1], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
                }
                else
                {
                    Logger.Error("RefreshBiketemp-825");
                }
            }
            var gpsstart = track.Start_gps_point.Split(',');
            if (gpsstart.Length == 2)
            {
                output.lon = double.Parse(gpsstart[0], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
                output.lat = double.Parse(gpsstart[1], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
            }
            else
            {
                Logger.Error("RefreshBiketemp-837");
            }

            output.out_trade_no = track.Pay_docno;
            output.ble_type = track.Ble_type;
            output.ble_serial = track.Ble_serial;
            output.start_site_name = track.Start_site_name;
            output.ble_name = track.Ble_name;
            output.start_time = track.Start_time.ToString();
            output.end_site_name = bsite == null ? "" : bsite.Name;
            output.pwd = track.Lock_pwd;
            output.BikesiteList = bikesitelist.MapTo<List<BikesiteEntity>>();
            return output;
        }
        /// <summary>
        /// 订单结束计算金额
        /// </summary>
        /// <param name="input"></param>
        public async Task<RentalInfoOutput> RentalFinishInfo([FromUri] RentalBikeInput input)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                "select a.id,a.created_at,a.updated_at,a.user_id,a.bike_id,e.ble_name,a.start_point,a.end_point,a.start_site_id,a.end_site_id,a.coupon_id,a.discount_amount" +
                ",a.start_time,a.end_time,a.payment,a.should_pay,a.pay_status,a.pay_method,a.pay_docno,a.remark,a.pay_time" +
                ",b.`name` as start_site_name,d.`name` as end_site_name,a.start_point, c.`name` as school_name" +
                ",c.time_charge,e.Lock_pwd, c.free_time, c.fixed_amount, c.top_amount, f.recharge_count");
            sb.Append(" from track as a left join bikesite as b on a.start_site_id = b.id");
            sb.Append(" left join school as c on b.school_id = c.id");
            sb.Append(" left join bikesite as d on a.end_site_id = d.id");
            sb.Append(" left join bike as e on a.bike_id = e.id");
            sb.Append(" left join recharge as f on a.user_id = f.user_id");
            sb.AppendFormat(" where a.pay_docno='{0}'", input.out_trade_no);
            var track = _sqlReadExecuter.SqlQuery<TrackEntity>(sb.ToString()).FirstOrDefault();
            
            if (track == null)
            {
                throw new UserFriendlyException("没有行程单!");
            }
            var output = new RentalInfoOutput();
            output.out_trade_no = track.Pay_docno;
            output.start_site_name = track.Start_site_name;
            output.ble_name = track.Ble_name;
            output.start_time = track.Start_time.ToString();
            output.end_site_name = track.End_site_name;
            output.end_time = track.End_time.ToString();
            output.school_name = track.School_name;
            output.remark = track.Remark;
            output.pay_method = track.Pay_method;
            output.recharge_count = track.Recharge_count?.ToString("F") ?? "0.00";
            output.pay_time = track.Pay_time.ToString();
            
            TimeSpan costtime = DateTime.Parse(track.End_time.ToString()) - DateTime.Parse(track.Start_time.ToString());
            TimeSpan costday = DateTime.Parse(track.End_time.ToString()).Date - DateTime.Parse(track.Start_time.ToString()).Date;

            //var ctm = (int)Math.Ceiling(costtime.TotalMinutes);//去掉多余的零头
            //var day = (int)Math.Ceiling(costday.TotalDays);
            var ctm = (int)Math.Floor(costtime.TotalMinutes);//去掉多余的零头
            var day = (int)Math.Ceiling(costday.TotalDays);
            if (costtime.TotalMinutes < 1)
            {
                output.rental_time = Math.Ceiling(costtime.TotalSeconds) / 100;
            }
            else
            {
                output.rental_time = ctm;
            }

            if (track.Payment != null)
            {
                output.allpay = ((double) track.Payment).ToString("F");
                output.shouldpay = (track.Should_pay??0).ToString("F");
            }
            else if (ctm < track.Free_time)
            {
                output.allpay = track.Fixed_amount.ToString("F");
                output.shouldpay = track.Fixed_amount.ToString("F");
            }
            //else if (ctm > track.Free_time && day == 0)
            //{
            //    var tpay = (ctm - track.Free_time) * track.time_charge / 100.00 + track.Fixed_amount;
            //    output.allpay = tpay <= track.Top_amount ? tpay.ToString() : track.Top_amount.ToString();
            //}
            else
            {
                //TimeSpan fcosttime = DateTime.Parse(track.Start_time.ToString()).Date.AddDays(1) - DateTime.Parse(track.Start_time.ToString());
                //TimeSpan ecosttime = DateTime.Parse(track.End_time.ToString()) - DateTime.Parse(track.End_time.ToString()).Date;

                //var ftpay = ((int)Math.Ceiling(fcosttime.TotalMinutes) - track.Free_time) * track.time_charge / 100.00 + track.Fixed_amount;//分转元
                //var etpay = ((int)Math.Ceiling(ecosttime.TotalMinutes) - track.Free_time) * track.time_charge / 100.00 + track.Fixed_amount;//分转元
                //var firstpay = ftpay <= track.Top_amount ? ftpay : track.Top_amount;
                //var endpay = (int)Math.Ceiling(ecosttime.TotalMinutes) > track.Free_time ? track.Top_amount : etpay;

                //output.allpay = track.Payment == null ? (firstpay + track.Top_amount * day + endpay).ToString() : track.Payment.ToString();//分转元
                output.allpay = (((ctm - track.Free_time) * track.time_charge / 100.00) + track.Fixed_amount).ToString("F");//分转元
                output.shouldpay = (((ctm - track.Free_time) * track.time_charge / 100.00) + track.Fixed_amount).ToString("F");//分转元
            }

            if (track.coupon_id != null)
            {
                //获取用户没有使用过的优惠券信息
                var couponuserass = await _couponUserAssReadRepository.FirstOrDefaultAsync(t => t.Id == track.coupon_id);

                if (couponuserass != null)
                {
                    var cp = new CouponDto
                    {
                        CouponUserid = couponuserass.Id.ToString(),
                        Type = couponuserass.Coupon.coupon_type.ToString(),
                        Value = couponuserass.Coupon.coupon_value.ToString("F"),
                        UsedTime = couponuserass.coupon_use_time.ToString(),
                        Display = couponuserass.Coupon.coupon_rule,
                        Couponpkgname = couponuserass.CouponPackage.coupon_pkg_name
                    };
                    output.Coupon = new CouponDto();
                    output.Coupon.Type = cp.Type;
                    output.Coupon.Value = cp.Value;
                    output.Coupon.CouponUserid = cp.CouponUserid;
                    output.Coupon.Couponpkgname = cp.Couponpkgname;
                    if (cp.Type == "1")
                    {
                        output.allpay =
                            (Math.Floor((decimal.Parse(output.shouldpay) - decimal.Parse(cp.Value)) *100) < 0
                                ? 0
                                : Math.Floor((decimal.Parse(output.shouldpay) - decimal.Parse(cp.Value)) * 100) / 100).ToString("F");
                        output.discountamount =
                            ((decimal) double.Parse(output.shouldpay) - (decimal) double.Parse(output.allpay)).ToString(
                                "F");
                    }
                    else if (cp.Type == "2")
                    {
                        //output.allpay = ((decimal) double.Parse(output.shouldpay)*(decimal) cp.Value).ToString("F");
                        output.allpay = (Math.Floor((decimal)double.Parse(output.shouldpay) * decimal.Parse(cp.Value) * 100) / 100).ToString("F");
                        output.discountamount =
                            ((decimal) double.Parse(output.shouldpay) - (decimal) double.Parse(output.allpay)).ToString(
                                "F");
                    }

                }
            }
            else if(track.coupon_id == null && track.Pay_status < 3 && track.Should_pay > 0)//还没结算，计算最优优惠券
            {
                var coupons = new List<CouponDto>();
                //获取用户没有使用过的优惠券信息
                var couponlist = await _couponUserAssReadRepository.GetAllListAsync(t => t.user_id == input.user_id && t.coupon_use_time == null);
                //用户的优惠券包集合
                var couponpkgass_couponpkgids = couponlist.Select(t => t.coupon_pkg_id ?? 0).ToList();
                //优惠券包 下面的优惠券信息
                var couponpkgasslist =
                    await
                        _couponPkgAssReadRepository.GetAllListAsync(
                            t => couponpkgass_couponpkgids.Contains(t.coupon_pkg_id ?? 0));
                foreach (var item in couponlist)
                {
                    var temp =
                        couponpkgasslist.FirstOrDefault(
                            t => t.coupon_pkg_id == item.coupon_pkg_id && t.coupon_id == item.coupon_id);
                    if (temp != null && temp.coupon_pkg_disable_time.Value.AddDays(1) >= DateTime.Now)
                    {
                        coupons.Add(new CouponDto
                        {
                            CouponUserid = item.Id.ToString(),
                            Type = item.Coupon.coupon_type.ToString(),
                            Value = item.Coupon.coupon_value.ToString("F"),
                            UsedTime = item.coupon_use_time.ToString(),
                            Display = item.Coupon.coupon_rule,
                            Couponpkgname = item.CouponPackage.coupon_pkg_name
                        });
                    }
                }
                CommonHelper.OrderByRule(coupons, double.Parse(output.shouldpay));
                var cp = coupons.FirstOrDefault();
                if (cp != null)
                {
                    output.Coupon = new CouponDto();
                    output.Coupon.Type = cp.Type;
                    output.Coupon.Value = cp.Value;
                    output.Coupon.CouponUserid = cp.CouponUserid;
                    output.Coupon.Couponpkgname = cp.Couponpkgname;
                    if (cp.Type == "1")
                    {
                        //output.allpay =
                        //    (((decimal)double.Parse(output.shouldpay) - decimal.Parse(cp.Value)) < 0
                        //        ? 0
                        //        : ((decimal)double.Parse(output.shouldpay) - decimal.Parse(cp.Value))).ToString("F");
                        output.allpay = (Math.Floor((decimal.Parse(output.shouldpay) - decimal.Parse(cp.Value)) * 100) < 0
                                ? 0
                                : Math.Floor((decimal.Parse(output.shouldpay) - decimal.Parse(cp.Value)) * 100) / 100).ToString("F");
                        output.discountamount =
                            (decimal.Parse(output.shouldpay) - decimal.Parse(output.allpay)).ToString("F");
                    }
                    else if (cp.Type == "2")
                    {
                        //output.allpay = ((decimal)double.Parse(output.shouldpay) * decimal.Parse(cp.Value)).ToString("F");
                        output.allpay = (Math.Floor(decimal.Parse(output.shouldpay) * decimal.Parse(cp.Value) * 100) / 100).ToString("F");
                        output.discountamount =
                            (decimal.Parse(output.shouldpay) - decimal.Parse(output.allpay)).ToString("F");
                    }
                }
            }
            return output;
        }
        /// <summary>
        /// 订单支付状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<TrackInfoOutput> RentalTrackInfo([FromUri] RentalBikeInput input)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select a.id,a.created_at,a.updated_at,a.user_id,a.bike_id,a.start_point,a.end_point,a.start_site_id,a.end_site_id,a.start_time,a.end_time,a.payment,a.pay_status,a.pay_method,a.pay_docno,a.remark,a.start_point,b.ble_type");
            sb.Append(" from track as a");
            sb.Append(" left join bike as b on a.bike_id = b.id");
            sb.AppendFormat(" where a.user_id={0} and a.pay_status < 3", input.user_id);
            sb.Append(" order by a.updated_at desc LIMIT 1");

            var track = _sqlReadExecuter.SqlQuery<TrackEntity>(sb.ToString()).FirstOrDefault();
            if (track == null)
            {
                return new TrackInfoOutput {out_trade_no = "", pay_status = 0, ble_type = 0};
            }
            var result = new TrackInfoOutput {out_trade_no = track.Pay_docno, pay_status = track.Pay_status, ble_type = track.Ble_type};
            return result;
        }
        /// <summary>
        /// 能否租用车辆
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<CanRentalOutput> CanReantalBike([FromUri]RentalBikeInput input)
        {
            var bike =
                await
                    _bikeReadRepository.FirstOrDefaultAsync(
                        t => t.Ble_name == input.Ble_name && t.rent_type == 1);
            if (bike == null || bike.Ble_type < 3)
            {
                throw new UserFriendlyException("车辆编号错误或该车不可租");
            }
            if (bike.Bike_status == 0 && bike.Ble_type != 3)//蓝牙锁除外
            {
                throw new UserFriendlyException("请先把租赁的自行车归还，再进行租车!");
            }
            
            var school = bike.School;
            decimal timecharge = school.Time_charge == null ? 0 :(decimal) school.Time_charge;
            var msg = $"{school.Free_time}分钟内{school.Fixed_amount}元，超出部分每分钟{timecharge/100}元";
            return new CanRentalOutput {charge = msg , type = bike.Ble_type.Value, serial = bike.Ble_serial};
        }
        /// <summary>
        /// 余额结算
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CostByRecharge(RentalRechargeInput input)
        {
            var track = await _trackRepository.FirstOrDefaultAsync(t => t.Pay_docno == input.out_trade_no);
            if (track != null)
            {
                track.Pay_status = 3;
                track.Trade_no = input.out_trade_no;
                track.Pay_method = "余额";
                track.Updated_at = DateTime.Now;
                track.Payment = track.Should_pay;
                track.Pay_time = DateTime.Now;
                //LogHelper.Logger.Info(Request.Form["total_fee"]);

                if (input.coupon_id != null)
                {
                    var couponuserass =
                        await _couponUserAssWriteRepository.FirstOrDefaultAsync(t => t.Id == input.coupon_id);
                    if (couponuserass != null)
                    {
                        couponuserass.coupon_use_time = DateTime.Now;
                        couponuserass.update_time =DateTime.Now;
                        await _couponUserAssWriteRepository.UpdateAsync(couponuserass);

                        track.coupon_id = input.coupon_id;
                        track.discount_amount = input.disamount;
                        track.Payment = (double) ((decimal) (track.Should_pay ?? 0) - (decimal) (input.disamount ?? 0));
                    }
                }

                await _trackRepository.UpdateAsync(track);

                var recharge = await _rechargeWriteRepository.FirstOrDefaultAsync(t => t.User_id == track.User_id);
                if (recharge != null && recharge.Recharge_count != null)
                {
                    if (track.Payment > recharge.Recharge_count)
                    {
                        throw new UserFriendlyException("预充值额不足,请先进行充值,或用别的方式付款");
                    }
                    recharge.Recharge_count =
                        (double) ((decimal) recharge.Recharge_count - (decimal) (track.Payment ?? 0));
                    recharge.Updated_at = DateTime.Now;

                    await _rechargeWriteRepository.UpdateAsync(recharge);
                }
                else
                {
                    throw new UserFriendlyException("没有预充值");
                }
            }
            else
            {
                throw new UserFriendlyException("订单号错误");
            }
        }
        /// <summary>
        /// 蓝牙锁开锁之后创建订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RentalBikeOutput> RentalBikeCreateTrack(RentalBikeInput input)
        {
            var bike = await
                    _bikeRepository.FirstOrDefaultAsync(
                        t => t.Ble_name == input.Ble_name && t.rent_type == 1);

            if (bike == null || bike.Ble_type < 3)
            {
                throw new UserFriendlyException("没有该车辆或该车不可租!");
            }
            if (bike.Bike_status == 0)
            {
                throw new UserFriendlyException("被租赁中!");
            }

            var gpsinput = input.gps_point.Trim().Split(',');

            double ip_lon = 0;

            double ip_lat = 0;

            if (gpsinput.Length == 2)
            {
                ip_lon = double.Parse(gpsinput[0].Trim(), NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
                //LogHelper.Logger.Info(ip_lon.ToString);
                ip_lat = double.Parse(gpsinput[1].Trim(), NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
            }
            var bikesitelist = await _bikesiteRepository.GetAllListAsync(t => t.School_id == bike.School_id && t.Enable && t.Type == 3);

            Entities.Bikesite bsite = null;
            //Dictionary<string,string> temp = new Dictionary<string, string>();
            double mindistance = 99999999;
            //LogHelper.Logger.Info(mindistance.ToString);
            foreach (var bikesite in bikesitelist)
            {
                var bikesitegps = bikesite.Gps_point.Split(',');
                double bs_lon = 0;
                double bs_lat = 0;
                if (bikesitegps.Length == 2)
                {
                    bs_lon = double.Parse(bikesitegps[0].Trim(), NumberStyles.Any,
                        CultureInfo.CreateSpecificCulture("zh-cn"));
                    bs_lat = double.Parse(bikesitegps[1].Trim(), NumberStyles.Any,
                        CultureInfo.CreateSpecificCulture("zh-cn"));
                }
                else
                {
                    Logger.Debug("RentalBiketemp-490-" + bikesite.Id);
                }

                var distance = LatlonHelper.GetDistance(ip_lat, ip_lon, bs_lat, bs_lon) * 1000;//KM->M

                //LogHelper.Logger.Info(distance.ToString);

                if (distance < mindistance && distance <= bikesite.Radius)//
                {
                    mindistance = distance;
                    bsite = bikesite;
                }
            }

            if (bsite == null)
            {
                throw new UserFriendlyException("范围内没有桩点");
            }
            var paydocno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
            var startdate = DateTime.Now;

            //pay_status 1  租车未还车
            var sqlstr =
                "insert into track(bike_id,user_id,created_at,updated_at,start_site_id,start_point,start_time,pay_docno,pay_status) values(" + bike.Id + "," + input.user_id + ",'" + startdate.ToString("yyyy/MM/dd HH:mm:ss") + "','" + startdate.ToString("yyyy/MM/dd HH:mm:ss") + "'," + bsite.Id + ",'" + input.gps_point + "','" + startdate.ToString("yyyy/MM/dd HH:mm:ss") + "','" + paydocno + "',1)";
            await _sqlExecuter.ExecuteAsync(sqlstr);



            var gps = bsite.Gps_point;
            var gpss = gps.Split(',');
            double gpss1 = 0;
            double gpss2 = 0;
            if (gpss.Length == 2)
            {
                gpss1 = double.Parse(gpss[0], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
                gpss2 = double.Parse(gpss[1], NumberStyles.Any, CultureInfo.CreateSpecificCulture("zh-cn"));
            }
            var output = new RentalBikeOutput
            {
                out_trade_no = paydocno,
                lon = gpss1,
                lat = gpss2,
                start_site_name = bsite.Name,
                ble_name = input.Ble_name,
                ble_serial = bike.Ble_serial,
                start_time = startdate.ToString("yyyy/MM/dd HH:mm:ss"),
                BikesiteList = bikesitelist.MapTo<List<BikesiteEntity>>()
            };
            bike.Bike_status = 0; //出租中

            await _bikeRepository.UpdateAsync(bike);
            bsite.Available_count = bsite.Available_count - 1;
            await _bikesiteRepository.UpdateAsync(bsite);
            return output;
        }
        /// <summary>
        /// 根据车辆编号获取车辆总共租用时间，租用次数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BikeUsedOutput> GetBikeUsedInfo(RentalBikeInput input)
        {
            var track = await _trackReadRepository.GetAllListAsync(t => t.Bike.Ble_name == input.Ble_name);
            var output = new BikeUsedOutput();
            output.Count = track.Count;
            TimeSpan allTimeSpan = new TimeSpan();
            foreach (var item in track)
            {
                if (item.End_time != null)
                {
                    TimeSpan costTimeSpan = DateTime.Parse(item.End_time.ToString()) -
                                            DateTime.Parse(item.Start_time.ToString());
                    allTimeSpan = allTimeSpan + costTimeSpan;
                }
            }

            if (allTimeSpan.TotalDays > 1)
            {
                output.Usedtime = allTimeSpan.Days + "天" + allTimeSpan.Hours + "小时" + allTimeSpan.Minutes + "分钟";
            }
            else if (allTimeSpan.TotalHours > 1)
            {
                output.Usedtime = allTimeSpan.Hours + "小时" + allTimeSpan.Minutes + "分钟";
            }
            else if (allTimeSpan.TotalMinutes > 1)
            {
                output.Usedtime = allTimeSpan.Minutes + "分钟";
            }
            else
            {
                output.Usedtime = "无";
            }
            return output;
            throw new NotImplementedException();
        }
        /// <summary>
        /// 结束租车（免费）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RentalFinishOutput> RentalBikeFinishtempo(RentalBikeInput input)
        {
            var track = await _trackRepository.FirstOrDefaultAsync(
                    t => t.Pay_docno == input.out_trade_no && t.User_id == input.user_id);
            if (track == null)
            {
                throw new UserFriendlyException("无该订单号!");
            }
            
            track.Payment = 0;
            track.Pay_method = "免费";
            track.Pay_time = DateTime.Now;
            track.Pay_status = 3;//免费，直接结束

            if (input.coupon_id != null && input.coupon_id > 0)
            {
                var couponuserass =
                    await _couponUserAssWriteRepository.FirstOrDefaultAsync(t => t.Id == input.coupon_id);
                if (couponuserass != null)
                {
                    couponuserass.coupon_use_time = DateTime.Now;
                    couponuserass.update_time = DateTime.Now;
                    await _couponUserAssWriteRepository.UpdateAsync(couponuserass);

                    track.coupon_id = input.coupon_id;
                    track.discount_amount = input.disamount;
                }
            }

            await _trackRepository.UpdateAsync(track);

            //bike.Bike_status = 1;
            //await _bikeRepository.UpdateAsync(bike);

            return new RentalFinishOutput { end_time = track.End_time.ToString(), out_trade_no = track.Pay_docno };
        }
    }
}