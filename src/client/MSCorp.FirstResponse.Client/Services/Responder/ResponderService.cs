using MSCorp.FirstResponse.Client.Data.Base;
using MSCorp.FirstResponse.Client.Extensions;
using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Models;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Responder
{
    public class ResponderService : IResponderService
    {
        private readonly IRequestProvider _requestProvider;

        public ResponderService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ObservableCollection<ResponderModel>> GetRespondersAsync()
        {
            UriBuilder builder = new UriBuilder(Settings.ServiceEndpoint);
            builder.Path = $"api/city/{Settings.SelectedCity}/responders";

            string uri = builder.ToString();

            try
            {
                IEnumerable<ResponderModel> respondersRoutes =
                    await _requestProvider.GetAsync<IEnumerable<ResponderModel>>(uri);

                if (respondersRoutes != null)
                {
                    return respondersRoutes.ToObservableCollection();
                }
                else
                {
                    return default(ObservableCollection<ResponderModel>);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error on routes api {ex} ");
                return default(ObservableCollection<ResponderModel>);
            }
        }

        public async Task<ObservableCollection<RouteModel>> GetRoutesAsync()
        {
            UriBuilder builder = new UriBuilder(Settings.ServiceEndpoint);
            builder.Path = $"api/city/{Settings.SelectedCity}/responder-routes";

            string uri = builder.ToString();

            try
            {
                IEnumerable<RouteModel> incidents =
                    await _requestProvider.GetAsync<IEnumerable<RouteModel>>(uri);

                if (incidents != null)
                {
                    return incidents.ToObservableCollection();
                }
                else
                {
                    return default(ObservableCollection<RouteModel>);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error on routes api {ex} ");
                return default(ObservableCollection<RouteModel>);
            }
        }
    }
}