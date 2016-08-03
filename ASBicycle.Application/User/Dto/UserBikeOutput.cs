using Abp.Application.Services.Dto;
using ASBicycle.Bike.Dto;

namespace ASBicycle.User.Dto
{
    public class UserBikeOutput : IOutputDto
    {
        public bool IsBindBike { get; set; }

        public BikeDto Bike { get; set; }
    }
}