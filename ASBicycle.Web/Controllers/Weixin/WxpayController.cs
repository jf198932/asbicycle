using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Abp.Domain.Uow;
using Abp.Logging;
using ASBicycle.Recharge;
using ASBicycle.Recharge_detail;
using ASBicycle.Track;

namespace ASBicycle.Web.Controllers.Weixin
{
    public class WxpayController : ASBicycleControllerBase
    {
        private readonly ITrackWriteRepository _trackRepository;
        private readonly IRecharge_detailWriteRepository _rechargeDetailWriteRepository;
        private readonly IRechargeWriteRepository _rechargeWriteRepository;

        public WxpayController(ITrackWriteRepository trackRepository, IRecharge_detailWriteRepository rechargeDetailWriteRepository, IRechargeWriteRepository rechargeWriteRepository)
        {
            _trackRepository = trackRepository;
            _rechargeDetailWriteRepository = rechargeDetailWriteRepository;
            _rechargeWriteRepository = rechargeWriteRepository;
        }

        [HttpPost, UnitOfWork]
        public virtual async Task<ActionResult> Notify()
        {
            //LogHelper.Logger.Info("进入回调");
            var wxxml = getPostStr();
            //LogHelper.Logger.Info(wxxml);

            var xdoc = new XmlDocument();
            xdoc.LoadXml(wxxml);
            XmlNode xn = xdoc.SelectSingleNode("xml");
            XmlNodeList xnl = xn.ChildNodes;
            
            if (xnl[9].InnerText == "SUCCESS")
            {
                var out_trade_no = xnl[8].InnerText;
                var total_fee = xnl[13].InnerText;

                var tracks = await _trackRepository.GetAllListAsync(t => t.Pay_docno == out_trade_no);
                if (tracks != null && tracks.Count > 0)
                {
                    var track = tracks.FirstOrDefault();
                    track.Pay_status = 3;
                    track.Trade_no = xnl[15].InnerText;
                    track.Pay_method = "微信";
                    track.Payment = double.Parse(total_fee)/100;
                    //LogHelper.Logger.Info(Request.Form["total_fee"]);
                    await _trackRepository.UpdateAsync(track);
                }
                else
                {
                    var recharge_details =
                        await _rechargeDetailWriteRepository.GetAllListAsync(t => t.recharge_docno == out_trade_no);
                    var recharge_detail = recharge_details.FirstOrDefault();
                    recharge_detail.Updated_at = DateTime.Now;
                    recharge_detail.Recharge_method = 2;
                    recharge_detail.doc_no = xnl[15].InnerText;
                    recharge_detail.Recharge_amount = double.Parse(total_fee)/100;

                    await _rechargeDetailWriteRepository.UpdateAsync(recharge_detail);

                    var recharges =
                        await _rechargeWriteRepository.GetAllListAsync(t => t.User_id == recharge_detail.User_id);

                    var recharge = recharges.FirstOrDefault();
                    recharge.Deposit = recharge_detail.Recharge_amount;
                    recharge.Updated_at = DateTime.Now;

                    await _rechargeWriteRepository.UpdateAsync(recharge);
                }

                Response.Write("SUCCESS");  //请不要修改或删除
            }
            else
            {
                Response.Write("FAIL");  //请不要修改或删除
            }
            
            return View();
        }

        public string getPostStr()
        {
            Int32 intLen = Convert.ToInt32(Request.InputStream.Length);
            byte[] b = new byte[intLen];
            Request.InputStream.Read(b, 0, intLen);
            return System.Text.Encoding.UTF8.GetString(b);
        }
    }
}