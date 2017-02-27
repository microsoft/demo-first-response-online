using MSCorp.FirstResponse.Client.Common;
using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.UWP.Extensions;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MSCorp.FirstResponse.Client.UWP.Controls
{
    public sealed partial class IncidentInfoIcon : UserControl
    {
        public event EventHandler<IncidentSelectedEventArgs> IncidentIconExit;
        public event EventHandler<IncidentSelectedEventArgs> IncidentIconDetails;
        public event EventHandler<IncidentSelectedEventArgs> IncidentIconNavigate;

        public IncidentInfoIcon(IncidentModel incident, bool menuVisible = false)
        {
            InitializeComponent();

            Incident = incident;

            if (Incident.IsHighPriority)
            {
                PriorityIconStoryboard.Begin();
            }

            var visibility = Incident.IsHighPriority ? Visibility.Visible : Visibility.Collapsed;
            PriorityImage.Visibility = BackEllipse.Visibility = PulseEllipse.Visibility = visibility;
            SetIconImage(incident);
            ButtonOne.Visibility = menuVisible ? Visibility.Visible : Visibility.Collapsed;
            IncidentTitle.Text = Incident.Title;
            IncidentDetails.Text = Incident.Description;
            IncidentLocation.Text = Incident.Address;

            Header.Background = new SolidColorBrush(Incident.IncidentColor.ToMediaColor());
        }

        public IncidentModel Incident { get; }
        private void OnIncidentIconExit(int incidentId) => IncidentIconExit?.Invoke(this, new IncidentSelectedEventArgs(incidentId));
        private void OnIncidentIconDetails(int incidentId, bool detailsVisible) => IncidentIconDetails?.Invoke(this, new IncidentSelectedEventArgs(incidentId, detailsVisible));
        private void OnIncidentIconNavigate(int incidentId) => IncidentIconNavigate?.Invoke(this, new IncidentSelectedEventArgs(incidentId));

        public void Close()
        {
            OnIncidentIconExit(Incident.Id);
        }

        private void OnIconImageTapped(object sender, TappedRoutedEventArgs e)
        {
            var currentVis = ButtonOne.Visibility;
            OnIncidentIconDetails(Incident.Id, currentVis == Visibility.Collapsed);
        }

        private void OnNavigateButtonClick(object sender, RoutedEventArgs e)
        {
            OnIncidentIconNavigate(Incident.Id);
        }

        private void OnCloseButtonClicked(object sender, RoutedEventArgs e)
        {
            OnIncidentIconExit(Incident.Id);
        }

        private void SetIconImage(IncidentModel incident)
        {
            switch (incident.IncidentCategory)
            {
                case IncidentType.Alert:
                    HeaderImage.Source =
                      new BitmapImage(new Uri("ms-appx:///Assets/Icons/icon_alert.png"));
                    break;
                case IncidentType.Animal:
                    HeaderImage.Source =
                      new BitmapImage(new Uri("ms-appx:///Assets/Icons/icon_animal.png"));
                    break;
                case IncidentType.Arrest:
                    HeaderImage.Source =
                      new BitmapImage(new Uri("ms-appx:///Assets/Icons/icon_arrest.png"));
                    break;
                case IncidentType.Car:
                    HeaderImage.Source =
                      new BitmapImage(new Uri("ms-appx:///Assets/Icons/icon_car.png"));
                    break;
                case IncidentType.Fire:
                    HeaderImage.Source =
                      new BitmapImage(new Uri("ms-appx:///Assets/Icons/icon_fire.png"));
                    break;
                case IncidentType.OfficerRequired:
                    HeaderImage.Source =
                      new BitmapImage(new Uri("ms-appx:///Assets/Icons/icon_officer.png"));
                    break;
                case IncidentType.Stranger:
                    HeaderImage.Source =
                      new BitmapImage(new Uri("ms-appx:///Assets/Icons/icon_stranger.png"));
                    break;
                default:
                    HeaderImage.Source =
                      new BitmapImage(new Uri("ms-appx:///Assets/Icons/icon_car.png"));
                    break;
            }
        }
    }
}