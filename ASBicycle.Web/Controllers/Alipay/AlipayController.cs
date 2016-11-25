using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Uow;
using Abp.Logging;
using ASBicycle.AliPay.App;
using ASBicycle.Recharge;
using ASBicycle.Recharge_detail;
using ASBicycle.Track;
using ASBicycle.Web.Models.Alipay;

namespace ASBicycle.Web.Controllers.Alipay
{
    public class AlipayController : ASBicycleControllerBase
    {
        private readonly ITrackWriteRepository _trackRepository;
        private readonly IRecharge_detailWriteRepository _rechargeDetailWriteRepository;
        private readonly IRechargeWriteRepository _rechargeWriteRepository;

        public AlipayController(ITrackWriteRepository trackRepository, IRecharge_detailWriteRepository rechargeDetailWriteRepository, IRechargeWriteRepository rechargeWriteRepository)
        {
            _trackRepository = trackRepository;
            _rechargeDetailWriteRepository = rechargeDetailWriteRepository;
            _rechargeWriteRepository = rechargeWriteRepository;
        }

        // GET: Alipay
        //public ActionResult Notify()
        //{
        //    Dictionary<string, string> sPara = GetRequestPost();
        //    string notify_id = Request.Form["notify_id"];//获取notify_id

        //    string sign = Request.Form["sign"];//获取sign

        //    if (notify_id != null && notify_id != "")//判断是否有带返回参数
        //    {
        //        Notify aliNotify = new Notify();
        //        if (aliNotify.GetResponseTxt(notify_id) == "true")
        //        {
        //            if (aliNotify.GetSignVeryfy(sPara, sign))
        //            {
        //                //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
        //                //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

        //                //商户订单号
        //                string out_trade_no = Request.Form["out_trade_no"];

        //                //支付宝交易号
        //                string trade_no = Request.Form["trade_no"];

        //                //交易状态
        //                string trade_status = Request.Form["trade_status"];

        //                if (Request.Form["trade_status"] == "TRADE_FINISHED")
        //                {
        //                    //判断该笔订单是否在商户网站中已经做过处理
        //                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
        //                    //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
        //                    //如果有做过处理，不执行商户的业务程序

        //                    //注意：
        //                    //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
        //                }
        //                else if (Request.Form["trade_status"] == "TRADE_SUCCESS")
        //                {
        //                    //判断该笔订单是否在商户网站中已经做过处理
        //                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
        //                    //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
        //                    //如果有做过处理，不执行商户的业务程序



