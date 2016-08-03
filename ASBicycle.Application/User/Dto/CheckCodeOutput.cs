using Abp.Application.Services.Dto;

namespace ASBicycle.User.Dto
{
    public class CheckCodeOutput : IOutputDto
    {
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string CheckCode { get; set; }
    }
}