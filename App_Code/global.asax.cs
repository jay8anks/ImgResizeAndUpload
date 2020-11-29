using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Http;


/// <summary>
/// Summary description for global
/// </summary>
public class Global : System.Web.HttpApplication
{
    public Global()
    {
        
    }

    protected void Application_Start(object sender, EventArgs e)
    {
        RouteTable.Routes.MapHttpRoute(
        name: "DefaultApi",
        routeTemplate: "api/{controller}/{id}",
        defaults: new { id = System.Web.Http.RouteParameter.Optional }
         );
    }
}