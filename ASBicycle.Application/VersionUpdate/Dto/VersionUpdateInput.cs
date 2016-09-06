using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace ASBicycle.VersionUpdate.Dto
{
    public class VersionUpdateInput : IInputDto
    {
        public int device_os { get; set; }
    }
}
