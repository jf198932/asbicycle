using Abp.Application.Services.Dto;

namespace ASBicycle.User.Dto
{
    public class UserDto : EntityDto
    {
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Weixacc { get; set; }
        public string Email { get; set; }
        public int? Certification { get; set; }
        public string Remember_token { get; set; }
        public int? Credits { get; set; }
        public int? Balance { get; set; }
        public string Img { get; set; }
        public int? School_id { get; set; }
        public int? User_type { get; set; }
        public int? Device_os { get; set; }
        public string Device_id { get; set; }
    }
}