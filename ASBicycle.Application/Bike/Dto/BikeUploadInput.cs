using System.Web;
using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class BikeUploadInput : IInputDto
    {
        public string BikeId { get; set; }
    }
}