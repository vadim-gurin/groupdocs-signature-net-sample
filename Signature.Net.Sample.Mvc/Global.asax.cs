﻿using System.Web.Mvc;
using System.Web.Routing;
using Signature.Net.Sample.Mvc;

namespace Signature.Net.Sample.Mvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //GroupdocsSignature.Init(new GroupdocsSignatureSettings
            //{
            //    RootStoragePath = Server.MapPath("~/App_Data/"),
            //    DatabaseProvider = new MsSqlProvder(System.Configuration.ConfigurationManager.ConnectionStrings["SignatureDb"].ConnectionString)
            //});
        }

    }
}