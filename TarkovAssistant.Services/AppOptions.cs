namespace TarkovAssistant.Services
{
    public class AppOptions
    {
        public string Server { get; set; } = string.Empty;
        public string SreenshotPath { get; set; } = string.Empty;
        public bool TrackLocation { get; set; }
        public int? Profile { get; set; }
    }
}
