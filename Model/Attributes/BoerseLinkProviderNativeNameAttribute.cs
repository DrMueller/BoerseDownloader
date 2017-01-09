using System;

namespace MMU.BoerseDownloader.Model.Attributes
{
    public class BoerseLinkProviderNativeNameAttribute : Attribute
    {
        public BoerseLinkProviderNativeNameAttribute(string nativeName)
        {
            NativeName = nativeName;
        }

        public string NativeName { get; }
    }
}