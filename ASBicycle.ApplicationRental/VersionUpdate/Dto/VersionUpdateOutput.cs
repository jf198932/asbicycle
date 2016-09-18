using Abp.Application.Services.Dto;

namespace ASBicycle.Rental.VersionUpdate.Dto
{
    public class VersionUpdateOutput : IOutputDto
    {
        /// <summary>
        /// 设备
        /// </summary>
        public int device_os { get; set; }
        //版本号
        public int versionCode { get; set; }
        //版本名称
        public string versionName { get; set; }
        /// <summary>
        /// 1.不升级  2 可升级   3 强制升级
        /// </summary>
        public int upgrade { get; set; }

        public string versionUrl { get; set; }
    }
}
