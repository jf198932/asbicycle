using System.Reflection;
using Abp.Modules;

namespace ASBicycle.Rental
{
    [DependsOn(typeof(ASBicycleCoreModule))]
    public class ASBicycleApplicationRentalModulel : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
