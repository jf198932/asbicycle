using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace ASBicycle.User.Dto
{
    public class PhoneNumInput : IInputDto
    {
        [Required]
        public string Phone { get; set; }
    }

    public class UserIdInput : IInputDto
    {
        public int User_id { get; set; }
    }
}