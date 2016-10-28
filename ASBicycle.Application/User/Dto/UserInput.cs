using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace ASBicycle.User.Dto
{
    public class UserInput : IInputDto
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Id_no { get; set; }
        public string Weixacc { get; set; }
        [RegularExpression("/^(\\w-*\\.*)+@(\\w-?)+(\\.\\w{2,})+$/", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }
        /// <summary>
        /// 身份类型 0：非校园用户  1：在校学生  2：教职工
        /// </summary>
        public int User_type { get; set; }

        public string Img { get; set; }
        //public int? Certification { get; set; }
        public int? School_id { get; set; }

        //public string Token { get; set; }

    }
}