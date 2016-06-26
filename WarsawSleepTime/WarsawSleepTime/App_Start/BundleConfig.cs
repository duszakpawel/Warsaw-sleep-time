using System.Web.Optimization;

namespace WarsawSleepTime
{
    /// <summary>
    /// Bundle configuration class.
    /// </summary>
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js", "~/Scripts/bootstrap-datetimepicker.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css", "~/Content/bootstrap-datetimepicker.min.css",
                      "~/Content/Site.css", "~/Content/chosen.css"));

            bundles.Add(new ScriptBundle("~/bundles/chosen").Include(
                        "~/Scripts/chosen.jquery.js*"));

            bundles.Add(new ScriptBundle("~/bundles/contact").Include("~/Scripts/contact.js"));
            bundles.Add(new ScriptBundle("~/bundles/search").Include("~/Scripts/search.js"));

        }
    }
}
