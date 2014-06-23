using Boodschappie.Controllers;
using Boodschappie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace Boodschappie
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

       
        protected void Application_Start()
        {

            System.Diagnostics.Debug.WriteLine("[Starting Database init]");
            System.Diagnostics.Debug.WriteLine("[DB: Clearing all Pools]");
            SqlConnection.ClearAllPools();
            
            System.Diagnostics.Debug.WriteLine("[DB: Initializing tables]");
            //Database.SetInitializer<AppContext>(new DropCreateDatabaseAlways<AppContext>());
            //implemented custom initializer with overridden Seed method
            Database.SetInitializer(new DbInitializer());
            AppContext db = new AppContext();
            db.Database.Initialize(true);
            
            System.Diagnostics.Debug.WriteLine("[DB: End of init]");

            //AccountController.SeedUsers();


            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
    }
}