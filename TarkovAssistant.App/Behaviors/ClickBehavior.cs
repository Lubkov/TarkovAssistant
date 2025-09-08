using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Input;

namespace TarkovAssistant.App.Behaviors
{
    public class ClickBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(ClickBehavior),
            new PropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object),
                typeof(ClickBehavior),
                new PropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseLeftButtonDown += OnMouseDown;
            AssociatedObject.MouseLeftButtonUp += OnMouseUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.MouseLeftButtonDown -= OnMouseDown;
            AssociatedObject.MouseLeftButtonUp -= OnMouseUp;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            AssociatedObject.CaptureMouse();
            e.Handled = true;
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Command != null && Command.CanExecute(CommandParameter))
                Command.Execute(CommandParameter);

            AssociatedObject.ReleaseMouseCapture();
            e.Handled = true;
        }
    }
}