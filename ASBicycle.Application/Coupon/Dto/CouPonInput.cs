using Abp.Application.Services.Dto;

namespace ASBicycle.Coupon.Dto
{
    public class CouPonInput : IInputDto
    {
        public int user_id { get; set; }

        public string code { get; set; }
    }
}