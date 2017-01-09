namespace MMU.BoerseDownloader.Model
{
    public class BoerseUser : Interfaces.IIdentifiable
    {
        public string LoginName { get; set; }

        public string Password { get; set; }

        public long Id { get; set; }
    }
}