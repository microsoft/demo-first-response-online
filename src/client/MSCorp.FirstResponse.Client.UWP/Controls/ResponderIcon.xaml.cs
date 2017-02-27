using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.UWP.Extensions;
using System;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MSCorp.FirstResponse.Client.UWP.Controls
{
    public sealed partial class ResponderIcon : UserControl
    {
        public ResponderIcon(ResponderModel responder)
        {
            InitializeComponent();
            Responder = responder;

            if (responder.ResponderDepartment == DepartmentType.Responder)
            {
                ResponderImage.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            var color = Responder.StatusColor.ToMediaColor();
            StatusColor.Fill = new SolidColorBrush(color);

            ResponderType.Text = Responder.ResponderCode;
        }

        public ResponderModel Responder { get; }
        public int RouteIndex { get; set; } = 0;
        public int RouteStepIndex { get; set; } = 0;
        public double RouteStepLatitude { get; set; } = 0;
        public double RouteStepLongitude { get; set; } = 0;
        public int RouteStepMax { get; set; } = 0;
        public bool OnPatrolRoute { get; set; } = false;
        public IReadOnlyList<BasicGeoposition> IncidentResponsePath { get; set; }
        public DateTime IncidentArrivalTime { get; set; }

        public EventHandler StatusUpdated;

        public void UpdateStatus(ResponseStatus status)
        {
            Responder.Status = status;
            StatusColor.Fill = new SolidColorBrush(Responder.StatusColor.ToMediaColor());

            StatusUpdated?.Invoke(this, null);
        }
    }
}