namespace MMU.BoerseDownloader.WpfUI.Models
{
    public class ViewModelParameter
    {
        public ViewModelParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; private set; }

        public object Value { get; private set; }
    }
}