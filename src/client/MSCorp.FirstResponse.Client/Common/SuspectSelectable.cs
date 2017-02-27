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
            base.Name = model.Name;
            base.HairColor = model.HairColor;
            base.EyeColor = model.EyeColor;
            base.SkinColor = model.SkinColor;
            base.Sex = model.Sex;
            base.SuspectSearchImage = model.SuspectSearchImage;
            _suspectSelected = suspectSelected;
        }
    }
}
