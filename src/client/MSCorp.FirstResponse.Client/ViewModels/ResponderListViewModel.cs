using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.Services.Responder;
using MSCorp.FirstResponse.Client.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.ViewModels
{
    public class ResponderListViewModel : ViewModelBase
    {
        private IList<ResponderModel> _fullResponderList;
        private ObservableCollection<ResponderModel> _responderList;
        private ObservableCollection<RouteModel> _routeList;

        private DepartmentType _selectedDepartment;

        private IResponderService _responderService;

        public ResponderListViewModel(IResponderService responderService)
        {
            _responderService = responderService;           

            FilterClearRespondersCommand = GetFilterRespondersCommand(DepartmentType.None);
            FilterPoliceRespondersCommand = GetFilterRespondersCommand(DepartmentType.Police);
            FilterFireRespondersCommand = GetFilterRespondersCommand(DepartmentType.Fire);
            FilterMedicalRespondersCommand = GetFilterRespondersCommand(DepartmentType.Ambulance);
        }

        public IList<ResponderModel> FullResponderList
        {
            get { return _fullResponderList; }
            set
            {
                _fullResponderList = value;
                RaisePropertyChanged(() => FullResponderList);
            }
        }

        public ObservableCollection<ResponderModel> ResponderList
        {
            get { return _responderList; }
            set
            {
                _responderList = value;
                RaisePropertyChanged(() => ResponderList);
            }
        }

        public ObservableCollection<RouteModel> RouteList
        {
            get { return _routeList; }
            set
            {
                _routeList = value;
                RaisePropertyChanged(() => RouteList);
            }
        }

        public bool AllRespondersChecked => _selectedDepartment == DepartmentType.None;

        public bool PoliceRespondersChecked => _selectedDepartment == DepartmentType.Police;

        public bool FireRespondersChecked => _selectedDepartment == DepartmentType.Fire;

        public bool MedicalRespondersChecked => _selectedDepartment == DepartmentType.Ambulance;

        public ICommand FilterPoliceRespondersCommand { get; }

        public ICommand FilterFireRespondersCommand { get; }

        public ICommand FilterMedicalRespondersCommand { get; }

        public ICommand FilterClearRespondersCommand { get; }

        private Command GetFilterRespondersCommand(DepartmentType departmentType)
        {
            return new Command(() => FilterResponders(departmentType));
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await base.InitializeAsync(navigationData);

            FullResponderList = await _responderService.GetRespondersAsync();
            ResponderList = new ObservableCollection<ResponderModel>();
            RouteList = await _responderService.GetRoutesAsync();

            FilterResponders(DepartmentType.None);
        }

        public void FilterResponders(DepartmentType departmentFilterType)
        {
            ResponderList.Clear();

            var responders = FullResponderList;
            if (departmentFilterType != DepartmentType.None)
            {
                responders = responders.Where(x => x.ResponderDepartment == departmentFilterType).ToList();
            }

            foreach (var responder in responders)
            {
                ResponderList.Add(responder);
            }

            _selectedDepartment = departmentFilterType;
            OnPropertyChanged(nameof(AllRespondersChecked));
            OnPropertyChanged(nameof(PoliceRespondersChecked));
            OnPropertyChanged(nameof(FireRespondersChecked));
            OnPropertyChanged(nameof(MedicalRespondersChecked));
        }
    }
}