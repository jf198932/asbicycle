using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASBicycle.VersionUpdate.Dto;
using System.Web.Http;

namespace ASBicycle.VersionUpdate
{
    public class VersionUpdateAppService : ASBicycleAppServiceBase, IVersionUpdateAppService
    {
        public async Task<VersionUpdateOutput> UpdateApp()
        {
            //todo 逻辑
            VersionUpdateOutput aa = new VersionUpdateOutput
            {
                versionCode = 2,
                versionName = "0.0.2",
                upgrade = 2,
                versionUrl = "http://121.40.34.43/ASBicycle/Uploads/isr_bms_0.0.2.apk"
            };
            return aa;
        }
    }
}
