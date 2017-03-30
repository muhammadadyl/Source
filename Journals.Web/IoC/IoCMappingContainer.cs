using AutoMapper;
using Journals.Data;
using Journals.Data.Infrastructure;
using Journals.Repository;
using Journals.Service;
using Journals.Service.Interfaces;
using Journals.Web.Controllers;
using Journals.Web.Mapper;
using Microsoft.Practices.Unity;

namespace Journals.Web.IoC
{
    public static class IoCMappingContainer
    {
        private static IUnityContainer _Instance = new UnityContainer();

        static IoCMappingContainer()
        {
        }

        public static IUnityContainer GetInstance()
        {
            _Instance.RegisterType<HomeController>();
            _Instance.RegisterType<PublisherController>();
            _Instance.RegisterType<SubscriberController>();
            _Instance.RegisterType<AccountController>();

            _Instance.RegisterType<IUserProfileRepository, UserProfileRepository>(new HierarchicalLifetimeManager());
            _Instance.RegisterType<IJournalRepository, JournalRepository>(new HierarchicalLifetimeManager());
            _Instance.RegisterType<ISubscriptionRepository, SubscriptionRepository>(new HierarchicalLifetimeManager());
            _Instance.RegisterType<IStaticMembershipService, StaticMembershipService>(new HierarchicalLifetimeManager());
            _Instance.RegisterType<IUserProfileService, UserProfileService>(new HierarchicalLifetimeManager());
            _Instance.RegisterType<ISubscriptionService, SubscriptionService>(new HierarchicalLifetimeManager());
            _Instance.RegisterType<IJournalService, JournalService>(new HierarchicalLifetimeManager());
            _Instance.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());
            _Instance.RegisterType<IDatabaseFactory, DatabaseFactory<JournalsContext>>(new HierarchicalLifetimeManager());

            var mapper = MappingProfile.InitializeAutoMapper().CreateMapper();
            _Instance.RegisterInstance<IMapper>(mapper);

            return _Instance;
        }
    }
}