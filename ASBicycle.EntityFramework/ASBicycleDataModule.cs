using System.Data.Entity;
using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using ASBicycle.EntityFramework;

namespace ASBicycle
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(ASBicycleCoreModule))]
    public class ASBicycleDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Database.SetInitializer<ASBicycleDbContext>(null);
            Database.SetInitializer<ReadonlyASBicycleDbContext>(null);
        }
    }
}
