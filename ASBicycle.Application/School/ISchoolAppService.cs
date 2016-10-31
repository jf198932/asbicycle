using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.School.Dto;

namespace ASBicycle.School
{
    public interface ISchoolAppService : IApplicationService
    {
        [HttpGet]
        Task<List<SchoolOutput>> GetSchoolList();

        [HttpGet]
        Task<List<SchoolBikeSiteOutput>> GetSchoolBikeSiteList([FromUri] SchoolInput input);
    }
}