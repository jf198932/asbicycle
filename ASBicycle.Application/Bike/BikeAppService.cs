using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Abp.AutoMapper;
using Abp.UI;
using ASBicycle.Bike.Dto;
using System.Linq;
using System.Text;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using ASBicycle.Common;

namespace ASBicycle.Bike
{
    public class BikeAppService : ASBicycleAppServiceBase, IBikeAppService
    {
        private readonly IBikeRepository _bikeRepository;
        private readonly IRepository<Entities.Track> _trackRepository;
        private readonly ISqlExecuter _sqlExecuter;
        private readonly IRepository<Entities.Bikesite> _bikesiteRepository;
        private readonly IRepository<Entities.User> _useRepository; 

        public BikeAppService(IBikeRepository bikeRepository, IRepository<Entities.Track> trackRepository, ISqlExecuter sqlExecuter, IRepository<Entities.Bikesite> bikesiteRepository, IRepository<Entities.User> useRepository)
        {
            _bikeRepository = bikeRepository;
            _trackRepository = trackRepository;
            _sqlExecuter = sqlExecuter;
            _bikesiteRepository = bikesiteRepository;
            _useRepository = useRepository;
        }
        [HttpGet]
        public async Task<AlarmBikeOutput> GetAlarmBikeWay([FromUri] string phone)
        {
            var bike = await _bikeRepository.FirstOrDefaultAsync(t => t.User.Phone == phone);
            var model = new AlarmBikeOutput();
            if (bike == null)
            {
                throw new UserFriendlyException("请先进行绑定");
            }
            model.bikename = bike.Ble_name;
            model.bikeimg = bike.Bike_img;

            var sqlstr =
                "select b.gps_point,b.`name` as sitename,DATE_FORMAT(op_time,'%Y-%m-%d %H:%i:%S') as alarmtime,bikesite_id,bike_id from log as l JOIN bikesite as b on l.bikesite_id = b.id WHERE op_time >= (select op_time from log where type = 3 and bike_id = " + bike.Id+" ORDER BY op_time DESC LIMIT 1) and l.bike_id="+bike.Id+" order by op_time desc";
            model.alarmlist = _sqlExecuter.SqlQuery<AlarmBikeDto>(sqlstr).ToList();
            foreach (var item in model.alarmlist)
            {
                if (!item.gps_point.IsNullOrEmpty())
                {
                    var tempgps = item.gps_point.Split(',');
                    item.lon = double.Parse(tempgps[0]);
                    item.lat = double.Parse(tempgps[1]);
                }
            }
            return model;
        }

