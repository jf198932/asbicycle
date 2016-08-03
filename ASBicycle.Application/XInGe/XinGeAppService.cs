using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PushNotifications;
using PushNotifications.Schema;

namespace ASBicycle.XInGe
{
    public class XinGeAppService : ASBicycleAppServiceBase, IXinGeAppService
    {
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

        public void SendIos(string account)
        {
            throw new System.NotImplementedException();
        }
    }
}