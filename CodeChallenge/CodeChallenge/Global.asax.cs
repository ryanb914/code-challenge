using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.WebApi;
using CodeChallenge.Controllers;
using CodeChallenge.Models;

namespace CodeChallenge {
  public class WebApiApplication : HttpApplication {
    protected void Application_Start() {
      AreaRegistration.RegisterAllAreas();
      GlobalConfiguration.Configure(WebApiConfig.Register);
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);

      // Setup Container for dependency injection
      var builder = new ContainerBuilder();
      builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
      builder.RegisterType<FlickrRestRequest>().As(typeof(IRequest));
      var container = builder.Build();

      var config = GlobalConfiguration.Configuration;
      config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
    }
  }
}
