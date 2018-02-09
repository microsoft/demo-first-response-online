using System.Collections.Generic;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Extensions
{
    internal static class LayoutExtensions
    {
        internal static IEnumerable<View> GetDescendants(this Layout<View> layout)
        {
            var descendants = new List<View>();

            foreach (var child in layout.Children)
            {
                descendants.Add(child);

                if (child is Layout<View>)
                {
                    descendants.AddRange(GetDescendants(child as Layout<View>));
                }
            }

            return descendants;
        }
    }
}
