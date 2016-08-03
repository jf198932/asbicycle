using Abp.Application.Services.Dto;

namespace ASBicycle.User.Dto
{
    public class UserUploadOutput : IOutputDto
    {
        public string ImgUrl { get; set; }
    }
}