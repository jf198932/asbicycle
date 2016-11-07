using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Logging;
using ASBicycle.AliPay.App;
using ASBicycle.Track;

namespace ASBicycle.Web.Controllers.Weixin
{
    public class WxpayController : ASBicycleControllerBase
    {
        private readonly ITrackWriteRepository _trackRepository;

        public WxpayController(ITrackWriteRepository trackWriteRepository)
        {
            _trackRepository = trackWriteRepository;
        }

        [HttpPost, UnitOfWork]
        public virtual ActionResult Notify()
        {
            //LogHelper.Logger.Info("进入回调");
            var wxxml = getPostStr();
            LogHelper.Logger.Info(wxxml);

            var xdoc = new XmlDocument();
            xdoc.LoadXml(wxxml);
            XmlNode xn = xdoc.SelectSingleNode("xml");
            XmlNodeList xnl = xn.ChildNodes;
            
            if (xnl[0].InnerText == "SUCCESS")
            {
                Response.Write("SUCCESS");  //请不要修改或删除
            }
            else
            {
                Response.Write("FAIL");  //请不要修改或删除
            }

            //Dictionary<string, string> sPara = GetRequestPost();
            //string notify_id = Request.Form["notify_id"];//获取notify_id

            //string sign = Request.Form["sign"];//获取sign

            //if (!string.IsNullOrEmpty(notify_id))//判断是否有带返回参数
            //{
            //    //LogHelper.Logger.Info($"notify_id={notify_id}，sign={sign}");

            //    Notify aliNotify = new Notify();
            //    if (aliNotify.GetResponseTxt(notify_id) == "true")
            //    {
            //        //LogHelper.Logger.Info("请求验证通过");

            //        if (aliNotify.GetSignVeryfy(sPara, sign))
            //        {
            //            //LogHelper.Logger.Info($"{Core.CreateLinkString2(sPara)}");

            //            //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
            //            //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

            //            //商户订单号
            //            string out_trade_no = Request.Form["out_trade_no"];

            //            //支付宝交易号
            //            string trade_no = Request.Form["trade_no"];

            //            //交易状态
            //            string trade_status = Request.Form["trade_status"];

            //            //LogHelper.Logger.Info($"out_trade_no={out_trade_no}，trade_no={trade_no}，trade_status={trade_status}");

            //            if (trade_status == "TRADE_FINISHED")
            //            {
            //                //LogHelper.Logger.Info("进入交易结束");

            //                //判断该笔订单是否在商户网站中已经做过处理
            //                //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
            //                //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
            //                //如果有做过处理，不执行商户的业务程序

            //                var track = _trackRepository.GetAll().FirstOrDefault(t => t.Pay_docno == out_trade_no);
            //                track.Pay_status = 3;
            //                track.Trade_no = trade_no;
            //                track.Pay_method = "支付宝";
            //                track.Payment = double.Parse(Request.Form["total_fee"]);
            //                //LogHelper.Logger.Info(Request.Form["total_fee"]);
            //                _trackRepository.Update(track);
            //                //注意：
            //                //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
            //            }
            //            else if (trade_status == "TRADE_SUCCESS")
            //            {
            //                //LogHelper.Logger.Info("进入交易成功");
            //                //LogHelper.Logger.Info($"payment:{double.Parse(Request.Form["total_fee"])}");
            //                //判断该笔订单是否在商户网站中已经做过处理
            //                //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
            //                //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
            //                //如果有做过处理，不执行商户的业务程序
            //                var track = _trackRepository.GetAll().FirstOrDefault(t => t.Pay_docno == out_trade_no);
            //                track.Pay_status = 3;
            //                track.Trade_no = trade_no;
            //                track.Pay_method = "支付宝";
            //                track.Payment = double.Parse(Request.Form["total_fee"]);
            //                //LogHelper.Logger.Info(Request.Form["total_fee"]);
            //                _trackRepository.Update(track);


            //                //注意：
            //                //付款完成后，支付宝系统发送该交易状态通知
            //            }
            //            //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
            //            Response.Write("success");  //请不要修改或删除
            //        }
            //        else
            //        {
            //            //LogHelper.Logger.Info("签名验证失败");
            //            Response.Write("sign fail!");
            //        }
            //    }
            //    else
            //    {
            //        Response.Write("response fail!");
            //    }
            //}
            //else
            //{
            //    Response.Write("非通知参数!");
            //}
            return View();
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public Dictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArraytemp = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArraytemp.Add(requestItem[i], Request.Form[requestItem[i]]);
            }
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in sArraytemp)
            {
                sArray.Add(temp.Key, temp.Value);
            }
            return sArray;
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