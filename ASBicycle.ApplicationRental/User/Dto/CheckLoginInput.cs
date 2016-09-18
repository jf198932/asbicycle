using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace ASBicycle.Rental.User.Dto
{
    public class CheckLoginInput : IInputDto
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string CheckCode { get; set; }
        public int? device_os { get; set; }
    }

    public class CheckIdentityInput : IInputDto
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Token { get; set; }
        public int? device_os { get; set; }
    }

    public class RegisterUserInput : IInputDto
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string CheckCode { get; set; }
        public int? device_os { get; set; }
    }
}