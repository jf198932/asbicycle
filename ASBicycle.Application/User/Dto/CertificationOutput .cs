using Abp.Application.Services.Dto;

namespace ASBicycle.User.Dto
{
    public class CertificationOutput : IOutputDto
    {
        /// <summary>
        /// 押金状态 1:未完成  2：完成
        /// </summary>
        public int deposit_status { get; set; }
        /// <summary>
        /// 身份证录入状态 1:未完成  2：完成
        /// </summary>
        public int identity_status { get; set; }
        /// <summary>
        /// 完成 1:未完成  2：完成
        /// </summary>
        public int success_status { get; set; }

        public double deposit { get; set; }
    }
}