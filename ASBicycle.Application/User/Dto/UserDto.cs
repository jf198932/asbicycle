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
        /// <summary>
        /// 身份类型 0：非校园用户  1：在校学生  2：教职工
        /// </summary>
        public int? User_type { get; set; }
        public int? Device_os { get; set; }
        public string Device_id { get; set; }
        public string Headimg { get; set; }

        public string Id_no { get; set; }

        public int Payed { get; set; }
        public string School_name { get; set; }
        public string User_type_name { get; set; }

        public bool IsBindBike { get; set; }

        public double Deposit { get; set; }
        /// <summary>
        /// 1:退款中 0 正常
        /// </summary>
        public int Refound_status { get; set; }
    }
}