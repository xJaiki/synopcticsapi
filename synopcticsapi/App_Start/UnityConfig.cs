using synopcticsapi.Data;
using synopcticsapi.Repository;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace synopcticsapi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            // Register database context
            container.RegisterType<SynopticDbContext>(new PerResolveLifetimeManager());

            // Register repository
            container.RegisterType<ISynopticRepository, SynopticRepository>();
        }
    }
}