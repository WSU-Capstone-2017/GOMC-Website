using System.Web.Optimization;

namespace Project
{
	public class BundleConfig
	{
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js",
                        "~/Scripts/js.cookie.js",
                        "~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.min.js",
					  "~/Scripts/respond.js"));

            // JS bundle of all developed content
            bundles.Add(new ScriptBundle("~/bundles/alljs").Include(
                "~/Scripts/gomc-new.js",
                "~/Scripts/admin.js",
                "~/Scripts/downloads.js",
                "~/Scripts/latex.js",
                "~/Scripts/login.js",
                "~/Scripts/xml.js"
                ));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
					  "~/Content/bootstrap.css",
                      "~/Content/bootstrap.theme.css"));
            // CSS bundle of all related styles
			bundles.Add(
				new StyleBundle("~/Content/allcss").Include(
                    "~/Content/gomc.css",
                    "~/Content/about.css",
                    "~/Content/admin.css",
                    "~/Content/docs.css",
                    "~/Content/downloads.css",
                    "~/Content/faq.css",
                    "~/Content/feat.css",
                    "~/Content/latex-html.css",
                    "~/Content/latex.css",
                    "~/Content/publications.css",
                    "~/Content/site.css",
                    "~/Content/xml.css"
                    ));
            // Set to true to enable full bundle optimization
            BundleTable.EnableOptimizations = true;
        }
	}
}
