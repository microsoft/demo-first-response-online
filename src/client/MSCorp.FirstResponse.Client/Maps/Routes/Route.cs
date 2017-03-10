using MSCorp.FirstResponse.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MSCorp.FirstResponse.Client.Maps.Routes
{
    public abstract class Route
    {
        private string _id;
        private int _lastRoutePositionIndex;
        private List<Geoposition> _routePositions;
        private Geoposition _currentPosition;

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public int LastRoutePositionIndex
        {
            get
            {
                return _lastRoutePositionIndex;
            }
        }

        public Geoposition CurrentPosition
        {
            get
            {
                return _currentPosition;
            }
        }

        public Geoposition NextPosition
        {
            get
            {
                return _lastRoutePositionIndex < TotalPositions - 2
                    ? _routePositions[_lastRoutePositionIndex + 1]
                    : _routePositions[TotalPositions - 1];
            }
        }

        public List<Geoposition> RoutePositions
        {
            get
            {
                return _routePositions;
            }
        }

        public int TotalPositions
        {
            get
            {
                return _routePositions.Count;
            }
        }

        public bool ArrivedToDestination
        {
            get
            {
                return _lastRoutePositionIndex == TotalPositions - 1;
            }
        }

        public bool ArrivedToMiddle
        {
            get
            {
                return _lastRoutePositionIndex == (TotalPositions / 2);
            }
        }

        public Route(Geoposition[] routePositions)
        {
            if (!routePositions.Any())
            {
                throw new ArgumentException("Error creating Route: positions array length is zero");
            }

            _id = Guid.NewGuid().ToString();

            _routePositions = routePositions.ToList();
        }

        public void Init()
        {
            _lastRoutePositionIndex = 0;
            _currentPosition = RoutePositions[0];
        }

        public void AddStartPoint(Geoposition start)
        {
            _routePositions.Insert(0, start);
        }

        public void AddRouteToStartPoint(IEnumerable<Geoposition> startRoute)
        {
            _routePositions.InsertRange(0, startRoute);
        }

        public void MoveToNextPosition()
        {
            if (!ArrivedToDestination)
            {
                _currentPosition = NextPosition;

                if (_lastRoutePositionIndex < TotalPositions - 1)
                {
                    _lastRoutePositionIndex++;
                }
            }
        }

        public void MoveToPosition(Geoposition position)
        {
            _currentPosition = position;
        }
    }

    public class Route<T> : Route
    {
        public T Element { get; set; }

        public Route(Geoposition[] routePositions) : base(routePositions)
        {
        }
    }
}