using System.Collections.Generic;
using MMU.BoerseDownloader.Model;

namespace MMU.BoerseDownloader.Logics.Logics
{
    public class DownloadEntryEqualityComparer : EqualityComparer<DownloadEntry>
    {
        public override bool Equals(DownloadEntry x, DownloadEntry y)
        {
            return x.GetHashCode() == y.GetHashCode();
        }

        public override int GetHashCode(DownloadEntry obj)
        {
            var str = $"{obj.DownloadContextName}_{obj.DownloadEntryTitles.ToString()}_{obj.DownloadLink}";
            return str.GetHashCode();
        }
    }
}