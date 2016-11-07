using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class CanRentalOutput : IOutputDto
    {
         public string charge { get; set; }
    }
}