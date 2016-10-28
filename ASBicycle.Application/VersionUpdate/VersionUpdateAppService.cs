using System.Linq;
using System.Threading.Tasks;
using ASBicycle.VersionUpdate.Dto;
using System.Web.Http;

namespace ASBicycle.VersionUpdate
{
    public class VersionUpdateAppService : ASBicycleAppServiceBase, IVersionUpdateAppService
    {
        private readonly IVersionUpdateReadRespository _versionUpdateReadRepository;

        public VersionUpdateAppService(IVersionUpdateReadRespository versionUpdateReadRepository)
        {
            _versionUpdateReadRepository = versionUpdateReadRepository;
        }

        public async Task<VersionUpdateOutput> UpdateApp([FromUri] VersionUpdateInput input)
        {
            var model = _versionUpdateReadRepository.GetAll().Where(t => t.device_os == input.device_os).OrderByDescending(t => t.versionCode)
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