        public async Task<BikeOutput> GetBikeInfo([FromUri]string serial)
        {
            var model = await _bikeRepository.FirstOrDefaultAsync(b => b.Ble_name == serial);
            if(model == null)
                throw new UserFriendlyException("没有该车辆");
            var result = model.MapTo<BikeOutput>();
            //result.School_name = model.User.School == null ? "":model.User.School.Name;
            result.Bikesite_name = model.Bikesite == null ? "":model.Bikesite.Name;

            return result;
        }
        [HttpPost]
        public async Task UpdateBike(BikeInput bikeInput)
        {
            var bike = await _bikeRepository.FirstOrDefaultAsync(b => b.Ble_name == bikeInput.Serial);
            if(bikeInput.Vlock_status != null)
                bike.Vlock_status = bikeInput.Vlock_status;
            if(bikeInput.Bike_status != null)
                bike.Bike_status = bikeInput.Bike_status;
            bike.Bike_img = bikeInput.Bike_img;
            bike.Battery = bikeInput.Battery;
            if(bikeInput.Bikesite_id != null)
                bike.Bikesite_id = bikeInput.Bikesite_id;
            bike.Position = bikeInput.Position;
            await _bikeRepository.UpdateAsync(bike);
        }
        [HttpPost]
        public async Task<BikeUploadOutput> UploadBikePic()
        {
            //foreach (string f in HttpContext.Current.Request.Files.AllKeys)
            //{
            //var bikeid = HttpContext.Current.Request.Params["bikeid"];

            //if (bikeid.IsNullOrEmpty())
            //    throw new UserFriendlyException("没有接收到bikeid");
            HttpPostedFile file = HttpContext.Current.Request.Files["filedata"];
            if (file != null)
            {
                //var bikemodel = await _bikeRepository.FirstOrDefaultAsync(t => t.Ble_name == bikeid);

                //if (null == bikemodel)
                //    throw new UserFriendlyException("请输入正确的追踪器编号");

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
                    //bikemodel.Bike_img = img;
                    //await _bikeRepository.UpdateAsync(bikemodel);
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
            //}
        }

        [HttpPost]
        public async Task<RentalBikeOutput> RentalBike(RentalBikeInput input)
        {
            
            var bike = await _bikeRepository.FirstOrDefaultAsync(t => t.Ble_name == input.Ble_name);
            if (bike == null)
            {
                throw new UserFriendlyException("车辆编号错误");
            }

            if (bike.Bikesite_id == null)
            {
                throw new UserFriendlyException("该车不能出租!");
            }

            //StringBuilder sb = new StringBuilder();
            //sb.Append(
            //    "select a.id,a.created_at,a.updated_at,a.user_id,a.bike_id,a.start_point,a.end_point,a.start_site_id,a.end_site_id,a.start_time,a.end_time,a.payment,a.pay_status,a.pay_method,a.pay_docno,a.remark,b.`name` as start_site_name,a.start_point");
            //sb.Append(" from track as a left join bikesite as b on a.start_site_id = b.id");
            //sb.AppendFormat(" where user_id={0} and bike_id={1} and end_site_id is NULL", input.user_id, bike.Id);
            //var track = _sqlExecuter.SqlQuery<TrackEntity>(sb.ToString()).ToList().FirstOrDefault();

            if (bike.Bike_status == 0)
            {
                throw new UserFriendlyException("请先把租赁的自行车归还，再进行租车!");
            }


            var paydocno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
            var startdate = DateTime.Now;
            var sqlstr =
                "insert into track(bike_id,user_id,created_at,updated_at,start_site_id,start_point,start_time,pay_docno) values(" + bike.Id + "," + input.user_id + ",'" + startdate + "','" + startdate + "'," + bike.Bikesite_id + ",'" + bike.Bikesite.Gps_point + "','" + startdate + "','" + paydocno + "')";
            await _sqlExecuter.ExecuteAsync(sqlstr);


            //await _trackRepository.InsertAsync(new Entities.Track
            //{
            //    Bike_id = bike.Id,
            //    User_id = input.user_id,
            //    Created_at = DateTime.Now,
            //    Updated_at = DateTime.Now,
            //    Start_site_id = bike.Bikesite_id,
            //    Start_point = input.gps_point,
            //    Start_time = DateTime.Now,
            //    Pay_docno = paydocno
            //});

            var gps = bike.Bikesite.Gps_point;
            var gpss = gps.Split(',');
            var output = new RentalBikeOutput
            {
                out_trade_no = paydocno,
                lon = double.Parse(gpss[0]),
                lat = double.Parse(gpss[1]),
                start_site_name = bike.Bikesite.Name,
                ble_name = input.Ble_name,
                start_time = startdate.ToString("yyyy/MM/dd HH:mm:ss"),
                pwd = bike.Lock_pwd
            };
            bike.Bike_status = 0; //出租中
            await _bikeRepository.UpdateAsync(bike);


            return output;
            
        }

        [HttpPost]
        public async Task<RentalFinishOutput> RentalBikeFinish(RentalBikeInput input)
        {
            var track = await _trackRepository.FirstOrDefaultAsync(
                    t => t.Pay_docno == input.out_trade_no && t.User_id == input.user_id);
            if (track == null)
            {
                throw new UserFriendlyException("无该订单号!");
            }
            var bike = await _bikeRepository.FirstOrDefaultAsync(t => t.Ble_name == input.Ble_name);
            if (bike.Bikesite_id == null)
            {
                throw new UserFriendlyException("请到桩点还车!");
            }
            var endtime = DateTime.Now;

            track.End_point = bike.Bikesite.Gps_point;
            track.End_site_id = bike.Bikesite.Id;
            track.End_time = endtime;
            track.Updated_at = endtime;
            track.Pay_status = 10;

            await _trackRepository.UpdateAsync(track);

            bike.Bike_status = 1;
            await _bikeRepository.UpdateAsync(bike);

            return new RentalFinishOutput {end_time = endtime.ToString("yyyy/MM/dd HH:mm:ss"), out_trade_no = track.Pay_docno};
        }
        [HttpPost]
        public async Task<RentalBikeOutput> RefreshBike(RentalBikeInput input)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select a.id,a.created_at,a.updated_at,a.user_id,a.bike_id,a.start_point,a.end_point,a.start_site_id,a.end_site_id,a.start_time,a.end_time,a.payment,a.pay_status,a.pay_method,a.pay_docno,a.remark,b.`name` as start_site_name,a.start_point, c.ble_name");
            sb.Append(" from track as a left join bikesite as b on a.start_site_id = b.id left join bike as c on a.bike_id = c.id");
            if (input.out_trade_no.IsNullOrEmpty())
            {
                sb.AppendFormat(" where a.user_id={0} and a.pay_status < 3", input.user_id);
            }
            else
            {
                sb.AppendFormat(" where a.out_trade_no={0}", input.out_trade_no);
            }
            var track = _sqlExecuter.SqlQuery<TrackEntity>(sb.ToString()).ToList().FirstOrDefault();
            if (track == null)
            {
                throw new UserFriendlyException("没有行程单!");
            }

            var bike = await _bikeRepository.FirstOrDefaultAsync(t => t.Ble_name == track.Ble_name);
            if (bike == null)
            {
                throw new UserFriendlyException("车辆编号错误");
            }

            var output = new RentalBikeOutput();
            if (bike.Bikesite_id != null)
            {
                var gps = bike.Bikesite.Gps_point;
                var gpss = gps.Split(',');
                output.lon_end = double.Parse(gpss[0]);
                output.lat_end = double.Parse(gpss[1]);
            }
            var gpsstart = track.Start_point.Split(',');
            output.lon = double.Parse(gpsstart[0]);
            output.lat = double.Parse(gpsstart[1]);

            output.out_trade_no = track.Pay_docno;
            output.start_site_name = track.Start_site_name;
            output.ble_name = track.Ble_name;
            output.start_time = track.Start_time.ToString();
            output.end_site_name = bike.Bikesite == null ? "" : bike.Bikesite.Name;

            return output;
        }

