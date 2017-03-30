using Journals.Data;
using Journals.Web.IoC;
using System;
using System.Data.Entity;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace Journals.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        protected void Application_Start()
        {
            Database.SetInitializer<JournalsContext>(new JournalInitializer());
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            var mappingContainer = IoCMappingContainer.GetInstance();
            DependencyResolver.SetResolver(new IoCScopeContainer(mappingContainer));

            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // Get the exception object.
            Exception exc = Server.GetLastError();

            if (exc.GetType() == typeof(HttpException))
            {
                if (exc.Message.Contains("Maximum request length exceeded."))
                    Response.Redirect(String.Format("~/Error/RequestLengthExceeded"));
            }

            Server.ClearError();
        }

        public class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                using (var context = new JournalsContext())
                    context.UserProfiles.Find(1);

                if (!WebSecurity.Initialized)
                    WebSecurity.InitializeDatabaseConnection("JournalsDB", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }
        }
    }
}