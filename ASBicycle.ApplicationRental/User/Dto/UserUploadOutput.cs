using Abp.Application.Services.Dto;

namespace ASBicycle.Rental.User.Dto
{
    public class UserUploadOutput : IOutputDto
    {
        public string ImgUrl { get; set; }
    }
}