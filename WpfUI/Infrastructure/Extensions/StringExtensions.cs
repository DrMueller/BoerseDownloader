using System;

namespace MMU.BoerseDownloader.WpfUI.Infrastructure.Extensions
{
    internal static class StringExtensions
    {
        internal static bool ContainsCaseInsensitive(this string str, string search)
        {
            return str.IndexOf(search, StringComparison.OrdinalIgnoreCase) > -1;
        }
    }
}