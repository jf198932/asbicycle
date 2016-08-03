using Abp.Application.Services.Dto;

namespace ASBicycle.User.Dto
{
    public class UserOutput : IOutputDto
    {
        public UserDto UserDto { get; set; }
    }
}