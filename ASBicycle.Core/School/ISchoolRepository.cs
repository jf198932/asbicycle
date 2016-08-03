using System.Collections.Generic;
using Abp.Domain.Repositories;

namespace ASBicycle.School
{
    public interface ISchoolRepository : IRepository<Entities.School, int>
    {
        List<Entities.School> GetAllWithPeople(int? schoolid);
    }
}