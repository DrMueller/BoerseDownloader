using System.Collections.Generic;
using System.Linq;

namespace MMU.BoerseDownloader.WpfUI.Models
{
    public class ViewModelParameterCollection
    {
        public ViewModelParameterCollection(params ViewModelParameter[] parameters)
        {
            Parameters = parameters;
        }

        public static ViewModelParameterCollection Empty { get; } = new ViewModelParameterCollection();

        public bool HasValue
        {
            get
            {
                return this != Empty;
            }
        }

        public ViewModelParameter this[string name]
        {
            get
            {
                return Parameters.FirstOrDefault(f => f.Name == name);
            }
        }

        public IEnumerable<ViewModelParameter> Parameters { get; private set; }
    }
}