        [HttpPost]
        public async Task<RentalCostOutput> RentalCast(RentalBikeInput input)
        {
            var bike = await _bikeRepository.FirstOrDefaultAsync(t => t.Ble_name == input.Ble_name);
            if (bike == null)
            {
                throw new UserFriendlyException("车辆编号错误");
            }
            
            StringBuilder sb = new StringBuilder();
            sb.Append(
                "select a.id,a.created_at,a.updated_at,a.user_id,a.bike_id,a.start_point,a.end_point,a.start_site_id,a.end_site_id,a.start_time,a.end_time,a.payment,a.pay_status,a.pay_method,a.should_pay,a.pay_docno,a.remark,b.`name` as start_site_name,d.`name` as end_site_name,a.start_point, c.`name` as school_name,c.time_charge");
            sb.Append(" from track as a left join bikesite as b on a.start_site_id = b.id");
            sb.Append(" left join school as c on b.school_id = c.id");
            sb.Append(" left join bikesite as d on a.end_site_id = d.id");
            sb.AppendFormat(" where a.user_id={0} and a.bike_id = {1} and a.pay_status < 3", input.user_id, bike.Id);
            var track = _sqlExecuter.SqlQuery<TrackEntity>(sb.ToString()).ToList().FirstOrDefault();
            if (track == null)
            {
                throw new UserFriendlyException("没有行程单!");
            }
            var output = new RentalCostOutput();

            var end_time = DateTime.Now;

            output.out_trade_no = track.Pay_docno;
            output.start_site_name = track.Start_site_name;
            output.ble_name = track.Ble_name;
            output.start_time = track.Start_time.ToString();
            output.end_site_name = track.End_site_name;
            output.end_time = end_time.ToString("yyyy/MM/dd HH:mm:ss");
            output.school_name = track.School_name;


            TimeSpan costtime = (track.End_time == null ? end_time : DateTime.Parse(track.End_time.ToString())) -
                                DateTime.Parse(track.Start_time.ToString());

            var ctm = (int)costtime.TotalMinutes;//去掉多余的零头
            output.rental_time = ctm;
            output.allpay = (ctm * track.time_charge / 100.00).ToString();//分转元

            return output;
        }

