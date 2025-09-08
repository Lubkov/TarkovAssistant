namespace TarkovAssistant.Services
{
    public class FileMonitor : IFileMonitor
    {
        private FileSystemWatcher? _watcher;
        private EventHandler<FileCreatedEventArgs>? _fileCreated;

        public bool Actived 
        { 
            get => _watcher?.EnableRaisingEvents ?? false;            
        }

        public void Start(string path, string filter = "*.*")
        {
            if (Directory.Exists(path))
            {
                _watcher = new FileSystemWatcher(path, filter)
                {
                    EnableRaisingEvents = false,
                    IncludeSubdirectories = false
                };
                _watcher.Created += OnWatcherCreated;
                _watcher.EnableRaisingEvents = true;
            }            
        }

        public void Stop()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
            }
        }

        public event EventHandler<FileCreatedEventArgs> FileCreated
        {
            add => _fileCreated += value;
            remove => _fileCreated -= value;
        }

        private void OnWatcherCreated(object sender, FileSystemEventArgs e)
        {
            _fileCreated?.Invoke(this, new FileCreatedEventArgs(e.Name ?? ""));
        }

        void IDisposable.Dispose()
        {
            if (_watcher != null)
            {
                _watcher.Created -= OnWatcherCreated;
                _watcher.Dispose();
            }
        }
    }
}
