using MSCorp.FirstResponse.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Helpers
{
    public static class IncidentHelper
    {
        private static IEnumerable<IncidentTypeData> IncidentTypeDataList { get; } = new[]
        {
            new IncidentTypeData { IsPriority = false, IncidentType = IncidentType.Alert, Icon = "icon_alert.png", Color = Color.FromHex("#FF5722") },
            new IncidentTypeData { IsPriority = false, IncidentType = IncidentType.Animal, Icon = "icon_animal.png", Color = Color.FromHex("#AB47BC") },
            new IncidentTypeData { IsPriority = false, IncidentType = IncidentType.Arrest, Icon = "icon_arrest.png", Color = Color.FromHex("#FFB0BEC5") },
            new IncidentTypeData { IsPriority = false, IncidentType = IncidentType.Car, Icon = "icon_car.png", Color = Color.FromHex("#8BC34A") },
            new IncidentTypeData { IsPriority = false, IncidentType = IncidentType.Fire, Icon = "icon_fire.png", Color = Color.FromHex("#FF9800") },
            new IncidentTypeData { IsPriority = false, IncidentType = IncidentType.OfficerRequired, Icon = "icon_officer.png", Color = Color.FromHex("#5C6BC0") },
            new IncidentTypeData { IsPriority = false, IncidentType = IncidentType.Stranger, Icon = "icon_stranger.png", Color = Color.FromHex("#FF4CAF50") },
            new IncidentTypeData { IsPriority = true,  IncidentType = IncidentType.Car, Icon = "icon_car.png", Color = Color.FromHex("#8BC34A") },
        };

        public static IncidentTypeData GetIncidentData(IncidentType incidentType, bool? isPriority = null)
        {
            if (isPriority != null)
            {
                var result = IncidentTypeDataList.FirstOrDefault(x => x.IncidentType == incidentType && x.IsPriority == isPriority);

                if (result != null)
                {
                    return result;
                }
            }

            return IncidentTypeDataList.FirstOrDefault(x => x.IncidentType == incidentType);
        }
    }
}