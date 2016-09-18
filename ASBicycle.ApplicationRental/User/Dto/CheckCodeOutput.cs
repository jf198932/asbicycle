using Abp.Application.Services.Dto;

namespace ASBicycle.Rental.User.Dto
{
    public class CheckCodeOutput : IOutputDto
    {
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string CheckCode { get; set; }
    }
}