        public async Task<RentalBikeOutput> RentalBiketemp(RentalBikeInput input)
        {
            var bike =
                await
                    _bikeRepository.FirstOrDefaultAsync(
                        t => t.Ble_name == input.Ble_name && t.rent_type == 1 && t.Bike_status == 1 && t.Ble_type == 4);
            if (bike == null)
            {
                throw new UserFriendlyException("车辆编号错误或该车不可租");
            }

            if (bike.Bike_status == 0)
            {
                throw new UserFriendlyException("请先把租赁的自行车归还，再进行租车!");
            }
            var gpsinput = input.gps_point.Split(',');
            var ip_lon = double.Parse(gpsinput[0]);
            var ip_lat = double.Parse(gpsinput[1]);

            var bikesitelist = _bikesiteRepository.GetAll().Where(t => t.School_id == bike.School_id && t.Type == 3).ToList();

            Entities.Bikesite bsite = null;
            //Dictionary<string,string> temp = new Dictionary<string, string>();
            double mindistance = 99999999;
            foreach (var bikesite in bikesitelist)
            {
                var bikesitegps = bikesite.Gps_point.Split(',');
                var bs_lon = double.Parse(bikesitegps[0]);
                var bs_lat = double.Parse(bikesitegps[1]);

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

            var paydocno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
            var startdate = DateTime.Now;
            //pay_status 1  租车未还车
            var sqlstr =
                "insert into track(bike_id,user_id,created_at,updated_at,start_site_id,start_point,start_time,pay_docno,pay_status) values(" + bike.Id + "," + input.user_id + ",'" + startdate + "','" + startdate + "'," + bsite.Id + ",'" + input.gps_point + "','" + startdate + "','" + paydocno + "',1)";
            await _sqlExecuter.ExecuteAsync(sqlstr);

            

            var gps = bsite.Gps_point;
            var gpss = gps.Split(',');
            var output = new RentalBikeOutput
            {
                out_trade_no = paydocno,
                lon = double.Parse(gpss[0]),
                lat = double.Parse(gpss[1]),
                start_site_name = bsite.Name,
                ble_name = input.Ble_name,
                start_time = startdate.ToString("yyyy/MM/dd HH:mm:ss"),
                pwd = bike.Lock_pwd,
                BikesiteList = bikesitelist.MapTo<List<BikesiteEntity>>()
            };
            bike.Bike_status = 0; //出租中
            await _bikeRepository.UpdateAsync(bike);
            bsite.Available_count = bsite.Available_count - 1;
            await _bikesiteRepository.UpdateAsync(bsite);
            return output;
        }

        public async Task<RentalCostOutput> RentalBikeFinishtemp(RentalBikeInput input)
        {
            var track = await _trackRepository.FirstOrDefaultAsync(
                    t => t.Pay_docno == input.out_trade_no && t.User_id == input.user_id);
            if (track == null)
            {
                throw new UserFriendlyException("无该订单号!");
            }
            var bike = await _bikeRepository.FirstOrDefaultAsync(t => t.Ble_name == input.Ble_name);

            var gpsinput = input.gps_point.Split(',');
            var ip_lon = double.Parse(gpsinput[0]);
            var ip_lat = double.Parse(gpsinput[1]);
            var bikesitelist = _bikesiteRepository.GetAll().Where(t => t.School_id == bike.School_id && t.Type == 3).ToList();

            Entities.Bikesite bsite = null;

            foreach (var bikesite in bikesitelist)
            {
                var bikesitegps = bikesite.Gps_point.Split(',');
                var bs_lon = double.Parse(bikesitegps[0]);
                var bs_lat = double.Parse(bikesitegps[1]);

                var distance = LatlonHelper.GetDistance(ip_lat, ip_lon, bs_lat, bs_lon) * 1000;//KM->M
                if (distance <= bikesite.Radius)//15米
                {
                    bsite = bikesite;
                    break;
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
            await _bikeRepository.UpdateAsync(bike);

            bsite.Available_count = bsite.Available_count + 1;
            await _bikesiteRepository.UpdateAsync(bsite);

            await CurrentUnitOfWork.SaveChangesAsync();
            
            StringBuilder sb = new StringBuilder();
            sb.Append(
                "select a.id,a.created_at,a.updated_at,a.user_id,a.bike_id,e.ble_name,a.start_point,a.end_point,a.start_site_id,a.end_site_id,a.start_time,a.end_time,a.payment,a.pay_status,a.pay_method,a.pay_docno,a.remark,b.`name` as start_site_name,d.`name` as end_site_name,a.start_point, c.`name` as school_name,c.time_charge");
            sb.Append(" from track as a left join bikesite as b on a.start_site_id = b.id");
            sb.Append(" left join school as c on b.school_id = c.id");
            sb.Append(" left join bikesite as d on a.end_site_id = d.id");
            sb.Append(" left join bike as e on a.bike_id = e.id");
            sb.AppendFormat(" where a.pay_docno={0}", input.out_trade_no);
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

            

            TimeSpan costtime = endtime - DateTime.Parse(tracktemp.Start_time.ToString());

            var ctm = (int)costtime.TotalMinutes;//去掉多余的零头
            output.rental_time = ctm;
            if (ctm < 1)
            {
                output.allpay = "0";
                track.Should_pay = 0;
            }
            else
            {
                output.allpay = (ctm * tracktemp.time_charge / 100.00).ToString();//分转元
                track.Should_pay = ctm * tracktemp.time_charge / 100.00;//分转元
            }
            await _trackRepository.UpdateAsync(track);
            return output;
        }

        public async Task<RentalBikeOutput> RefreshBiketemp(RentalBikeInput input)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                "select a.id,a.created_at,a.updated_at,a.user_id,a.bike_id,e.ble_name,a.start_point,a.end_point,a.start_site_id,a.end_site_id,a.start_time,a.end_time,a.payment,a.should_pay,a.pay_status,a.pay_method,a.pay_docno,a.remark,b.`name` as start_site_name,b.gps_point as start_gps_point,d.`name` as end_site_name,a.start_point,b.school_id,c.`name` as school_name,c.time_charge,e.Lock_pwd");
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
                sb.AppendFormat(" where a.out_trade_no={0}", input.out_trade_no);
            }

            var track = _sqlExecuter.SqlQuery<TrackEntity>(sb.ToString()).ToList().FirstOrDefault();
            if (track == null)
            {
                throw new UserFriendlyException("没有行程单!");
            }
            var gpsinput = input.gps_point.Split(',');
            var ip_lon = double.Parse(gpsinput[0]);
            var ip_lat = double.Parse(gpsinput[1]);
            var bikesitelist = _bikesiteRepository.GetAll().Where(t => t.School_id == track.School_id && t.Type == 3).ToList();

            Entities.Bikesite bsite = null;

            foreach (var bikesite in bikesitelist)
            {
                var bikesitegps = bikesite.Gps_point.Split(',');
                var bs_lon = double.Parse(bikesitegps[0]);
                var bs_lat = double.Parse(bikesitegps[1]);

                var distance = LatlonHelper.GetDistance(ip_lat, ip_lon, bs_lat, bs_lon) * 1000;//KM->M
                //Logger.Info($"distance:{distance}---Radius:{bikesite.Radius}");
                if (distance <= bikesite.Radius)//15米
                {
                    bsite = bikesite;
                    break;
                }
            }

            var output = new RentalBikeOutput();
            if (bsite != null)
            {
                var gps = bsite.Gps_point;
                var gpss = gps.Split(',');
                output.lon_end = double.Parse(gpss[0]);
                output.lat_end = double.Parse(gpss[1]);
            }
            var gpsstart = track.Start_gps_point.Split(',');
            output.lon = double.Parse(gpsstart[0]);
            output.lat = double.Parse(gpsstart[1]);

            output.out_trade_no = track.Pay_docno;
            output.start_site_name = track.Start_site_name;
            output.ble_name = track.Ble_name;
            output.start_time = track.Start_time.ToString();
            output.end_site_name = bsite == null ? "" : bsite.Name;
            output.pwd = track.Lock_pwd;
            output.BikesiteList = bikesitelist.MapTo<List<BikesiteEntity>>();
            return output;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RentalInfoOutput> RentalFinishInfo([FromUri] RentalBikeInput input)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(
                "select a.id,a.created_at,a.updated_at,a.user_id,a.bike_id,e.ble_name,a.start_point,a.end_point,a.start_site_id,a.end_site_id,a.start_time,a.end_time,a.payment,a.should_pay,a.pay_status,a.pay_method,a.pay_docno,a.remark,b.`name` as start_site_name,d.`name` as end_site_name,a.start_point, c.`name` as school_name,c.time_charge");
            sb.Append(" from track as a left join bikesite as b on a.start_site_id = b.id");
            sb.Append(" left join school as c on b.school_id = c.id");
            sb.Append(" left join bikesite as d on a.end_site_id = d.id");
            sb.Append(" left join bike as e on a.bike_id = e.id");
            sb.AppendFormat(" where a.pay_docno={0}", input.out_trade_no);
            var track = _sqlExecuter.SqlQuery<TrackEntity>(sb.ToString()).ToList().FirstOrDefault();
            
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


            TimeSpan costtime = DateTime.Parse(track.End_time.ToString()) - DateTime.Parse(track.Start_time.ToString());

            var ctm = (int)costtime.TotalMinutes;//去掉多余的零头
            output.rental_time = ctm;
            output.allpay = track.Payment == null ? (ctm * track.time_charge / 100.00).ToString() : track.Payment.ToString();//分转元

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
            sb.Append("select a.id,a.created_at,a.updated_at,a.user_id,a.bike_id,a.start_point,a.end_point,a.start_site_id,a.end_site_id,a.start_time,a.end_time,a.payment,a.pay_status,a.pay_method,a.pay_docno,a.remark,a.start_point");
            sb.Append(" from track as a");
            sb.AppendFormat(" where a.user_id={0} and a.pay_status < 3", input.user_id);
            sb.Append(" order by a.updated_at desc LIMIT 1");

            var track = _sqlExecuter.SqlQuery<TrackEntity>(sb.ToString()).FirstOrDefault();
            if (track == null)
            {
                return new TrackInfoOutput {out_trade_no = "", pay_status = 0};
            }
            var result = new TrackInfoOutput {out_trade_no = track.Pay_docno, pay_status = track.Pay_status};
            return result;
        }
        /// <summary>
        /// 结束租车
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
            //var bike = await _bikeRepository.FirstOrDefaultAsync(t => t.Ble_name == input.Ble_name);

