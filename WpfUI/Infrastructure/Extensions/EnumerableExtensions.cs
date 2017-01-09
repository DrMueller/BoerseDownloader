using System;
using System.Collections.Generic;

namespace MMU.BoerseDownloader.WpfUI.Infrastructure.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var entry in list)
            {
                action(entry);
            }
        }
    }
}