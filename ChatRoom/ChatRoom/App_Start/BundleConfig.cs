using System.Web;
using System.Web.Optimization;

namespace ChatRoom {
	public class BundleConfig {
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles) {
			bundles.Add(new ScriptBundle("~/bundles/angular").Include(
						"~/Scripts/angular.js",
						"~/Scripts/angular-route.js",
						"~/Scripts/angular-cookies.js",
						"~/Scripts/angular-resource.js"));

			bundles.Add(new ScriptBundle("~/bundles/angular/app").Include(
				"~/App/app.js",
				"~/App/routing.js",
				"~/App/configuration.js",
				"~/App/services/*.js",
				"~/App/directives/common.js"));


			bundles.Add(new ScriptBundle("~/bundles/angular/controllers").Include(
				"~/App/controllers/*.js"));


			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));

			//BundleTable.EnableOptimizations = true;
		}
	}
}
