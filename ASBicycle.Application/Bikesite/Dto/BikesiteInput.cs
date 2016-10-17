using Abp.Application.Services.Dto;

namespace ASBicycle.Bikesite.Dto
{
    public class BikesiteInput : IInputDto
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class BikesitePageInput : IInputDto
    {
        public BikesitePageInput()
        {
            index = 1;
            pagesize = 15;
        }
        public int id { get; set; }
        public int index { get; set; }
        public int pagesize { get; set; }
    }
}