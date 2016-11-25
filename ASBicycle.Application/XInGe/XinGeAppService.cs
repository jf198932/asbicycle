using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml;
using Abp.Logging;
using ASBicycle.AliPay.App;
using ASBicycle.Common;
using ASBicycle.Recharge;
using ASBicycle.Recharge_detail;
using ASBicycle.WxPay.App;
using PushNotifications;
using PushNotifications.Schema;

namespace ASBicycle.XInGe
{
    public class XinGeAppService : ASBicycleAppServiceBase, IXinGeAppService
    {
        private readonly IRechargeWriteRepository _rechargeWriteRepository;
        private readonly IRecharge_detailWriteRepository _rechargeDetailWriteRepository;

        public XinGeAppService(IRechargeWriteRepository rechargeWriteRepository,
            IRecharge_detailWriteRepository rechargeDetailWriteRepository)
        {
            _rechargeWriteRepository = rechargeWriteRepository;
            _rechargeDetailWriteRepository = rechargeDetailWriteRepository;
        }

        public void SendAndroid(string account)
        {
            //XingeApp xa = new XingeApp("2100201817", "d3f09594e11b439a6cf3b6b725698831");
            //Msg_Android android = new Msg_Android_TouChuan("标题", XinGeConfig.message_type_touchuan)
            //{
            //    content = "内容"
            //};
            //android.message_type = 2;//消息
            Dictionary<string, object> d = new Dictionary<string, object>
            {
                {"site", "1号宿舍楼"},
                {"date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")},
                {"serial_id", "12341"}
            };
            ////d.Add("1", new CustomContentcs {site="1号宿舍楼", date=DateTime.Now, serial_id="12341"});
            //android.custom_content = d;
            //xa.PushToAccount(account, android);
            PushClient pc = new PushClient("2100201817", "d3f09594e11b439a6cf3b6b725698831");
            AndroidNotification an = new AndroidNotification("标题", "测试内容") {MessageType = MessageType.Penetrate};
            an.CustomItems.Add("site", "1号宿舍楼");
            an.CustomItems.Add("date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            an.CustomItems.Add("serial_id", "12341");
            pc.PushSingleAccountAsync(account, an);
        }

