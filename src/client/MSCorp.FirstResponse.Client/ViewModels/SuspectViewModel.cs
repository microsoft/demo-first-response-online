using System.Linq;
using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.ViewModels.Base;
using Xamarin.Forms;
using System.Windows.Input;
using MSCorp.FirstResponse.Client.Services.Cities;
using System.Collections.Generic;
using MSCorp.FirstResponse.Client.Extensions;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Services;
using MSCorp.FirstResponse.Client.Common;
using System;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.ViewModels
{
    public class SuspectViewModel : ViewModelBase
    {

        private string _name;

        private SuspectSexType _sex;

        private SuspectEyeColorType _eyeColor;

        private SuspectHairColorType _hairColor;

        private ObservableCollection<SuspectSelectable> _males = new ObservableCollection<SuspectSelectable>();

        private ObservableCollection<SuspectSelectable> _females = new ObservableCollection<SuspectSelectable>();

        private List<SuspectModel> _selectedSuspects = new List<SuspectModel>();

        private ISuspectService _suspectService;

        public List<SuspectSexType> SexTypes { get; set; }

        public List<SuspectEyeColorType> EyeColorTypes { get; set; }

        public List<SuspectHairColorType> HairColorTypes { get; set; }

        public SuspectViewModel(ISuspectService suspectService)
        {
            _suspectService = suspectService;
            SexTypes = Enum.GetValues(typeof(SuspectSexType)).Cast<SuspectSexType>().ToList();
            EyeColorTypes = Enum.GetValues(typeof(SuspectEyeColorType)).Cast<SuspectEyeColorType>().ToList();
            HairColorTypes = Enum.GetValues(typeof(SuspectHairColorType)).Cast<SuspectHairColorType>().ToList();
        }

        public async override Task InitializeAsync(object navigationData)
        {
            await base.InitializeAsync(navigationData);
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        public SuspectSexType Sex
        {
            get { return _sex; }
            set
            {
                _sex = value;
                RaisePropertyChanged(() => Sex);
            }
        }

        public SuspectEyeColorType EyeColor
        {
            get { return _eyeColor; }
            set
            {
                _eyeColor = value;
                RaisePropertyChanged(() => EyeColor);
            }
        }

        public SuspectHairColorType HairColor
        {
            get { return _hairColor; }
            set
            {
                _hairColor = value;
                RaisePropertyChanged(() => HairColor);
            }
        }

        public ObservableCollection<SuspectSelectable> Males
        {
            get { return _males; }
            set
            {
                _males = value;
                RaisePropertyChanged(() => Males);
            }
        }

        public ObservableCollection<SuspectSelectable> Females
        {
            get { return _females; }
            set
            {
                _females = value;
                RaisePropertyChanged(() => Females);
            }
        }

        public ICommand ItemSelectedCommand => new Command<SuspectSelectable>(OnSelectItem);

        public ICommand SearchCommand => new Command(SearchSuspects);

        public ICommand DoneCommand => new Command(DoneIdentify);

        public ICommand ClosePopupCommand => new Command(ClosePopup);

        private async void SearchSuspects()
        {
            var terms = new List<string> {
                _name,
                SuspectEyeColorType.Any.Equals(_eyeColor) ? "" : _eyeColor.ToString(),
                SuspectHairColorType.Any.Equals(_hairColor) ? "" : _hairColor.ToString()
            }.Where(x => !string.IsNullOrEmpty(x))
            .ToList();

            if (_sex == SuspectSexType.Female)
            {
                terms.Add("\"" + _sex.ToString() + "\"");
                terms.Add("-" + SuspectSexType.Male);
            }
            if (_sex == SuspectSexType.Male)
            {
                terms.Add("\"" + _sex.ToString() + "\"");
                terms.Add("-" + SuspectSexType.Female);
            }

            string search = string.Join(" %2B ", terms);
            var currentSuspects = await _suspectService.GetSuspectsAsync(search);

            Males = currentSuspects.Where(q => q.Sex.Equals(SuspectSexType.Male.ToString())).Select(q => new SuspectSelectable(q, false)).ToObservableCollection();
            Females = currentSuspects.Where(q => q.Sex.Equals(SuspectSexType.Female.ToString())).Select(q => new SuspectSelectable(q, false)).ToObservableCollection();
        }

        private void OnSelectItem(SuspectSelectable suspect)
        {
            if (!suspect.SuspectSelected)
            {
                suspect.SuspectSelected = true;
                if (!_selectedSuspects.Contains(suspect))
                {
                    _selectedSuspects.Add(suspect);
                }
            }
            else
            {
                suspect.SuspectSelected = false;
                if (_selectedSuspects.Contains(suspect))
                {
                    _selectedSuspects.Remove(suspect);
                }
            }
        }

        private void DoneIdentify()
        {
            MessagingCenter.Send(_selectedSuspects, MessengerKeys.SelectedSuspectsChanged);
            ClosePopup();
        }

        private async void ClosePopup()
        {
            await PopupNavigation.PopAllAsync(true);
        }

    }
}
