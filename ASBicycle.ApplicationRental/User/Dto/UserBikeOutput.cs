using Abp.Application.Services.Dto;
using ASBicycle.Rental.Bike.Dto;

namespace ASBicycle.Rental.User.Dto
{
    public class UserBikeOutput : IOutputDto
    {
        public bool IsBindBike { get; set; }

        public BikeDto Bike { get; set; }
    }
}