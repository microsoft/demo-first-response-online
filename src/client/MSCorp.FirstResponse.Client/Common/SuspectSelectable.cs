using MSCorp.FirstResponse.Client.Models;

namespace MSCorp.FirstResponse.Client.Common
{
    public class SuspectSelectable : SuspectModel
    {
        private bool _suspectSelected;

        public bool SuspectSelected
        {
            get
            {
                return _suspectSelected;
            }

            set
            {
                if (_suspectSelected != value)
                {
                    _suspectSelected = value;
                    RaisePropertyChanged(() => SuspectSelected);
                }
            }
        }

        public SuspectSelectable() : base()
        {
            _suspectSelected = false;
        }

        public SuspectSelectable(SuspectModel model, bool suspectSelected) : base()
        {
            Id = model.Id;
            Name = model.Name;
            HairColor = model.HairColor;
            EyeColor = model.EyeColor;
            SkinColor = model.SkinColor;
            Sex = model.Sex;
            SuspectSearchImage = model.SuspectSearchImage;
            _suspectSelected = suspectSelected;
        }
    }
}
