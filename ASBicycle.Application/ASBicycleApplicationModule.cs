using System.Reflection;
using Abp.Modules;

namespace ASBicycle
{
    [DependsOn(typeof(ASBicycleCoreModule))]
    public class ASBicycleApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
