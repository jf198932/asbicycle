using System.Reflection;
using Abp.Application.Services;
using Abp.Modules;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;

namespace ASBicycle
{
    [DependsOn(typeof(AbpWebApiModule), typeof(ASBicycleApplicationModule))]
    public class ASBicycleWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(ASBicycleApplicationModule).Assembly, "v2_0_0")
                .WithConventionalVerbs()//根据方法名使用惯例HTTP动词，默认对于所有的action使用Post
                .Build();

            //DynamicApiControllerBuilder
            //    .ForAll<IApplicationService>(typeof(ASBicycleApplicationRentalModulel).Assembly, "rental")
            //    .WithConventionalVerbs()//根据方法名使用惯例HTTP动词，默认对于所有的action使用Post
            //    .Build();
        }

        
    }
}
