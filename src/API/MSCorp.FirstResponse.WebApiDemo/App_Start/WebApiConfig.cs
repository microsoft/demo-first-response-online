using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MSCorp.FirstResponse.WebApiDemo.Data.Context;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;
using MSCorp.FirstResponse.WebApiDemo.Services;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;

namespace MSCorp.FirstResponse.WebApiDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var jsonFormatter = new JsonMediaTypeFormatter();
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));

            RegisterServices();
            InitializeDB();
            
            // Web API routes
            config.MapHttpAttributeRoutes();
        }

        private static void InitializeDB()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<FirstResponseContext>());
            var context = new FirstResponseContext();
            DbInitializer.Initialize(context, new UserManager<User>(new UserStore<User>(context))); // TODO: GL, get instance from DI
        }

        private static void RegisterServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<FirstResponseContext>();

            builder.Register(c => 
            new UserManager<User>(
                new UserStore<User>(
                    new FirstResponseContext())));

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
               .Where(t => t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces();

            builder.RegisterType<IncidentService>();
            builder.RegisterType<TicketService>();
            
            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
