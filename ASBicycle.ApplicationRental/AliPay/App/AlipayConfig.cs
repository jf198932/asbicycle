using System.Web;

namespace ASBicycle.Rental.AliPay.App
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：1.0
    /// 修改日期：2016-06-06
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// </summary>
    public class Config
    {

        //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        // 合作身份者ID，签约账号，以2088开头由16位纯数字组成的字符串，查看地址：https://openhome.alipay.com/platform/keyManage.htm?keyType=partner
        public static string partner = "2088421507099722";

        // 收款支付宝账号，以2088开头由16位纯数字组成的字符串，一般情况下收款账号就是签约账号
        public static string seller_id = partner;
		
		//商户的私钥,原始格式，RSA公私钥生成：https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.nBDxfy&treeId=58&articleId=103242&docType=1
        public static string private_key =
            "MIICXQIBAAKBgQCovZaBaOVZpuADX2eP3tlDhq6SdgnOOqgjyTIBeOdc/8FjUFU/C+7VCVzH4xUn2dnNTG7+OWvvr96j2UoK1abC1h15V8tg6mLHQtG1KIki2ndU3I6um9E1o1neNSOrUUe0+hGhB6YAoTa0R6ixZdpjGE+bI06Q0qxtgO5jJkPanwIDAQABAoGAVSDomDr6UiNtKmZsUdnklDuTQNKKGd/fycYME1ASsQlaYCc8CoWTpSiHBVTb7HHsfrVL3bfMa2y3jsPlpdepvBmGfKF5JVHdz2HFZgzlYEzkrBH7tFlHnpzmDf3grbtSNV7UJoi6Hcy7gcKviKbH/8D9iu5T0mxGgOiDOF0Ya5kCQQDgO3RzwAhqGAwo0rKGjhy1rWqzK+yM5He0gYXnd9dnMjPFpZfD6B40uc+//oL8LpQ8a4j5tW1gqIErNI1cpIRjAkEAwKWJ920fkXrbr7xYoTH7F7gXUAQtX6DPJiGXtazKedZRcmoe9ABcKjcElWfscRVsYOD7cDrZQM942dkUB4sPlQJAZANAwqwBVMjbC45GoGtcdNAfikDqJkF0/ubSgdZbFiU3IE3mrjOm3V+PQRRU+gQQjA5urun0GiuuSUYMUyjx/wJBAJtJ1TjZgcTnYAb8sATgISMxhbk+ZMTc/54hHgWYT25+0BCGcoUFdUWiK9Ozfeh5+G7vbD8/cLjJhQU18utRsTECQQCsU8FUcACYh8aDWiDwNVO/fMLx5dSlZHX0RmVwc0ubBC2SBdVmR6/zHaMaxGiOp2hiUA1BLVVTBITZcWH52lNe";

        //支付宝的公钥，查看地址：https://b.alipay.com/order/pidAndKey.htm 
        public static string alipay_public_key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCovZaBaOVZpuADX2eP3tlDhq6SdgnOOqgjyTIBeOdc/8FjUFU/C+7VCVzH4xUn2dnNTG7+OWvvr96j2UoK1abC1h15V8tg6mLHQtG1KIki2ndU3I6um9E1o1neNSOrUUe0+hGhB6YAoTa0R6ixZdpjGE+bI06Q0qxtgO5jJkPanwIDAQAB";
        
        // 签名方式
        public static string sign_type = "RSA";

        // 调试用，创建TXT日志文件夹路径，见AlipayCore.cs类中的LogResult(string sWord)打印方法。
        public static string log_path = HttpRuntime.AppDomainAppPath.ToString() + "Logs";

        // 字符编码格式 目前支持 gbk 或 utf-8
        public static string input_charset = "utf-8";

        // 支付类型 ，无需修改
        public static string payment_type = "1";

        // 调用的接口名，无需修改
        public static string service = "mobile.securitypay.pay";

        //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑

    }
}