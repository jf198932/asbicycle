using Abp.Application.Services.Dto;

namespace ASBicycle.Coupon.Dto
{
    public class CouPonInput : IInputDto
    {
        public CouPonInput()
        {
            index = 1;
        }

        public int user_id { get; set; }

        public double pay { get; set; }

        public string code { get; set; }

        public int index { get; set; }
    }
}