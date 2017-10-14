using Journals.Data;
using Journals.Service.Interfaces;
using Journals.Web.IoC;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using WebMatrix.WebData;

namespace Journals.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode,
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static SimpleMembershipInitializer _initializer;
        private static System.Timers.Timer timer = new System.Timers.Timer();
        private static object _initializerLock = new object();
        private static object _emailSendingLock = new object();
        private static bool _isInitialized;
        private static IUnityContainer _container { get; set; }

        protected void Application_Start()
        {
            Database.SetInitializer<JournalsContext>(new JournalInitializer());
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            _container = IoCMappingContainer.GetInstance();
            DependencyResolver.SetResolver(new IoCScopeContainer(_container));

            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);

            timer.Interval = double.Parse(ConfigurationManager.AppSettings["emailTrigger"]);//3600000;
            timer.Start();
            timer.Elapsed += Timer_Elapsed;

        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                var subscriptionService = _container.Resolve<ISubscriptionService>();
                var issueService = _container.Resolve<IIssueService>();
                var issues = issueService.GetAllNewlyAddedIssue(DateTime.Now.AddMilliseconds(0 - double.Parse(ConfigurationManager.AppSettings["emailTrigger"])));
                Parallel.ForEach(issues, (issue, state) =>
                {
                    var users = subscriptionService.GetSubscriberForJournal(issue.JournalId);
                    var emailService = _container.Resolve<IEmailService>();
                    Parallel.ForEach(users, (user, loopState) =>
                    {
                        var content =
                        string.Format(@"<p>Hi {0}</p>
<p>We are glad to inform you that our new Issue is just being released. click the following <a href='{1}/Subscriber/GetJournal/{2}'>link</a> to redirect to Journal issue</p>", user.UserName, ConfigurationManager.AppSettings["website"], issue.Id);
                        emailService.SendEmail(users.ToDictionary(d => d.UserName, v => v.EmailAddress), string.Format("Newly Issued Journal {0} vol {1} ", issue.Journal.Title, issue.Version), content);

                    });
                });
            });
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