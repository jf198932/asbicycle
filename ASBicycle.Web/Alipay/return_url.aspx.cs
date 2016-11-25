using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using ASBicycle.AliPay.App;

/// <summary>
/// 功能：页面跳转同步通知页面
/// 版本：1.0
/// 日期：2016-06-06
/// 说明：
/// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
/// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
/// 
/// ///////////////////////页面功能说明///////////////////////
/// 本页面代码示例用于处理客户端使用http(s) post传输到此服务端的移动支付同步返回中的result待验签字段.
/// 注意：只要把同步返回中的result结果传输过来做验签.
/// </summary>
public partial class return_url : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Notify aliNotify = new Notify();

        ////获取待验签数据
        //Dictionary<string, string> sPara = GetRequestPost();

        ////获取同步返回中的success.
        //string success = Request.Form["success"];

        ////获取同步返回中的sign.
        //string sign = Request.Form["sign"].Replace("\"", "");

        ////注意：在客户端把返回参数请求过来的时候务必要把sign做一次urlencode,保证特殊字符不会被转义。
        //if (success == "\"true\"")//判断success是否为true.
        //{
        //    //判断配置是否匹配.
        //    if (Request.Form["partner"].Replace("\"", "") == Config.partner && Request.Form["service"].Replace("\"", "") == Config.service)
        //    {
        //        //除去数组中的空值和签名参数,且把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串
        //        string data = Core.CreateLinkString(Core.FilterPara(sPara));

        //        //Core.LogResult(data);//调试用，判断待验签参数是否和客户端一致。
        //        //Core.LogResult(sign);//调试用，判断sign值是否和客户端请求时的一致，                
        //        bool issign = false;

        //        //获得验签结果
        //        issign = RSA.verify(data, sign, Notify.getPublicKeyStr(Config.alipay_public_key), Config.input_charset);
        //        if (issign)
        //        {
        //            //此处可做商家业务逻辑，建议商家以异步通知为准。
        //            Response.Write("return success!");
        //        }
        //        else
        //        {
        //            Response.Write("return fail!");
        //        }
        //    }
        //}
    }

    public Dictionary<string, string> GetRequestPost()
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
        Dictionary<string, string> sArray = new Dictionary<string, string>();
        foreach (KeyValuePair<string, string> temp in sArraytemp)
        {
            sArray.Add(temp.Key, temp.Value);
        }
        return sArray;
    }
}
