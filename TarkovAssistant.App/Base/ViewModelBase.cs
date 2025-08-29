using System.ComponentModel;

namespace TarkovAssistant.App.Base
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected ViewModelBase()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        void IDisposable.Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose()
        {

        }
    }
}