        public string SendIos([FromUri] int Recharge_method, double Recharge_amount, string doc_no)
        {
            ////退款详细数据，必填，格式（支付宝交易号^退款金额^备注），多笔请用#隔开
            //string detail_data = "2016111321001004950280009783" + "^" + "0.02" + "^" + "正常退款";//"2016111821001004570251489448^9.9^正常退款";
            //                                                                                 //把请求参数打包成数组
            //SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            //sParaTemp.Add("partner", Config.partner);
            //sParaTemp.Add("_input_charset", Config.input_charset.ToLower());
            //sParaTemp.Add("service", "refund_fastpay_by_platform_nopwd");
            ////sParaTemp.Add("notify_url", ConfigurationManager.AppSettings["Alipay_refund_notify_url"]);
            //sParaTemp.Add("batch_no", DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999));
            //sParaTemp.Add("refund_date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //sParaTemp.Add("batch_num", "1");
            //sParaTemp.Add("detail_data", detail_data);

            ////建立请求
            //string sHtmlText = Submit.BuildRequest(sParaTemp);
            ////todo log
            //LogHelper.Logger.Info(sHtmlText);
            //return sHtmlText;
            //return Encoding.GetEncoding("utf-8").GetString(Encoding.Unicode.GetBytes("中阿打算"));
            //return "的是否敢";




            //var detail = await _rechargeDetailWriteRepository.FirstOrDefaultAsync(t => t.User_id == User_id && t.status == 1);

            if (Recharge_method == 1)//支付宝
            {
                //退款详细数据，必填，格式（支付宝交易号^退款金额^备注），多笔请用#隔开
                string detail_data = doc_no + "^" + Recharge_amount + "^" + "正常退款";//"2016111821001004570251489448^9.9^正常退款";
                //把请求参数打包成数组
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner", Config.partner);
                sParaTemp.Add("_input_charset", Config.input_charset.ToLower());
                sParaTemp.Add("service", "refund_fastpay_by_platform_nopwd");
                //sParaTemp.Add("notify_url", ConfigurationManager.AppSettings["Alipay_refund_notify_url"]);
                sParaTemp.Add("batch_no", DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999));
                sParaTemp.Add("refund_date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sParaTemp.Add("batch_num", "1");
                sParaTemp.Add("detail_data", detail_data);

                //建立请求
                string sHtmlText = Submit.BuildRequest(sParaTemp);
                //todo log
                LogHelper.Logger.Info(sHtmlText);

                return sHtmlText;
                //XmlDocument xmlDoc = new XmlDocument();

                //xmlDoc.LoadXml(sHtmlText);
                //XmlNode xn = xmlDoc.SelectSingleNode("alipay");
                //XmlNodeList xnl = xn.ChildNodes;
                //if (xnl[0].InnerText == "T")
                //{
                //    detail.status = 2;
                //    detail.Updated_at = DateTime.Now;
                //    _rechargeDetailWriteRepository.Update(detail);

                //    var recharge = _rechargeWriteRepository.FirstOrDefault(t => t.User_id == detail.User_id);
                //    recharge.Deposit = 0;
                //    _rechargeWriteRepository.Update(recharge);

                //    _rechargeDetailWriteRepository.Insert(new Entities.Recharge_detail
                //    {
                //        Created_at = DateTime.Now,
                //        Updated_at = DateTime.Now,
                //        User_id = detail.User_id,
                //        Recharge_amount = double.Parse(xnl[12].InnerText) / 100,
                //        Recharge_method = 1,
                //        Recharge_type = detail.Recharge_type,
                //        recharge_docno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999),
                //        doc_no = xnl[10].InnerText,
                //        Type = 2,
                //        status = 0
                //    });
                //}
            }
            else if (Recharge_method == 2) //微信
            {
                var noncestr = CommonUtil.CreateNoncestr();
                //var timespan = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();

                Dictionary<string, string> sPara = new Dictionary<string, string>();
                sPara.Add("appid", WxpayConfig.appId);
                sPara.Add("mch_id", WxpayConfig.mchid);
                sPara.Add("nonce_str", noncestr);
                sPara.Add("transaction_id", doc_no);
                sPara.Add("out_refund_no", DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999));
                sPara.Add("total_fee", (Recharge_amount * 100).ToString());
                sPara.Add("refund_fee", (Recharge_amount * 100).ToString());
                sPara.Add("op_user_id", WxpayConfig.mchid);

                var wxpayhelper = new WxPayHelper();
                var sign = wxpayhelper.GetBizSign(sPara, false);
                sPara.Add("sign", sign);

                var requestXml = CommonUtil.ArrayToXml(sPara);

                //string cert = @"D:\cert\apiclient_cert.p12";

                string cert = HttpContext.Current.Server.MapPath("~/cert/apiclient_cert.p12");
                LogHelper.Logger.Info(cert);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                //X509Certificate cer = new X509Certificate(cert, WxpayConfig.mchid);
                //X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                //store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                //System.Security.Cryptography.X509Certificates.X509Certificate cer =
                //    store.Certificates.Find(X509FindType.FindBySubjectName, "苏州爱尚骑行信息科技有限公司", false)[0];

                var cer = new X509Certificate(cert, WxpayConfig.mchid, X509KeyStorageFlags.MachineKeySet);

                var resp = HttpHelper.PostDataToServerForHttps(
                    "https://api.mch.weixin.qq.com/secapi/pay/refund", requestXml, HttpWebRequestMethod.POST, cer);

                //todo log
                LogHelper.Logger.Info(resp);

                return resp;
                //var xdoc = new XmlDocument();
                //xdoc.LoadXml(resp);
                //XmlNode xn = xdoc.SelectSingleNode("xml");
                //XmlNodeList xnl = xn.ChildNodes;

                //if (xnl[6].InnerText == "SUCCESS") //退款成功
                //{
                //    detail.status = 2;
                //    detail.Updated_at = DateTime.Now;
                //    _rechargeDetailWriteRepository.Update(detail);

                //    var recharge = _rechargeWriteRepository.FirstOrDefault(t => t.User_id == detail.User_id);
                //    recharge.Deposit = 0;
                //    _rechargeWriteRepository.Update(recharge);

                //    _rechargeDetailWriteRepository.Insert(new Entities.Recharge_detail
                //    {
                //        Created_at = DateTime.Now,
                //        Updated_at = DateTime.Now,
                //        User_id = detail.User_id,
                //        Recharge_amount = double.Parse(xnl[12].InnerText) / 100,
                //        Recharge_method = 1,
                //        Recharge_type = detail.Recharge_type,
                //        recharge_docno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999),
                //        doc_no = xnl[10].InnerText,
                //        Type = 2,
                //        status = 0
                //    });
                //}
            }
            return "";
        }


        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
    }
}