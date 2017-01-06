using Abp.Application.Services.Dto;

namespace ASBicycle.Bike.Dto
{
    public class BikeUsedOutput : IOutputDto
    {
        public int Count { get; set; }
        public string Usedtime { get; set; }
    }
}