            //var gpsinput = input.gps_point.Split(',');
            //var ip_lon = double.Parse(gpsinput[0]);
            //var ip_lat = double.Parse(gpsinput[1]);
            //var bikesitelist = _bikesiteRepository.GetAll().Where(t => t.School_id == bike.School_id).ToList();

            //Entities.Bikesite bsite = null;

            //foreach (var bikesite in bikesitelist)
            //{
            //    var bikesitegps = bikesite.Gps_point.Split(',');
            //    var bs_lon = double.Parse(bikesitegps[0]);
            //    var bs_lat = double.Parse(bikesitegps[1]);

            //    var distance = LatlonHelper.GetDistance(ip_lat, ip_lon, bs_lat, bs_lon) * 1000;//KM->M
            //    if (distance <= bikesite.Radius)//15米
            //    {
            //        bsite = bikesite;
            //        break;
            //    }
            //}
            //if (bsite == null)
            //{
            //    throw new UserFriendlyException("范围内没有桩点");
            //}
            //var endtime = DateTime.Now;

            //track.End_point = input.gps_point;
            //track.End_site_id = bsite.Id;
            //track.End_time = endtime;
            //track.Updated_at = endtime;
            track.Payment = 0;
            track.Pay_method = "免费";
            track.Pay_status = 3;//免费，直接结束

            await _trackRepository.UpdateAsync(track);

            //bike.Bike_status = 1;
            //await _bikeRepository.UpdateAsync(bike);

            return new RentalFinishOutput { end_time = track.End_time.ToString(), out_trade_no = track.Pay_docno };
        }
    }
}