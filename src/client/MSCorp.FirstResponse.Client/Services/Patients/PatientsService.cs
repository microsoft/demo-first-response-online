using System;
using System.Threading.Tasks;
using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.Data.Base;
using MSCorp.FirstResponse.Client.Helpers;
using System.Diagnostics;

namespace MSCorp.FirstResponse.Client.Services.Patients
{
    public class PatientsService : IPatientsService
    {
        private readonly IRequestProvider _requestProvider;

        public PatientsService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async  Task<PatientModel> GetPatientAsync(string patientId)
        {
            UriBuilder builder = new UriBuilder(Settings.ServiceEndpoint);
            builder.Path = $"patient/{patientId}";

            string uri = builder.ToString();
            PatientModel patient = await _requestProvider.GetAsync<PatientModel>(uri);

            return patient;
        }

        public async Task<bool> AddReportAsync(string patientId, EpcrModel ePcr)
        {
            UriBuilder builder = new UriBuilder(Settings.ServiceEndpoint);
            builder.Path = $"patient/{patientId}/epcr";

            string uri = builder.ToString();

            try
            {
                await _requestProvider.PostAsync(uri, ePcr);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data in: {ex}");
                return false;
            }

            return true;
        }
    }
}
