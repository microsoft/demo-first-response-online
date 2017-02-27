using MSCorp.FirstResponse.Client.Controls;

namespace MSCorp.FirstResponse.Client.Maps
{
    public abstract class AbstractHeatMapManager
    {
        public CustomMap FormsMap { get; private set; }

        protected AbstractHeatMapManager(CustomMap formsMap)
        {
            FormsMap = formsMap;
        }

        public void UpdateVisibility()
        {
            CreateHeatMapIfNeeded();

            if (FormsMap.IsHeatMapVisible)
            {
                ShowHeatMap();
            }
            else
            {
                HideHeatMap();
            }
        }

        protected abstract void HideHeatMap();

        protected abstract void ShowHeatMap();

        protected abstract void CreateHeatMapIfNeeded();
    }
}
