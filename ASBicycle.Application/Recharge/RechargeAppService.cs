using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Abp.Logging;
using Abp.UI;
using ASBicycle.AliPay.App;
using ASBicycle.Common;
using ASBicycle.Recharge.Dto;
using ASBicycle.Recharge_detail;
using ASBicycle.Refound;
using ASBicycle.WxPay.App;

namespace ASBicycle.Recharge
{
    public class RechargeAppService : ASBicycleAppServiceBase, IRechargeAppService
    {
        private readonly IRechargeWriteRepository _rechargeWriteRepository;
        private readonly IRecharge_detailWriteRepository _rechargeDetailWriteRepository;
        private readonly IRefoundWriteRepository _refoundWriteRepository;

        public RechargeAppService(IRechargeWriteRepository rechargeWriteRepository,
            IRecharge_detailWriteRepository rechargeDetailWriteRepository,
            IRefoundWriteRepository refoundWriteRepository)
        {
            _rechargeWriteRepository = rechargeWriteRepository;
            _rechargeDetailWriteRepository = rechargeDetailWriteRepository;
            _refoundWriteRepository = refoundWriteRepository;
        }

        public async Task<RechargeOutput> CreateRecharge(RechargeInput input)
        {
            var recharge = await _rechargeWriteRepository.GetAllListAsync(t => t.User_id == input.user_id);
            if (recharge == null || recharge.Count == 0)
            {
                await _rechargeWriteRepository.InsertAsync(new Entities.Recharge
                {
                    Created_at = DateTime.Now,
                    Updated_at = DateTime.Now,
                    User_id = input.user_id,
                    //Deposit = input.deposit
                });
            }
            
            var paydocno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
            await _rechargeDetailWriteRepository.InsertAsync(new Entities.Recharge_detail
            {
                Created_at = DateTime.Now,
                Updated_at = DateTime.Now,
                User_id = input.user_id,
                recharge_docno = paydocno,
                Type = input.type,
                status = 0,
                Recharge_type = input.recharge_type,
            });
            return new RechargeOutput {out_trade_no = paydocno};
        }

        public async Task ApplyRefound(RefoundInput input)
        {
            //var refound = await _refoundWriteRepository.FirstOrDefaultAsync(t => t.User_id == input.User_id);
            //if (refound != null)
            //{
            //    refound.Refound_status = 1;
            //    refound.Updated_at = DateTime.Now;
            //    await _refoundWriteRepository.UpdateAsync(refound);
            //}
            //else
            //{
            //    await _refoundWriteRepository.InsertAsync(new Entities.Refound
            //    {
            //        Created_at = DateTime.Now,
            //        Updated_at = DateTime.Now,
            //        Refound_status = 1,
            //        User_id = input.User_id,
            //        Refound_amount = input.Amount
            //    });
            //}

            var details =
                await
                    _rechargeDetailWriteRepository.GetAllListAsync(
                        t => t.User_id == input.User_id && t.Type == 1 && t.Recharge_type == input.Recharge_type && t.status == 0 && t.doc_no != null);


            foreach (var rechargeDetail in details)
            {
                if (rechargeDetail == null)
                {
                    throw new UserFriendlyException("没有可退款的订单");
                }
                if (rechargeDetail.Recharge_type == 1)
                {
                    rechargeDetail.status = 1;
                    rechargeDetail.Updated_at = DateTime.Now;
                    await _rechargeDetailWriteRepository.UpdateAsync(rechargeDetail);
                }
            }
            
            //var paydocno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
            //await _rechargeDetailWriteRepository.InsertAsync(new Entities.Recharge_detail
            //{
            //    Created_at = DateTime.Now,
            //    Updated_at = DateTime.Now,
            //    User_id = input.User_id,
            //    recharge_docno = paydocno,
            //    Type = 2,
            //    Recharge_type = input.Recharge_type,
            //});
            //throw new NotImplementedException();
        }

