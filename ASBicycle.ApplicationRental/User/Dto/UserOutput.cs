using Abp.Application.Services.Dto;

namespace ASBicycle.Rental.User.Dto
{
    public class UserOutput : IOutputDto
    {
        public UserDto UserDto { get; set; }
    }
}