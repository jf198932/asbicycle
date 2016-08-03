using System.Web.Optimization;

namespace ASBicycle.Web
{
    public class BundleConfig
    {
        //// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        //public static void RegisterBundles(BundleCollection bundles)
        //{
        //    // Set EnableOptimizations to false for debugging. For more information,
        //    // visit http://go.microsoft.com/fwlink/?LinkId=301862

        //    //TODO:
        //    BundleTable.EnableOptimizations = false;

        //    //公用
        //    bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
        //                "~/Scripts/jquery-{version}.js"));

        //    bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
        //                "~/Scripts/jquery.validate*"));

        //    // Use the development version of Modernizr to develop with and learn from. Then, when you're
        //    // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        //    bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
        //                "~/Scripts/modernizr-*"));

        //    bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
        //              "~/Scripts/bootstrap.js",
        //              "~/Scripts/respond.js"));

        //    bundles.Add(new StyleBundle("~/Content/css").Include(
        //              "~/Content/bootstrap.css",
        //              "~/Content/global.css",
        //              "~/Content/site.css",
        //              "~/Content/site.response.css"
        //              ));

        //    //后台管理系统

        //    //CSS 样式
        //    bundles.Add(new StyleBundle("~/bundles/admin/global/css").Include(
        //              "~/metronic/assets/global/plugins/font-awesome/css/font-awesome.min.css",
        //              "~/metronic/assets/global/plugins/bootstrap/css/bootstrap.min.css",
        //              "~/metronic/assets/global/plugins/uniform/css/uniform.default.css",
        //              "~/metronic/assets/global/plugins/simple-line-icons/simple-line-icons.min.css",
        //              "~/metronic/assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css"
        //              //"~/metronic/assets/global/plugins/select2/css/select2-bootstrap.min.css"

        //              //"~/metronic/assets/global/css/style.css",
        //              //"~/metronic/assets/global/css/style-responsive.css",
        //              //"~/metronic/assets/global/css/admin.main.css",
        //              //"~/metronic/assets/global/css/admin.main-responsive.css",
        //              //"~/metronic/assets/global/css/plugins.css",
        //              //"~/metronic/assets/global/css/pages/tasks.css",
        //              //"~/metronic/assets/global/css/custom.css"

        //              ));

        //    bundles.Add(new StyleBundle("~/bundles/admin/grid/css").Include(
        //              "~/metronic/assets/global/plugins/select2/css/select2.min.css",
        //              "~/metronic/assets/global/plugins/datatables/datatables.min.css",
        //              "~/metronic/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.css",
        //              "~/metronic/assets/global/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.min.css",

        //              "~/metronic/assets/global/css/plugins.min.css",
        //              "~/metronic/assets/layouts/layout/css/layout.min.css",
        //              "~/metronic/assets/layouts/layout/css/themes/darkblue.min.css",
        //              "~/metronic/assets/layouts/layout/css/custom.min.css"
        //              ));

        //    //JS 脚本
        //    bundles.Add(new ScriptBundle("~/bundles/admin/core/js").Include(
        //                "~/metronic/assets/global/plugins/jquery.min.js",
        //                "~/metronic/assets/global/plugins/bootstrap/js/bootstrap.min.js",
        //                "~/metronic/assets/global/plugins/js.cookie.min.js",
        //                "~/metronic/assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
        //                "~/metronic/assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
        //                "~/metronic/assets/global/plugins/jquery.blockui.min.js",
        //                "~/metronic/assets/global/plugins/uniform/jquery.uniform.min.js",
        //                "~/metronic/assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js"
        //                //"~/metronic/assets/global/scripts/admin.main.js"
        //                ));

        //    bundles.Add(new ScriptBundle("~/bundles/admin/grid/js").Include(
        //               "~/metronic/assets/global/plugins/select2/js/select2.min.js",
        //               "~/metronic/assets/global/scripts/datatable.js",
        //               "~/metronic/assets/global/plugins/datatables/datatables.min.js",
        //               "~/metronic/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js",
        //               "~/metronic/assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js"
        //               ));

        //    bundles.Add(new ScriptBundle("~/bundles/admin/end/js").Include(
        //               "~/metronic/assets/layouts/layout/scripts/layout.min.js",
        //                "~/metronic/assets/layouts/layout/scripts/demo.min.js",
        //                "~/metronic/assets/layouts/global/scripts/quick-sidebar.min.js"
        //               ));

        //    //前台网站

        //}

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862

            //TODO:
            BundleTable.EnableOptimizations = false;

            //公用
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Content/assets/plugins/jquery-migrate-1.2.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/global.css",
                      "~/Content/site.css",
                      "~/Content/site.response.css"
                      ));

            //后台管理系统

            //CSS 样式
            bundles.Add(new StyleBundle("~/bundles/admin/css").Include(
                      //全局样式
                      "~/Content/assets/plugins/font-awesome/css/font-awesome.min.css",
                      "~/Content/assets/plugins/bootstrap/css/bootstrap.min.css",
                      //表单美化
                      "~/Content/assets/plugins/uniform/css/uniform.default.css",
                      //主题样式
                      "~/Content/assets/css/style-metronic.css",
                      "~/Content/assets/css/style.css",
                      "~/Content/assets/css/style-responsive.css",
                      "~/Content/assets/css/admin.main.css",
                      "~/Content/assets/css/admin.main-responsive.css",
                      "~/Content/assets/css/plugins.css",
                      "~/Content/assets/css/pages/tasks.css",
                      "~/Content/assets/css/custom.css"
                      ));

            bundles.Add(new StyleBundle("~/bundles/admin/grid/css").Include(
                      "~/Content/assets/plugins/select2/select2_metro.css",
                      "~/Content/assets/plugins/data-tables/DT_bootstrap.css"
                      ));

            //JS 脚本
            bundles.Add(new ScriptBundle("~/bundles/admin/js").Include(
                        //IMPORTANT! Load jquery-ui-1.10.3.custom.min.js before bootstrap.min.js to fix bootstrap tooltip conflict with jquery ui tooltip
                        "~/Content/assets/plugins/jquery-ui/jquery-ui-1.10.3.custom.min.js",
                        "~/Content/assets/plugins/bootstrap/js/bootstrap.min.js",
                        "~/Content/assets/plugins/bootstrap-hover-dropdown/twitter-bootstrap-hover-dropdown.min.js",
                        "~/Content/assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                        "~/Content/assets/plugins/jquery.blockui.min.js",
                        "~/Content/assets/plugins/jquery.cokie.min.js",
                        "~/Content/assets/plugins/uniform/jquery.uniform.min.js",
                        "~/Content/assets/scripts/app.js",
                        "~/Content/assets/scripts/admin.main.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/admin/grid/js").Include(
                       "~/Content/assets/plugins/select2/select2.min.js",
                       "~/Content/assets/plugins/data-tables/jquery.dataTables.js",
                       "~/Content/assets/plugins/data-tables/jquery.dataTables.AjaxSource.min.js",
                       "~/Content/assets/plugins/data-tables/DT_bootstrap.js",
                       "~/Content/assets/scripts/table-managed.js"
                       ));



            //前台网站

        }
    }
}