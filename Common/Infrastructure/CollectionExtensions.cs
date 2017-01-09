using System.Collections.Generic;
using System.Threading;

namespace MMU.BoerseDownloader.Common.Infrastructure
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> AsCancellable<T>(this IEnumerable<T> source, CancellationToken token)
        {
            foreach (var item in source)
            {
                token.ThrowIfCancellationRequested();
                yield return item;
            }
        }
    }
}