        //                    //注意：
        //                    //付款完成后，支付宝系统发送该交易状态通知
        //                }
        //                //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
        //                Response.Write("success");  //请不要修改或删除
        //            }
        //            else
        //            {
        //                Response.Write("sign fail!");
        //            }
        //        }
        //        else
        //        {
        //            Response.Write("response fail!");
        //        }
        //    }
        //    else
        //    {
        //        Response.Write("非通知参数!");
        //    }
        //    return View();
        //}
        [HttpPost, UnitOfWork]
        public virtual async Task<ActionResult> Notify()
        {
            //LogHelper.Logger.Info("进入回调");

            SortedDictionary<string, string> sPara = GetRequestPost();
            string notify_id = Request.Form["notify_id"];//获取notify_id

            string sign = Request.Form["sign"];//获取sign

            if (!string.IsNullOrEmpty(notify_id))//判断是否有带返回参数
            {
                //LogHelper.Logger.Info($"notify_id={notify_id}，sign={sign}");

                Notify aliNotify = new Notify();
                if (aliNotify.GetResponseTxt(notify_id) == "true")
                {
                    //LogHelper.Logger.Info("请求验证通过");
                    
                    if (aliNotify.GetSignVeryfy(sPara, sign))
                    {
                        //LogHelper.Logger.Info($"{Core.CreateLinkString2(sPara)}");

                        //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                        //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                        //商户订单号
                        string out_trade_no = Request.Form["out_trade_no"];

                        //支付宝交易号
                        string trade_no = Request.Form["trade_no"];

                        //交易状态
                        string trade_status = Request.Form["trade_status"];

                        //LogHelper.Logger.Info($"out_trade_no={out_trade_no}，trade_no={trade_no}，trade_status={trade_status}");

                        if (trade_status == "TRADE_FINISHED")
                        {
                            //LogHelper.Logger.Info("进入交易结束");

                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                            //如果有做过处理，不执行商户的业务程序

                            var tracks = await _trackRepository.GetAllListAsync(t => t.Pay_docno == out_trade_no);
                            if (tracks != null && tracks.Count > 0)
                            {
                                var track = tracks.FirstOrDefault();
                                track.Pay_status = 3;
                                track.Trade_no = trade_no;
                                track.Pay_method = "支付宝";
                                track.Payment = double.Parse(Request.Form["total_fee"]);
                                track.Updated_at = DateTime.Now;
                                track.Pay_time = DateTime.Parse(Request.Form["gmt_payment"]);
                                //LogHelper.Logger.Info(Request.Form["total_fee"]);
                                await _trackRepository.UpdateAsync(track);
                            }
                            else
                            {
                                var recharge_details =
                                    await
                                        _rechargeDetailWriteRepository.GetAllListAsync(
                                            t => t.recharge_docno == out_trade_no);
                                var recharge_detail = recharge_details.FirstOrDefault();
                                recharge_detail.Updated_at = DateTime.Now;
                                recharge_detail.Recharge_method = 1;
                                recharge_detail.doc_no = trade_no;
                                recharge_detail.Recharge_amount = double.Parse(Request.Form["total_fee"]);

                                await _rechargeDetailWriteRepository.UpdateAsync(recharge_detail);

                                var recharges = await _rechargeWriteRepository.GetAllListAsync(t => t.User_id == recharge_detail.User_id);

                                var recharge = recharges.FirstOrDefault();

                                recharge.Deposit = recharge_detail.Recharge_amount;
                                recharge.Updated_at = DateTime.Now;

                                await _rechargeWriteRepository.UpdateAsync(recharge);
                            }
                            
                            //注意：
                            //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                        }
                        else if (trade_status == "TRADE_SUCCESS")
                        {
                            //LogHelper.Logger.Info("进入交易成功");
                            //LogHelper.Logger.Info($"payment:{double.Parse(Request.Form["total_fee"])}");
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                            //如果有做过处理，不执行商户的业务程序
                            var tracks = await _trackRepository.GetAllListAsync(t => t.Pay_docno == out_trade_no);
                            if (tracks != null && tracks.Count > 0)
                            {
                                var track = tracks.FirstOrDefault();
                                track.Pay_status = 3;
                                track.Trade_no = trade_no;
                                track.Pay_method = "支付宝";
                                track.Updated_at = DateTime.Now;
                                track.Payment = double.Parse(Request.Form["total_fee"]);
                                //LogHelper.Logger.Info(Request.Form["total_fee"]);
                                await _trackRepository.UpdateAsync(track);
                            }
                            else
                            {
                                var recharge_details =
                                    await
                                        _rechargeDetailWriteRepository.GetAllListAsync(
                                            t => t.recharge_docno == out_trade_no);
                                var recharge_detail = recharge_details.FirstOrDefault();
                                recharge_detail.Updated_at = DateTime.Now;
                                recharge_detail.Recharge_method = 1;
                                recharge_detail.doc_no = trade_no;
                                recharge_detail.Recharge_amount = double.Parse(Request.Form["total_fee"]);

                                await _rechargeDetailWriteRepository.UpdateAsync(recharge_detail);

                                var recharges = await _rechargeWriteRepository.GetAllListAsync(t => t.User_id == recharge_detail.User_id);

                                var recharge = recharges.FirstOrDefault();
                                recharge.Deposit = recharge_detail.Recharge_amount;
                                recharge.Updated_at = DateTime.Now;

                                await _rechargeWriteRepository.UpdateAsync(recharge);
                            }


                            //注意：
                            //付款完成后，支付宝系统发送该交易状态通知
                        }
                        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
                        Response.Write("success");  //请不要修改或删除
                    }
                    else
                    {
                        //LogHelper.Logger.Info("签名验证失败");
                        Response.Write("sign fail!");
                    }
                }
                else
                {
                    Response.Write("response fail!");
                }
            }
            else
            {
                Response.Write("非通知参数!");
            }
            return View();
        }

        [HttpPost, UnitOfWork]
        public ActionResult Return(Returnurl returnurl)
        {
            Notify aliNotify = new Notify();

            //获取待验签数据
            SortedDictionary<string, string> sPara = GetRequestPost();

            //获取同步返回中的success.
            string success = Request.Form["success"];

            //获取同步返回中的sign.
            string sign = Request.Form["sign"].Replace("\"", "");

            //注意：在客户端把返回参数请求过来的时候务必要把sign做一次urlencode,保证特殊字符不会被转义。
            if (success == "\"true\"")//判断success是否为true.
            {
                //判断配置是否匹配.
                if (Request.Form["partner"].Replace("\"", "") == Config.partner && Request.Form["service"].Replace("\"", "") == Config.service)
                {
                    //除去数组中的空值和签名参数,且把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
                    string data = Core.CreateLinkString(Core.FilterPara(sPara));

                    //Core.LogResult(data);//调试用，判断待验签参数是否和客户端一致。
                    //Core.LogResult(sign);//调试用，判断sign值是否和客户端请求时的一致，                
                    bool issign = false;

                    //获得验签结果
                    issign = RSA.verify(data, sign, AliPay.App.Notify.getPublicKeyStr(Config.alipay_public_key), Config.input_charset);
                    if (issign)
                    {
                        //此处可做商家业务逻辑，建议商家以异步通知为准。
                        Response.Write("return success!");
                    }
                    else
                    {
                        Response.Write("return fail!");
                    }
                }
            }
            return View();
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            Dictionary<string, string> sArraytemp = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArraytemp.Add(requestItem[i], Request.Form[requestItem[i]]);
            }
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in sArraytemp)
            {
                sArray.Add(temp.Key, temp.Value);
            }
            return sArray;
        }
    }
}