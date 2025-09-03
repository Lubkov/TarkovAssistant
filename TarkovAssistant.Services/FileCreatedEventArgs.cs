namespace TarkovAssistant.Services
{
    public class FileCreatedEventArgs : EventArgs
    {
        public string FileName { get; }

        public FileCreatedEventArgs(string fileName)
        {
            FileName = fileName;
        }
    }
}
