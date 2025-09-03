namespace TarkovAssistant.Services
{
    public interface IFileMonitor : IDisposable
    {
        bool Actived { get; }
        void Start(string path, string filter = "*.*");
        void Stop();

        event EventHandler<FileCreatedEventArgs> FileCreated;        
    }
}
