using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Abp.Domain.Repositories;
using ASBicycle.Rental.VersionUpdate.Dto;

namespace ASBicycle.Rental.VersionUpdate
{
    public class VersionUpdateAppService : ASBicycleAppServiceBase, IVersionUpdateAppService
    {
        private readonly IRepository<Entities.VersionUpdate> _versionUpdateRepository;

        public VersionUpdateAppService(IRepository<Entities.VersionUpdate> versionUpdateRepository)
        {
            _versionUpdateRepository = versionUpdateRepository;
        }

        public async Task<VersionUpdateOutput> UpdateApp([FromUri] VersionUpdateInput input)
        {
            var model = _versionUpdateRepository.GetAll().Where(t => t.device_os == input.device_os).OrderByDescending(t => t.versionCode)
                .Select(t=> new VersionUpdateOutput
                {
                    device_os = t.device_os,
                    versionCode = t.versionCode,
                    versionName = t.versionName,
                    upgrade = t.upgrade,
                    versionUrl = t.versionUrl
                }).FirstOrDefault();


            return model;
        }
    }
}
