using Abp.Application.Services.Dto;

namespace ASBicycle.Rental.Bike.Dto
{
    public class BikeUploadOutput : IOutputDto
    {
         public string ImgUrl { get; set; }
    }
}