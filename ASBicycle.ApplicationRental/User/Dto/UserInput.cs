using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace ASBicycle.Rental.User.Dto
{
    public class UserInput : IInputDto
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Weixacc { get; set; }
        [RegularExpression("/^(\\w-*\\.*)+@(\\w-?)+(\\.\\w{2,})+$/", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }

        public string Img { get; set; }
        //public int? Certification { get; set; }
        public int? School_id { get; set; }

        //public string Token { get; set; }

    }
}