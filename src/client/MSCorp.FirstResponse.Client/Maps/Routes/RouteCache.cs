using MSCorp.FirstResponse.Client.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MSCorp.FirstResponse.Client.Maps.Routes
{
    internal class RouteCache
    {
        private readonly ConcurrentDictionary<RouteCacheItem, IEnumerable<Geoposition>> _dictionary;

        public RouteCache()
        {
            _dictionary = new ConcurrentDictionary<RouteCacheItem, IEnumerable<Geoposition>>();
        }

        public void SetRoute(Geoposition from, Geoposition to, IEnumerable<Geoposition> positions)
        {
            var item = new RouteCacheItem(from, to);

            _dictionary.AddOrUpdate(item, positions, (k, v) =>
            {
                return positions;
            });
        }

        public IEnumerable<Geoposition> GetRoute(Geoposition from, Geoposition to)
        {
            var item = new RouteCacheItem(from, to);

            IEnumerable<Geoposition> positions = null;
            _dictionary.TryGetValue(item, out positions);

            return positions;
        }

        public bool HasRoute(Geoposition from, Geoposition to)
        {
            var item = new RouteCacheItem(from, to);

            return _dictionary.ContainsKey(item);
        }

        internal class RouteCacheItem
        {
            public Geoposition From { get; private set; }

            public Geoposition To { get; private set; }

            public RouteCacheItem(Geoposition from, Geoposition to)
            {
                From = from ?? throw new ArgumentNullException(nameof(from));
                To = to ?? throw new ArgumentNullException(nameof(to));
            }

            public override bool Equals(object obj)
            {
                RouteCacheItem item = obj as RouteCacheItem;
                if (item == null)
                    return false;

                return From.Equals(item.From) && To.Equals(item.To);
            }

            public override int GetHashCode()
            {
                int hash = 17;
                hash = hash * 23 + From.GetHashCode();
                hash = hash * 23 + To.GetHashCode();
                return hash;
            }
        }
    }
}
