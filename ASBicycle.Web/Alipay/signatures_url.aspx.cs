using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASBicycle.Rental.AliPay.App;

/// <summary>
/// 功能：页面跳转同步通知页面
/// 版本：1.0
/// 日期：2016-06-06
/// 说明：
/// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
/// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
/// 
/// ///////////////////////页面功能说明///////////////////////
/// 本页面代码示例用于处理客户端使用http(s) post传输到此服务端的移动支付请求参数待签名字符串。
/// 本页面代码示例采用客户端创建订单待签名的请求字符串传输到服务端的这里进行签名操作并返回。
/// </summary>
public partial class signatures : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //post接收客户端发来的订单信息.
        Dictionary<string, string> sPara = GetRequestPost();

        //获取订单信息partner.
        string partner = Request.Form["partner"];

        //获取订单信息service.
        string service = Request.Form["service"];

        //判断partner和service信息匹配成功.
        if (partner != null && service != null)
        {
            if (partner.Replace("\"", "") == Config.partner && service.Replace("\"", "") == Config.service)
            {
                //将获取的订单信息，按照“参数=参数值”的模式用“&”字符拼接成字符串.
                string data = Core.CreateLinkString(sPara);

                //调试用，打印日志信息，默认在项目路径下的log文件夹.
                Core.LogResult(data);

                //使用商户的私钥进行RSA签名，并且把sign做一次urleccode.
                string sign = HttpUtility.UrlEncode(RSA.sign(data, Config.private_key, Config.input_charset));

                //拼接请求字符串（注意：不要忘记参数值的引号）.
                data = data + "&sign=\"" + sign + "\"&sign_type=\"" + Config.sign_type + "\"";

                //返回给客户端请求.
                Response.Write(data);

            }
            else
            {
                Response.Write("订单信息不匹配!");
            }
        }
        else
        {
            Response.Write("无客户端请求!");
        }
    }
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