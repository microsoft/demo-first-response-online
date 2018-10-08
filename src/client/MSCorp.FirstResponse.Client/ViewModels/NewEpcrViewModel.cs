using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.Services.Patients;
using MSCorp.FirstResponse.Client.ViewModels.Base;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.ViewModels
{
    public class NewEpcrViewModel : ViewModelBase
    {
        private readonly IPatientsService _patientsService;

        private EpcrModel _ePcr;
        private PatientModel _patient;
        private string _location;
        private string _patientName;

        public EpcrModel Epcr
        {
            get
            {
                return _ePcr;
            }

            set
            {
                _ePcr = value;
                RaisePropertyChanged(() => Epcr);
            }
        }

        public PatientModel Patient
        {
            get
            {
                return _patient;
            }

            set
            {
                _patient = value;
                RaisePropertyChanged(() => Patient);
            }
        }

        public string Location
        {
            get
            {
                return _location;
            }

            set
            {
                _location = value;
                RaisePropertyChanged(() => Location);
            }
        }

        public string PatientName
        {
            get
            {
                return _patientName;
            }

            set
            {
                _patientName = value;
                RaisePropertyChanged(() => PatientName);
            }
        }

        public ICommand ClosePopupCommand => new Command(ClosePopup);

        public ICommand SubmitEpcrCommand => new Command(SubmitEpcr);

        public NewEpcrViewModel(IPatientsService patientsService)
        {
            _patientsService = patientsService;
            _ePcr = new EpcrModel
            {
                ReportDate = DateTime.Now
            };
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is IncidentInvolvedNavigationParameter)
            {
                IncidentInvolvedNavigationParameter parameters = navigationData as IncidentInvolvedNavigationParameter;

                Location = parameters.Incident.Address;
                PatientName = parameters.Person?.Name;

                await LoadPatientData(parameters.Person);
            }
        }

        private async void SubmitEpcr()
        {
            IsBusy = true;

            if (Patient != null)
            {
                if (await _patientsService.AddReportAsync(Patient.Id, Epcr))
                {
                    ClosePopup();
                }
                else
                {
                    // notify error
                    await DialogService.ShowAlertAsync("Error in the values submitted", "Processing Error", "Please Verify and try again");
                }
            }

            IsBusy = false;
        }

        private async void ClosePopup()
        {
            await PopupNavigation.PopAllAsync(true);
        }

        private async Task LoadPatientData(SuspectModel person)
        {
            try
            {
                IsBusy = true;
                Patient = await _patientsService.GetPatientAsync(person.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading patient data: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
