using Abp.Application.Services.Dto;

namespace ASBicycle.Rental.VersionUpdate.Dto
{
    public class VersionUpdateInput : IInputDto
    {
        public int device_os { get; set; }
    }
}
