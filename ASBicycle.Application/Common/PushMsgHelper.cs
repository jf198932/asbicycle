using System;
using System.IO;
using System.Net;
using System.Text;

namespace ASBicycle.Common
{
    public static class PushMsgHelper
    {
        public static string PushToWeb(string weburl, string data, Encoding encode)
        {
            byte[] byteArray = encode.GetBytes(data);

            HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(new Uri(weburl));
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;

            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();

            //接受返回信息
            HttpWebResponse response = (HttpWebResponse) webRequest.GetResponse();
            StreamReader result = new StreamReader(response.GetResponseStream(), encode);
            return result.ReadToEnd();
        } 
    }
}