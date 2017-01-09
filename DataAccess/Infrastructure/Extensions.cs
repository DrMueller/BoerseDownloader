namespace MMU.BoerseDownloader.DataAccess.Infrastructure
{
    internal static class Extensions
    {
        internal static string GetSafeFilename(this string unsafeFilename)
        {
            var result = unsafeFilename;

            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                result = result.Replace(c, '_');
            }

            var charactersWeDontLike = new[] { @"\", "." };
            foreach (var c in charactersWeDontLike)
            {
                result = result.Replace(c, "_");
            }

            return result;
        }
    }
}