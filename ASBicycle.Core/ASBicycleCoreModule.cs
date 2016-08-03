using System.Reflection;
using Abp.Modules;

namespace ASBicycle
{
    public class ASBicycleCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
