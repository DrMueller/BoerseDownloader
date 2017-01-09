namespace MMU.BoerseDownloader.WpfUI.Models
{
    public class StringValue
    {
        // http://stackoverflow.com/questions/479329/how-to-bind-a-liststring-to-a-datagridview-control 
        public StringValue(string s)
        {
            Value = s;
        }

        public string Value { get; private set; }
    }
}