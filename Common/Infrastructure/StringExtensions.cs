using MMU.BoerseDownloader.Model.Attributes;
using MMU.BoerseDownloader.Model.Enumerations;

namespace MMU.BoerseDownloader.Common.Infrastructure
{
    public static class StringExtensions
    {
        public static bool ContainsCaseInsensitive(this string str, string search)
        {
            return str.IndexOf(search) > -1;
        }

        public static string GetNativeName(this BoerseLinkProvider boerseLinkprovider)
        {
            var type = typeof(BoerseLinkProvider);

            var memInfo = type.GetMember(boerseLinkprovider.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(BoerseLinkProviderNativeNameAttribute), false);
            var nativeName = ((BoerseLinkProviderNativeNameAttribute)attributes[0]).NativeName;

            return nativeName;
        }
    }
}