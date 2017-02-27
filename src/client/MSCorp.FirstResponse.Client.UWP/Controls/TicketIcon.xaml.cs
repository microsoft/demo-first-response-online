using MSCorp.FirstResponse.Client.Models;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MSCorp.FirstResponse.Client.UWP.Controls
{
    public sealed partial class TicketIcon : UserControl
    {
        public TicketIcon(TicketModel ticket)
        {
            InitializeComponent();

            Model = ticket;
            IconImage.Source = Model.Type == "Traffic Violation" ? 
                new BitmapImage(new Uri("ms-appx:///Assets/Pins/pin_car.png")) : 
                new BitmapImage(new Uri("ms-appx:///Assets/Pins/pin_pedestrian.png"));
        }

        public TicketModel Model { get; }
    }
}
