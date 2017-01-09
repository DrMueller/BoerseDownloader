using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MMU.BoerseDownloader.Model
{
    public class DownloadEntryTitleCollection : ReadOnlyCollection<DownloadEntryTitle>
    {
        public DownloadEntryTitleCollection(IList<DownloadEntryTitle> list) : base(list)
        {
        }

        public string ShortName
        {
            get
            {
                var titleStrings = Items.Select(f => f.Title);
                var result = string.Join(string.Empty, titleStrings);
                if (result.Length > 30)
                {
                    result = result.Substring(0, 30);
                }

                return result;
            }
        }

        public override string ToString()
        {
            return ShortName;
        }
    }
}