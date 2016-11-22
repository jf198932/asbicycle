using Abp.Application.Services.Dto;

namespace ASBicycle.Recharge.Dto
{
    public class RefoundInput : IInputDto
    {
        public RefoundInput ()
        {
            Recharge_type = 1;
        }
        public int User_id { get; set; }
        public double Amount { get; set; }
        public int Recharge_type { get; set; }
    }
}