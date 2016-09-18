using Abp.Application.Services.Dto;

namespace ASBicycle.Rental.Bike.Dto
{
    public class BikeUploadInput : IInputDto
    {
        public string BikeId { get; set; }
    }
}