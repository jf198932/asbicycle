using Abp.Web.Mvc.Views;

namespace ASBicycle.Web.Views
{
    public abstract class ASBicycleWebViewPageBase : ASBicycleWebViewPageBase<dynamic>
    {

    }

    public abstract class ASBicycleWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected ASBicycleWebViewPageBase()
        {
            LocalizationSourceName = ASBicycleConsts.LocalizationSourceName;
        }
    }
}