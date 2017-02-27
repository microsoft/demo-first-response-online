using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using MSCorp.FirstResponse.Client.Models;

namespace MSCorp.FirstResponse.Client.Extensions
{
    public static class LinqExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> _LinqResult)
        {
            return new ObservableCollection<T>(_LinqResult);
        }

        public static IEnumerable<T> ToPolygonConvexHull<T>(this IEnumerable<T> _LinqResult) where T : Geoposition
        {
            return _LinqResult.Where(q => q.Latitude == _LinqResult.Max(q2 => q2.Latitude)).OrderBy(q => q.Longitude)
                      .Union(_LinqResult.Where(q => q.Latitude == _LinqResult.Min(q2 => q2.Latitude)).OrderByDescending(q => q.Longitude));
        }
    }
}