﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.AutoMapper;
using Abp.UI;
using ASBicycle.Bikesite;
using ASBicycle.School.Dto;

namespace ASBicycle.School
{
    public class SchoolAppService : ASBicycleAppServiceBase, ISchoolAppService
    {
        private readonly IBikesiteReadRepository _bikesiteReadRepository;
        private readonly IBikesiteWriteRepository _bikesiteRepository;
        private readonly ISqlReadExecuter _sqlReadExecuter;

        public SchoolAppService(IBikesiteReadRepository bikesiteReadRepository
            , IBikesiteWriteRepository bikesiteRepository
            , ISqlReadExecuter sqlReadExecuter)
        {
            _bikesiteReadRepository = bikesiteReadRepository;
            _bikesiteRepository = bikesiteRepository;
            _sqlReadExecuter = sqlReadExecuter;
        }

        public async Task<List<SchoolBikeSiteOutput>> GetSchoolBikeSiteList([FromUri] SchoolInput input)
        {
            var bikesites = await _bikesiteReadRepository.GetAllListAsync(b => b.School_id == input.id && b.Enable);
            return bikesites.Select(item => new SchoolBikeSiteOutput
            {
                Id = item.Id,
                School_id = input.id,
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
            StringBuilder sb = new StringBuilder();
            //sb.Append(
            //    "select s.id,s.`name`,s.areacode,s.gps_point,COUNT(DISTINCT a.id) as bike_count,s.time_charge,s.refresh_date,COUNT(DISTINCT b.id) as site_count");
            //sb.Append(" from school as s LEFT JOIN bikesite as b on s.id = b.school_id");
            //sb.Append(" LEFT JOIN bike as a on b.id = a.bikesite_id and a.bike_status = 1 and a.rent_type = 1 and a.ble_type = 4");
            //sb.Append(" WHERE s.gps_point is not null");
            //sb.Append(" GROUP BY s.id,s.`name`,s.areacode,s.gps_point,s.bike_count,s.time_charge,s.refresh_date");

            sb.Append(
                "select s.id,s.`name`,s.areacode,s.gps_point,SUM(DISTINCT b.available_count) as bike_count,s.time_charge,s.refresh_date,COUNT(DISTINCT b.id) as site_count");
            sb.Append(" from school as s LEFT JOIN bikesite as b on s.id = b.school_id and b.`enable` = true");
            sb.Append(" WHERE s.gps_point is not null");
            sb.Append(" GROUP BY s.id,s.`name`,s.areacode,s.gps_point,s.bike_count,s.time_charge,s.refresh_date");

            var result = _sqlReadExecuter.SqlQuery<SchoolOutput>(sb.ToString());

            //var school = _schoolRepository.GetAll().Where(t=> !string.IsNullOrEmpty(t.Gps_point) && t.TenancyName.ToLower() != "default").ToList();
            //if (school == null)
            //{
            //    throw new UserFriendlyException("没有学校");
            //}
            //Mapper.CreateMap<Entities.School, SchoolOutput>();
            //return new List<SchoolOutput>(school.MapTo<List<SchoolOutput>>());
            if (result == null || !result.Any())
            {
                throw new UserFriendlyException("没有学校");
            }
            return result.ToList();
        }
    }
}