using System.Windows;

namespace TarkovAssistant.App.Models
{
    public class FormWrapper
    {
        private FormModel _form;
        private bool _fullScreen = false;
        private WindowStyle _style;
        private ResizeMode _resizeMode;
        private double _left;
        private double _top;
        private double _width;
        private double _height;

        public bool IsFullScreen
        {
            get => _fullScreen;
            set
            {
                if (_fullScreen != value)
                {
                    if (value)
                    {
                        EnableFullScreenMode();
                    }
                    else
                    {
                        DisableFullScreenMode();
                    }
                }
            }
        }

        public FormWrapper(FormModel form)
        {
            _form = form;
            SaveFormState();
        }

        public void EnableFullScreenMode()
        {
            SaveFormState();

            _form.WindowStyle = WindowStyle.None;
            _form.ResizeMode = ResizeMode.NoResize;
            _form.Left = 0;
            _form.Top = 0;
            _form.Width = SystemParameters.PrimaryScreenWidth;
            _form.Height = SystemParameters.PrimaryScreenHeight;

            _fullScreen = true;
        }

        public void DisableFullScreenMode()
        {
            RestoreFormState();
            _fullScreen = false;
        }

        private void SaveFormState()
        {
            _style = _form.WindowStyle;
            _resizeMode = _form.ResizeMode;
            _left = _form.Left;
            _top = _form.Top;
            _width = _form.Width;
            _height = _form.Height;
        }

        public void RestoreFormState()
        {
            _form.WindowStyle = _style;
            _form.ResizeMode = _resizeMode;
            _form.Left = _left;
            _form.Top = _top;
            _form.Width = _width;
            _form.Height = _height;
        }
    }
}
