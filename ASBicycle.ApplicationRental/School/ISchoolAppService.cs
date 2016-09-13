using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Application.Services;
using ASBicycle.Rental.School.Dto;

namespace ASBicycle.Rental.School
{
    public interface ISchoolAppService : IApplicationService
    {
        [HttpGet]
        Task<List<SchoolOutput>> GetSchoolList();
        [HttpGet]
        Task<List<SchoolBikeSiteOutput>> GetSchoolBikeSiteList([FromUri]int id);
    }
}