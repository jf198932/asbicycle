using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using ASBicycle.AliPay.App;
using ASBicycle.Web.Models.Alipay;

namespace ASBicycle.Web.Controllers.Alipay
{
    public class AlipayController : ASBicycleControllerBase
    {
        private readonly IRepository<Entities.Track> _trackRepository;

        public AlipayController(IRepository<Entities.Track> trackRepository)
        {
            _trackRepository = trackRepository;
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
        [HttpPost]
        public ActionResult Notify(Notifyrul notifyurl)
        {
            Dictionary<string, string> sPara = GetRequestPost();
            //string notify_id = Request.Form["notify_id"];//获取notify_id

            //string sign = Request.Form["sign"];//获取sign

            if (!string.IsNullOrEmpty(notifyurl.notify_id))//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                if (aliNotify.GetResponseTxt(notifyurl.notify_id) == "true")
                {
                    if (aliNotify.GetSignVeryfy(sPara, notifyurl.sign))
                    {
                        //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                        //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                        //商户订单号
                        //string out_trade_no = Request.Form["out_trade_no"];

                        //支付宝交易号
                        //string trade_no = Request.Form["trade_no"];

                        //交易状态
                        //string trade_status = Request.Form["trade_status"];
                        
                        if (notifyurl.trade_status == "TRADE_FINISHED")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                            //如果有做过处理，不执行商户的业务程序

                            var track = _trackRepository.GetAll().FirstOrDefault(t => t.Pay_docno == notifyurl.out_trade_no);
                            track.Pay_status = 5;
                            track.Pay_method = "支付宝";
                            track.Payment = int.Parse(Request.Form["total_fee"]);
                            _trackRepository.Update(track);
                            //注意：
                            //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                        }
                        else if (notifyurl.trade_status == "TRADE_SUCCESS")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                            //如果有做过处理，不执行商户的业务程序
                            var track = _trackRepository.GetAll().FirstOrDefault(t => t.Pay_docno == notifyurl.out_trade_no);
                            track.Pay_status = 4;
                            track.Pay_method = "支付宝";
                            track.Payment = int.Parse(Request.Form["total_fee"]);
                            _trackRepository.Update(track);


                            //注意：
                            //付款完成后，支付宝系统发送该交易状态通知
                        }
                        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
                        Response.Write("success");  //请不要修改或删除
                    }
                    else
                    {
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

        public ActionResult Return()
        {
            Dictionary<string, string> sPara = GetRequestPost();
            string notify_id = Request.Form["notify_id"];//获取notify_id

            string sign = Request.Form["sign"];//获取sign

            if (notify_id != null && notify_id != "")//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                if (aliNotify.GetResponseTxt(notify_id) == "true")
                {
                    if (aliNotify.GetSignVeryfy(sPara, sign))
                    {
                        //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                        //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                        //商户订单号
                        string out_trade_no = Request.Form["out_trade_no"];

                        //支付宝交易号
                        string trade_no = Request.Form["trade_no"];

                        //交易状态
                        string trade_status = Request.Form["trade_status"];

                        if (Request.Form["trade_status"] == "TRADE_FINISHED")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                        }
                        else if (Request.Form["trade_status"] == "TRADE_SUCCESS")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                            //如果有做过处理，不执行商户的业务程序



                            //注意：
                            //付款完成后，支付宝系统发送该交易状态通知
                        }
                        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
                        Response.Write("success");  //请不要修改或删除
                    }
                    else
                    {
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

        [HttpPost]
        public ActionResult Return(Returnurl returnurl)
        {
            Notify aliNotify = new Notify();

            //获取待验签数据
            Dictionary<string, string> sPara = GetRequestPost();

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
    }
}