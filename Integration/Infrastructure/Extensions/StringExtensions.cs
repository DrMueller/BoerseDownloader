namespace MMU.BoerseDownloader.Integration.Infrastructure.Extensions
{
    internal static class StringExtensions
    {
        internal static bool CheckIfValidDownloadEntryTitle(this string str)
        {
            var strToCheck = str?.Trim();
            return !string.IsNullOrEmpty(strToCheck) && strToCheck != "\n";
        }
    }
}