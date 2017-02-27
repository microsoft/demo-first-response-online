using MSCorp.FirstResponse.Client.ViewModels;
using MSCorp.FirstResponse.Client.ViewModels.Base;
using MSCorp.FirstResponse.Client.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Services.Navigation
{
    public partial class NavigationService : INavigationService
    {
        protected readonly Dictionary<Type, Type> _mappings;

        protected Application CurrentApplication
        {
            get
            {
                return Application.Current;
            }
        }

        public NavigationService()
        {
            _mappings = new Dictionary<Type, Type>();

            CreatePageViewModelMappings();
        }

        public Task InitializeAsync()
        {
            return NavigateToAsync<LoginViewModel>();
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return InternalNavigateToAsync(viewModelType, null);
        }

        public Task NavigateToAsync(Type viewModelType, object parameter)
        {
            return InternalNavigateToAsync(viewModelType, parameter);
        }

        public async Task NavigateBackAsync()
        {
            if (CurrentApplication.MainPage is MainView)
            {
                var mainPage = CurrentApplication.MainPage as MainView;
                await mainPage.Navigation.PopAsync();
            }
            else if (CurrentApplication.MainPage != null)
            {
                await CurrentApplication.MainPage.Navigation.PopAsync();
            }
        }

        public virtual Task RemoveLastFromBackStackAsync()
        {
            var mainPage = CurrentApplication.MainPage as CustomNavigationView;

            if (mainPage != null)
            {
                mainPage.Navigation.RemovePage(
                    mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        public virtual Task RemoveBackStackAsync()
        {
            var mainPage = CurrentApplication.MainPage as CustomNavigationView;

            if (mainPage != null)
            {
                for (int i = 0; i < mainPage.Navigation.NavigationStack.Count - 1; i++)
                {
                    var page = mainPage.Navigation.NavigationStack[i];
                    mainPage.Navigation.RemovePage(page);
                }
            }

            return Task.FromResult(true);
        }

        protected virtual async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            Page page = CreateAndBindPage(viewModelType, parameter);

            if (page is MainView)
            {
                CurrentApplication.MainPage = page;
            }
            else if (page is LoginView)
            {
                CurrentApplication.MainPage = new CustomNavigationView(page);
            }
            else if (CurrentApplication.MainPage is MainView)
            {
                var mainPage = CurrentApplication.MainPage as MainView;
                var navigationPage = mainPage.Detail as CustomNavigationView;

                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    navigationPage = new CustomNavigationView(page);
                    mainPage.Detail = navigationPage;
                }

                mainPage.IsPresented = false;
            }
            else
            {
                var navigationPage = CurrentApplication.MainPage as CustomNavigationView;

                if (navigationPage != null)
                {
                    await navigationPage.PushAsync(page);
                }
                else
                {
                    CurrentApplication.MainPage = new CustomNavigationView(page);
                }
            }

            await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
        }

        protected Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!_mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
            }

            return _mappings[viewModelType];
        }

        protected Page CreateAndBindPage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a page");
            }

            Page page = Activator.CreateInstance(pageType) as Page;
            ViewModelBase viewModel = ViewModelLocator.Instance.Resolve(viewModelType) as ViewModelBase;
            page.BindingContext = viewModel;

            return page;
        }

        private void CreatePageViewModelMappings()
        {
            _mappings.Add(typeof(NewTicketViewModel), typeof(NewTicketPopupView));
            _mappings.Add(typeof(SuspectViewModel), typeof(SuspectPopupView));
            _mappings.Add(typeof(CitiesViewModel), typeof(CitiesView));
            _mappings.Add(typeof(LoginViewModel), typeof(LoginView));
            _mappings.Add(typeof(MainViewModel), typeof(MainView));
            _mappings.Add(typeof(PowerBIViewModel), typeof(PowerBIView));
        }
    }
}
