using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Maps;
using MSCorp.FirstResponse.Client.Maps.Routes;
using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.UWP.Controls;
using MSCorp.FirstResponse.Client.UWP.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;

namespace MSCorp.FirstResponse.Client.UWP.Maps
{
    public class ResponderManager : AbstractResponderManager
    {
        private readonly MapControl _nativeMap;
        private MapPolygon _searchAreaPolygon;

        public ResponderManager(MapControl nativeMap, CustomMap formsMap, RouteManager routeManager, PushpinManager pushpinManager)
            : base(formsMap, routeManager, pushpinManager)
        {
            _nativeMap = nativeMap;
        }

        public override void StartResponderUpdater()
        {
            Task.Factory.StartNew(async () =>
            {
                await _nativeMap.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    try
                    {
                        await InnerStartResponderUpdater();
                    }
                    catch (OperationCanceledException)
                    {
                        System.Diagnostics.Debug.WriteLine("ResponderManager.StartResponderUpdater cancelled");
                    }
                });
            }, TaskCreationOptions.LongRunning);
        }

        protected override void DrawSearchAreaPolygon(Models.Geoposition[] polygonData)
        {
            if (_searchAreaPolygon != null)
            {
                _nativeMap.MapElements.Remove(_searchAreaPolygon);
            }

            IEnumerable<BasicGeoposition> positions = polygonData.Select(pos => new BasicGeoposition
            {
                Latitude = pos.Latitude,
                Longitude = pos.Longitude
            });

            Geopath polygonPath = new Geopath(positions);

            _searchAreaPolygon = new MapPolygon
            {
                Path = polygonPath,
                ZIndex = 1,
                FillColor = FormsMap.SearchPolygonColor.ToMediaColor(),
                StrokeThickness = 0
            };

            _nativeMap.MapElements.Add(_searchAreaPolygon);
        }

        protected override void RemoveSearchAreaPolygon()
        {
            if (_searchAreaPolygon != null)
            {
                _nativeMap.MapElements.Remove(_searchAreaPolygon);
            }
        }

        protected override void UpdatePushpinPosition(Route e)
        {
            DependencyObject pushpin = null;
            var itemsControl = _nativeMap.Children.OfType<MapItemsControl>()
                                                  .FirstOrDefault();

            if (e is Route<ResponderModel>)
            {
                var responderRoute = e as Route<ResponderModel>;
                pushpin = itemsControl?.Items.OfType<ResponderIcon>()
                                             .Where(icon => icon.Responder == responderRoute.Element)
                                             .FirstOrDefault();
            }
            else if (e is Route<UserRole>)
            {
                pushpin = itemsControl?.Items.OfType<UserIcon>()
                                             .FirstOrDefault();
            }

            if (pushpin != null)
            {
                var newCoordinate = new Geopoint(new BasicGeoposition
                {
                    Latitude = e.CurrentPosition.Latitude,
                    Longitude = e.CurrentPosition.Longitude
                });

                MapControl.SetLocation(pushpin, newCoordinate);
            }
        }
    }
}
