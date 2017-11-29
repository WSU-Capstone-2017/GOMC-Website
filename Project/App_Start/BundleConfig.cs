using System.Web.Optimization;

namespace Project
{
	public class BundleConfig
	{
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/js.cookie").Include(
				"~/Scripts/js.cookie.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new ScriptBundle("~/bundles/gomc").Include(
				"~/Scripts/gomc.js"));

            // Will uncomment once JS is done
            //bundles.Add(new ScriptBundle("~/bundles/all/js").Include(
            //    // Remove all blank scripts
            //    "~/Scripts/gomc.js",
            //    "~/Scripts/about.js",
            //    "~/Scripts/admin.js",
            //    "~/Scripts/docs.js",
            //    "~/Scripts/downloads.js",
            //    "~/Scripts/faq.js",
            //    "~/Scripts/feat.js",
            //    "~/Scripts/latex-html.js",
            //    "~/Scripts/latex.js",
            //    "~/Scripts/login.js",
            //    "~/Scripts/privacy.js",
            //    "~/Scripts/publications.js",
            //    "~/Scripts/site.js",
            //    "~/Scripts/terms.js",
            //    "~/Scripts/xml.js",
            //    "~/Scripts/more-downloads.js",
            //    "~/Scripts/more-examples.js"
            //    ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css"));

			bundles.Add(
				new StyleBundle("~/Content/bootstrap").Include(
					"~/Content/bootstrap.css",
					"~/Content/bootstrap.theme.css"));

			bundles.Add(
				new StyleBundle("~/Content/all/css").Include(
                    // Remove all blank files
                    // "~/Content/core.css" // old css
                    "~/Content/gomc.css",
                    "~/Content/about.css",
                    "~/Content/admin.css",
                    "~/Content/docs.css",
                    "~/Content/downloads.css",
                    "~/Content/faq.css",
                    "~/Content/feat.css",
                    "~/Content/latex-html.css",
                    "~/Content/latex.css",
                    "~/Content/login.css",
                    "~/Content/more-downloads.css",
                    "~/Content/more-examples.css",
                    "~/Content/privacy.css",
                    "~/Content/publications.css",
                    "~/Content/site.css",
                    "~/Content/terms.css",
                    "~/Content/xml.css"
                    ));
            // Set to true to enable full bundle optimization
            BundleTable.EnableOptimizations = false;
        }
	}
}
