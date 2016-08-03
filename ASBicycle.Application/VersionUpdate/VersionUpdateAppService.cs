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
                versionCode = 1,
                versionName = "0.0.1",
                mustUpdate = 1,
                versionUrl = ""
            };
            return aa;
        }
    }
}
