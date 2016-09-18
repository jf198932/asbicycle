using Abp.Application.Services.Dto;

namespace ASBicycle.Rental.User.Dto
{
    public class UserBikeInput : IInputDto
    {
        public int User_id { get; set; }

        public string Serial { get; set; }

        public string Bike_img { get; set; }

        public string Token { get; set; }
    }
}