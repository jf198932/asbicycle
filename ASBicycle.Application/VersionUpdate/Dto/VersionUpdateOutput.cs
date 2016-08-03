using Abp.Application.Services.Dto;

namespace ASBicycle.VersionUpdate.Dto
{
    public class VersionUpdateOutput : IOutputDto
    {
        //版本号
        public int versionCode { get; set; }
        //版本名称
        public string versionName { get; set; }

        public int mustUpdate { get; set; }

        public string versionUrl { get; set; }
    }
}
