using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Abp.Web.Models;
using ASBicycle.Rental.AliPay.App;
using ASBicycle.Rental.AliPay.Dto;

namespace ASBicycle.Rental.AliPay
{
    public class AlipayAppService : ASBicycleAppServiceBase, IAlipayAppService
    {
        [HttpPost, DontWrapResult]
        public void notify_url()
        {
            Dictionary<string, string> sPara = GetRequestPost();
            string notify_id = HttpContext.Current.Request.Form["notify_id"];//获取notify_id

            string sign = HttpContext.Current.Request.Form["sign"];//获取sign

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
                        string out_trade_no = HttpContext.Current.Request.Form["out_trade_no"];

                        //支付宝交易号
                        string trade_no = HttpContext.Current.Request.Form["trade_no"];

                        //交易状态
                        string trade_status = HttpContext.Current.Request.Form["trade_status"];

                        if (HttpContext.Current.Request.Form["trade_status"] == "TRADE_FINISHED")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                        }
                        else if (HttpContext.Current.Request.Form["trade_status"] == "TRADE_SUCCESS")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //付款完成后，支付宝系统发送该交易状态通知
                        }
                        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
                        HttpContext.Current.Response.Write("success");  //请不要修改或删除
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("sign fail!");
                    }
                }
                else
                {
                    HttpContext.Current.Response.Write("response fail!");
                }
            }
            else
            {
                HttpContext.Current.Response.Write("非通知参数!");
            }
        }
        [HttpPost, DontWrapResult]
        public void return_url()
        {
            Notify aliNotify = new Notify();

            //获取待验签数据
            Dictionary<string, string> sPara = GetRequestPost();

            //获取同步返回中的success.
            string success = HttpContext.Current.Request.Form["success"];

            //获取同步返回中的sign.
            string sign = HttpContext.Current.Request.Form["sign"].Replace("\"", "");

            //注意：在客户端把返回参数请求过来的时候务必要把sign做一次urlencode,保证特殊字符不会被转义。
            if (success == "\"true\"")//判断success是否为true.
            {
                //判断配置是否匹配.
                if (HttpContext.Current.Request.Form["partner"].Replace("\"", "") == Config.partner && HttpContext.Current.Request.Form["service"].Replace("\"", "") == Config.service)
                {
                    //除去数组中的空值和签名参数,且把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
                    string data = Core.CreateLinkString(Core.FilterPara(sPara));

                    //Core.LogResult(data);//调试用，判断待验签参数是否和客户端一致。
                    //Core.LogResult(sign);//调试用，判断sign值是否和客户端请求时的一致，                
                    bool issign = false;

                    //获得验签结果
                    issign = RSA.verify(data, sign, Notify.getPublicKeyStr(Config.alipay_public_key), Config.input_charset);
                    if (issign)
                    {
                        //此处可做商家业务逻辑，建议商家以异步通知为准。
                        HttpContext.Current.Response.Write("return success!");
                    }
                    else
                    {
                        HttpContext.Current.Response.Write("return fail!");
                    }
                }
            }
        }
        [HttpPost]
        public AlipayOutput signatures(SignaturesInput input)
        {
            //post接收客户端发来的订单信息.
            Dictionary<string, string> sPara = new Dictionary<string, string>();
            sPara.Add("out_trade_no", input.out_trade_no);
            sPara.Add("total_fee", input.total_fee);
            sPara.Add("subject", input.subject);
            sPara.Add("body", input.body);
            sPara.Add("service", Config.service);
            sPara.Add("partner", Config.partner);
            sPara.Add("_input_charset", Config.input_charset);
            sPara.Add("notify_url", "http://121.40.34.43/asbicycle/Alipay/Notify");
            sPara.Add("payment_type", Config.payment_type);
            sPara.Add("seller_id", Config.seller_id);

            //获取订单信息partner.
            //string partner = HttpContext.Current.Request.Form["partner"];

            //获取订单信息service.
            //string service = HttpContext.Current.Request.Form["service"];

            //判断partner和service信息匹配成功.
            //if (partner != null && service != null)
            //{
            //    if (partner.Replace("\"", "") == Config.partner && service.Replace("\"", "") == Config.service)
            //    {
                    //将获取的订单信息，按照“参数=参数值”的模式用“&”字符拼接成字符串.
                    string data = Core.CreateLinkString(sPara);
                    //调试用，打印日志信息，默认在项目路径下的log文件夹.
                    Core.LogResult(data);

                    //使用商户的私钥进行RSA签名，并且把sign做一次urleccode.
                    string sign = HttpUtility.UrlEncode(RSA.sign(data, Config.private_key, Config.input_charset));

                    //拼接请求字符串（注意：不要忘记参数值的引号）.
                    data = data + "&sign=\"" + sign + "\"&sign_type=\"" + Config.sign_type + "\"";

                    //返回给客户端请求.
                    //HttpContext.Current.Response.Write(data);
                    return new AlipayOutput {data = data};

            //    }
            //    else
            //    {
            //        HttpContext.Current.Response.Write("订单信息不匹配!");
            //    }
            //}
            //else
            //{
            //    HttpContext.Current.Response.Write("无客户端请求!");
            //}
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
            coll = HttpContext.Current.Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArraytemp.Add(requestItem[i], HttpContext.Current.Request.Form[requestItem[i]]);
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