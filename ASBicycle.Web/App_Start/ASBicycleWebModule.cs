using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.IO;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Hangfire;
using Abp.Web.SignalR;

namespace ASBicycle.Web
{
    [DependsOn(
        typeof(AbpWebMvcModule),
        typeof(ASBicycleDataModule), 
        typeof(ASBicycleApplicationModule), 
        typeof(ASBicycleWebApiModule),
        typeof(AbpWebSignalRModule),
        typeof(AbpHangfireModule)
        )]
    public class ASBicycleWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Add/remove languages for your application
            Configuration.Localization.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flag-england", true));
            Configuration.Localization.Languages.Add(new LanguageInfo("zh-CN", "简体中文", "famfamfam-flag-cn"));

            //Add/remove localization sources here
            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    ASBicycleConsts.LocalizationSourceName,
                    new XmlFileLocalizationDictionaryProvider(
                        HttpContext.Current.Server.MapPath("~/Localization/ASBicycle")
                        )
                    )
                );

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<ASBicycleNavigationProvider>();

            //Configure to use Hangfire as background job manager. Remove these lines to use default background job manager, instead of Hangfire.
            //Configuration.BackgroundJobs.UseHangfire(configuration =>
            //{
            //    //configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            //    configuration.GlobalConfiguration.UseMySqlStorage("Default");
            //});
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public override void PostInitialize()
        {
            var server = HttpContext.Current.Server;
            var appFolders = IocManager.Resolve<AppFolders>();

            appFolders.SampleProfileImagesFolder = server.MapPath("~/Common/Images/SampleProfilePics");
            appFolders.TempFileDownloadFolder = server.MapPath("~/Temp/Downloads");

            try { DirectoryHelper.CreateIfNotExists(appFolders.TempFileDownloadFolder); } catch { }
        }
    }
}
