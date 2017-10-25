using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Project
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static HttpClient Client { get; } = new HttpClient();

		protected void Application_Start()
		{
			System.Web.Optimization.PreApplicationStartCode.Start();

			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
	}
}
