using Abp.Application.Services.Dto;

namespace ASBicycle.Rental.Bike.Dto
{
    public class RentalBikeOutput : IOutputDto
    {
         public string out_trade_no { get; set; }
    }
}