using System;
using System.Collections.Generic;
using Abp.EntityFramework;
using ASBicycle.Entities;
using ASBicycle.School;
using System.Linq;

namespace ASBicycle.EntityFramework.Repositories.School
{
    public class SchoolRepository : ASBicycleRepositoryBase<Entities.School, int>, ISchoolRepository
    {
        public SchoolRepository(IDbContextProvider<ASBicycleDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public List<Entities.School> GetAllWithPeople(int? schoolid)
        {
            var query = GetAll();
            if (schoolid.HasValue)
            {
                query = query.Where(s => s.Id == schoolid.Value);
            }

            return query.OrderBy(s => s.Name).ToList();
        }
    }
}