        public async Task<RefoundOutput> ApplyRefoundTimely(RefoundInput input)
        {
            var detail = await _rechargeDetailWriteRepository.FirstOrDefaultAsync(t => t.User_id == input.User_id && t.Recharge_type == 1 && t.Type == 1 && t.status == 0 && t.doc_no != null);
            if(detail == null)
                throw new UserFriendlyException("没有可以退款的订单");
            var result = new RefoundOutput {code = 1, msg = "FAIL"};

            if (detail.Recharge_method == 1)//支付宝
            {
                //退款详细数据，必填，格式（支付宝交易号^退款金额^备注），多笔请用#隔开
                string detail_data = detail.doc_no + "^" + detail.Recharge_amount + "^" + "正常退款";//"2016111821001004570251489448^9.9^正常退款";
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
                //LogHelper.Logger.Info(sHtmlText);

                XmlDocument xmlDoc = new XmlDocument();
                
                xmlDoc.LoadXml(sHtmlText);
                XmlNode xn = xmlDoc.SelectSingleNode("alipay");
                XmlNodeList xnl = xn.ChildNodes;
                if (xnl[0].InnerText == "T")
                {
                    detail.status = 2;
                    detail.Updated_at = DateTime.Now;
                    _rechargeDetailWriteRepository.Update(detail);

                    var recharge = _rechargeWriteRepository.FirstOrDefault(t => t.User_id == detail.User_id);
                    recharge.Deposit = 0;
                    _rechargeWriteRepository.Update(recharge);

                    _rechargeDetailWriteRepository.Insert(new Entities.Recharge_detail
                    {
                        Created_at = DateTime.Now,
                        Updated_at = DateTime.Now,
                        User_id = detail.User_id,
                        Recharge_amount = detail.Recharge_amount,
                        Recharge_method = 1,
                        Recharge_type = detail.Recharge_type,
                        recharge_docno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999),
                        doc_no = "",
                        Type = 2,
                        status = 0,
                        source_recharge_docno = detail.recharge_docno,
                        source_doc_no = detail.doc_no
                    });

                    result.code = 0;
                    result.msg = "SUCCESS";
                    return result;
                }
                else
                {
                    result.code = 1;
                    result.msg = "FAIL";
                    return result;
                }
            }
            else if (detail.Recharge_method == 2) //微信
            {
                var noncestr = CommonUtil.CreateNoncestr();
                //var timespan = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();

                Dictionary<string, string> sPara = new Dictionary<string, string>();
                sPara.Add("appid", WxpayConfig.appId);
                sPara.Add("mch_id", WxpayConfig.mchid);
                sPara.Add("nonce_str", noncestr);
                sPara.Add("transaction_id", detail.doc_no);
                sPara.Add("out_refund_no", DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999));
                sPara.Add("total_fee", (detail.Recharge_amount * 100).ToString());
                sPara.Add("refund_fee", (detail.Recharge_amount * 100).ToString());
                sPara.Add("op_user_id", WxpayConfig.mchid);

                var wxpayhelper = new WxPayHelper();
                var sign = wxpayhelper.GetBizSign(sPara, false);
                sPara.Add("sign", sign);

                var requestXml = CommonUtil.ArrayToXml(sPara);

                //string cert = @"~/cert/apiclient_cert.p12";
                string cert = HttpContext.Current.Server.MapPath("~/cert/apiclient_cert.p12");
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                var cer = new X509Certificate(cert, WxpayConfig.mchid, X509KeyStorageFlags.MachineKeySet);

                var resp = HttpHelper.PostDataToServerForHttps(
                    "https://api.mch.weixin.qq.com/secapi/pay/refund", requestXml, HttpWebRequestMethod.POST, cer);

                //todo log
                //LogHelper.Logger.Info(resp);

                var xdoc = new XmlDocument();
                xdoc.LoadXml(resp);
                XmlNode xn = xdoc.SelectSingleNode("xml");
                XmlNodeList xnl = xn.ChildNodes;

                if (xnl[6].InnerText == "SUCCESS") //退款成功
                {
                    detail.status = 2;
                    detail.Updated_at = DateTime.Now;
                    _rechargeDetailWriteRepository.Update(detail);

                    var recharge = _rechargeWriteRepository.FirstOrDefault(t => t.User_id == detail.User_id);
                    recharge.Deposit = 0;
                    _rechargeWriteRepository.Update(recharge);

                    _rechargeDetailWriteRepository.Insert(new Entities.Recharge_detail
                    {
                        Created_at = DateTime.Now,
                        Updated_at = DateTime.Now,
                        User_id = detail.User_id,
                        Recharge_amount = double.Parse(xnl[12].InnerText)/100,
                        Recharge_method = 2,
                        Recharge_type = detail.Recharge_type,
                        recharge_docno = DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999),
                        doc_no = xnl[10].InnerText,
                        Type = 2,
                        status = 0,
                        source_recharge_docno = detail.recharge_docno,
                        source_doc_no = detail.doc_no
                    });

                    result.code = 0;
                    result.msg = "SUCCESS";
                    return result;
                }
                else
                {
                    result.code = 1;
                    result.msg = "FAIL";
                    return result;
                }
            }
            return result;
        }


        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
    }
}