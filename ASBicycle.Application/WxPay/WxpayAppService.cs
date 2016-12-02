using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Web;
using System.Xml;
using Abp.UI;
using ASBicycle.Common;
using ASBicycle.Track;
using ASBicycle.WxPay.App;
using ASBicycle.WxPay.Dto;

namespace ASBicycle.WxPay
{
    public class WxpayAppService : ASBicycleAppServiceBase, IWxpayAppService
    {
        //private readonly ITrackWriteRepository _trackWriteRepository;

        public WxpayAppService(ITrackWriteRepository trackWriteRepository)
        {
            //_trackWriteRepository = trackWriteRepository;
        }
        private string prepayId = "";

        public WxpayOutput signatures(SignaturesInput input)
        {
            var noncestr = CommonUtil.CreateNoncestr();
            var timespan = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000)/10000000).ToString();
            //var paydocno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
            //await _trackWriteRepository.InsertAsync(new Entities.Track
            //{
            //    Start_time = DateTime.Now,
            //    Start_point = "0,0",
            //    User_id = input.user_id,
            //    Pay_docno = paydocno
            //});

            Dictionary<string, string> sPara = new Dictionary<string, string>();
            sPara.Add("appid", WxpayConfig.appId);
            sPara.Add("body", input.body);
            //sPara.Add("input_charset", "UTF-8");
            sPara.Add("mch_id", WxpayConfig.mchid);
            sPara.Add("nonce_str", noncestr);
            sPara.Add("notify_url", input.type == 1? ConfigurationManager.AppSettings["Wxpay_notify_url"] : ConfigurationManager.AppSettings["Wxpay_notify_url2"]);
            sPara.Add("out_trade_no", input.out_trade_no);
            sPara.Add("spbill_create_ip", HttpContext.Current.Request.UserHostAddress);
            //sPara.Add("spbill_create_ip", "192.168.1.69");
            sPara.Add("total_fee", (input.total_fee * 100).ToString());
            sPara.Add("trade_type", "APP");
            

            var wxpayhelper = new WxPayHelper();

            var sign = wxpayhelper.GetBizSign(sPara, false);

            sPara.Add("sign", sign);

            var requestXml = CommonUtil.ArrayToXml(sPara);

            //var convertXml = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.Default.GetBytes(requestXml));

            var result = HttpHelper.PostDataToServerForHttps("https://api.mch.weixin.qq.com/pay/unifiedorder", requestXml, HttpWebRequestMethod.POST);

            //获取预支付ID
            var xdoc = new XmlDocument();
            xdoc.LoadXml(result);
            XmlNode xn = xdoc.SelectSingleNode("xml");
            XmlNodeList xnl = xn.ChildNodes;
            if (xnl[0].InnerText != "FAIL")
            {
                if (xnl.Count > 7)
                {
                    prepayId = xnl[7].InnerText;
                    //package = string.Format("prepay_id={0}", prepayId);
                }
                Dictionary<string, string> sPara2 = new Dictionary<string, string>();
                sPara2.Add("appid", WxpayConfig.appId);
                sPara2.Add("noncestr", noncestr);
                sPara2.Add("package", WxpayConfig.package);
                sPara2.Add("partnerid", WxpayConfig.mchid);
                sPara2.Add("prepayid", prepayId);
                sPara2.Add("timestamp", timespan);

                var sign2 = wxpayhelper.GetBizSign(sPara2, false);

                var outresult = new WxpayOutput();
                outresult.appid = WxpayConfig.appId;
                outresult.noncestr = noncestr;
                outresult.package = WxpayConfig.package;
                outresult.partnerid = WxpayConfig.mchid;
                outresult.prepayid = prepayId;
                outresult.timestamp = timespan;
                outresult.sign = sign2;

                return outresult;
            }
            else
            {
                throw new UserFriendlyException(xnl[1].InnerText);
            }

            //StringBuilder sb = new StringBuilder();
            //sb.Append("<xml>");
            //sb.AppendFormat("<appid>{0}</appid>", WxpayConfig.appId);
            //sb.AppendFormat("<mch_id>{0}</mch_id>", WxpayConfig.mchid);
            //sb.AppendFormat("<nonce_str>{0}</nonce_str>", CommonUtil.CreateNoncestr());
            //sb.AppendFormat("<body>{0}</body>", input.body);
            //sb.AppendFormat("<out_trade_no>{0}</out_trade_no>", input.out_trade_no);
            //sb.AppendFormat("<total_fee>{0}</total_fee>", input.total_fee);
            //sb.AppendFormat("<spbill_create_ip>{0}</spbill_create_ip>", HttpContext.Current.Request.UserHostAddress);
            //sb.Append("<trade_type>APP</trade_type>");
            //sb.AppendFormat("<notify_url>{0}</notify_url>", ConfigurationManager.AppSettings["Wxpay_notify_url"]);

            //string prepayXml = HttpUtil.Send(data, "https://api.mch.weixin.qq.com/pay/unifiedorder");
            throw new System.NotImplementedException();
        }
    }
}