using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace ASBicycle.User.Dto
{
    public class CheckLoginInput : IInputDto
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string CheckCode { get; set; }
        public int? device_os { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public string mobile_brand { get; set; }
        /// <summary>
        /// 机型
        /// </summary>
        public string mobile_model { get; set; }
        /// <summary>
        /// 系统版本
        /// </summary>
        public string os_version { get; set; }
        /// <summary>
        /// app版本号
        /// </summary>
        public string app_version { get; set; }
        /// <summary>
        /// 设备串号
        /// </summary>
        public string device_id { get; set; }
    }

    public class CheckIdentityInput : IInputDto
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Token { get; set; }
        public int? device_os { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string mobile_brand { get; set; }
        /// <summary>
        /// 机型
        /// </summary>
        public string mobile_model { get; set; }
        /// <summary>
        /// 系统版本
        /// </summary>
        public string os_version { get; set; }
        /// <summary>
        /// app版本号
        /// </summary>
        public string app_version { get; set; }
        /// <summary>
        /// 设备串号
        /// </summary>
        public string device_id { get; set; }
    }

    public class RegisterUserInput : IInputDto
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string CheckCode { get; set; }
        public int? device_os { get; set; }
        /// <summary>
        /// 设备串号
        /// </summary>
        public string device_id { get; set; }

    }
}