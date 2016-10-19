using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Policy;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using Abp.Web.Models;
using ASBicycle.AliPay.App;
using ASBicycle.AliPay.Dto;

namespace ASBicycle.AliPay
{
    public class AlipayAppService : ASBicycleAppServiceBase, IAlipayAppService
    {
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
            sPara.Add("notify_url", "http://api.isriding.com/app/Alipay/Notify");
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
                    //Core.LogResult(data);

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