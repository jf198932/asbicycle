using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.AutoMapper;
using Abp.UI;
using ASBicycle.Bikesite;
using ASBicycle.School.Dto;
using AutoMapper;

namespace ASBicycle.School
{
    public class SchoolAppService : ASBicycleAppServiceBase, ISchoolAppService
    {
        private readonly ISchoolRepository _schoolRepository;
        private readonly IBikesiteRepository _bikesiteRepository;

        public SchoolAppService(ISchoolRepository schoolRepository, IBikesiteRepository bikesiteRepository)
        {
            _schoolRepository = schoolRepository;
            _bikesiteRepository = bikesiteRepository;
        }

        public async Task<List<SchoolBikeSiteOutput>> GetSchoolBikeSiteList([FromUri]int id)
        {
            var bikesites = await _bikesiteRepository.GetAllListAsync(b => b.School_id == id);
            return bikesites.Select(item => new SchoolBikeSiteOutput
            {
                Id = item.Id,
                School_id = id,
                Type = item.Type,
                Name = item.Name,
                Description = item.Description,
                Rent_charge = item.Rent_charge,
                Return_charge = item.Return_charge,
                Gps_point = item.Gps_point,
                Bike_count = item.Bike_count,
                Available_count = item.Available_count,
                Radius = item.Radius,
                //Available_bikes = item
                Beacons = item.Sitebeacons.MapTo<List<SitebeaconDto>>()
            }).ToList();
        }

        public async Task<List<SchoolOutput>> GetSchoolList()
        {
            var school = _schoolRepository.GetAll().Where(t=> !string.IsNullOrEmpty(t.Gps_point) && t.TenancyName.ToLower() != "default").ToList();
            if (school == null)
            {
                throw new UserFriendlyException("没有学校");
            }
            Mapper.CreateMap<Entities.School, SchoolOutput>();
            return new List<SchoolOutput>(school.MapTo<List<SchoolOutput>>());
            
        }
    }
}