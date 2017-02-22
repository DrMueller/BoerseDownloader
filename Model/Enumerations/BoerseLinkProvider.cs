using MMU.BoerseDownloader.Model.Attributes;

namespace MMU.BoerseDownloader.Model.Enumerations
{
    public enum BoerseLinkProvider
    {
        [BoerseLinkProviderNativeName("Hellraz0r")]
        Hellraz0r,
        [BoerseLinkProviderNativeName("serienJK")]
        SerienJk,
        [BoerseLinkProviderNativeName("Kristallprinz")]
        Kristallprinz,
        [BoerseLinkProviderNativeName("Tardis Core")]
        TardisCore,
        [BoerseLinkProviderNativeName("bauklo")]
        Bauklo
    }
}