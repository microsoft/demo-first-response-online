using Microsoft.Practices.Unity;
using MSCorp.FirstResponse.Client.Data.Base;
using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Services.Authentication;
using MSCorp.FirstResponse.Client.Services.Cities;
using MSCorp.FirstResponse.Client.Services.Dialog;
using MSCorp.FirstResponse.Client.Services.Heatmap;
using MSCorp.FirstResponse.Client.Services.Incidents;
using MSCorp.FirstResponse.Client.Services.Mock.Authentication;
using MSCorp.FirstResponse.Client.Services.Navigation;
using MSCorp.FirstResponse.Client.Services.Responder;
using MSCorp.FirstResponse.Client.Services.Ticket;
using MSCorp.FirstResponse.Client.Services.Ticket.Mock;
using System;

namespace MSCorp.FirstResponse.Client.ViewModels.Base
{
    public class ViewModelLocator
    {
        private readonly IUnityContainer _unityContainer;

        private static readonly ViewModelLocator _instance = new ViewModelLocator();

        public static ViewModelLocator Instance
        {
            get { return _instance; }
        }

        public bool UseMockService
        {
            get { return Settings.UseMockService; }
            set
            {
                Settings.UseMockService = value;
                RegisterServices();
            }
        }

        protected ViewModelLocator()
        {
            _unityContainer = new UnityContainer();

            // providers
            _unityContainer.RegisterType<IRequestProvider, RequestProvider>();

            // Services    
            RegisterServices();

            // View models
            _unityContainer.RegisterType<CitiesViewModel>();
            _unityContainer.RegisterType<IncidentListViewModel>();
            _unityContainer.RegisterType<LoginViewModel>();
            _unityContainer.RegisterType<MainViewModel>();
            _unityContainer.RegisterType<PowerBIViewModel>();
            _unityContainer.RegisterType<ResponderListViewModel>();
            _unityContainer.RegisterType<SuspectViewModel>();
        }

        private void RegisterServices()
        {
            // Services    
            if (UseMockService)
            {
                _unityContainer.RegisterType<IAuthenticationService, MockAuthenticationService>();
                _unityContainer.RegisterType<ICitiesService, MockCitiesService>();
                _unityContainer.RegisterType<ITicketService, MockTicketService>();
                _unityContainer.RegisterType<IHeatmapService, MockHeatmapService>();
                _unityContainer.RegisterType<IIncidentsService, MockIncidentsService>();
                _unityContainer.RegisterType<IResponderService, MockResponderService>();
                _unityContainer.RegisterType<ISuspectService, MockSuspectService>();
            }
            else
            {
                _unityContainer.RegisterType<IAuthenticationService, AuthenticationService>();
                _unityContainer.RegisterType<IIncidentsService, IncidentsService>();
                _unityContainer.RegisterType<ITicketService, TicketService>();
                _unityContainer.RegisterType<IHeatmapService, HeatmapService>();
                _unityContainer.RegisterType<ICitiesService, CitiesService>();
                _unityContainer.RegisterType<IResponderService, ResponderService>();
                _unityContainer.RegisterType<ISuspectService, SuspectService>();
            }

            _unityContainer.RegisterType<IDialogService, DialogService>();
            _unityContainer.RegisterType<INavigationService, NavigationService>();
        }

        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return _unityContainer.Resolve(type);
        }

        public void Register<T>(T instance)
        {
            _unityContainer.RegisterInstance<T>(instance);
        }

        public void Register<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>();
        }

        public void RegisterSingleton<TInterface, T>() where T : TInterface
        {
            _unityContainer.RegisterType<TInterface, T>(new ContainerControlledLifetimeManager());
        }
    }
}