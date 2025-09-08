using Microsoft.Xaml.Behaviors;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using TarkovAssistant.App.ViewModels;

namespace TarkovAssistant.App.Behaviors
{
    public class DragBehavior : Behavior<FrameworkElement>
    {
        private bool _isDragging;
        private Point _startMouse;
        private double _startX, _startY;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += OnMouseDown;
            AssociatedObject.MouseMove += OnMouseMove;
            AssociatedObject.MouseLeftButtonUp += OnMouseUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseLeftButtonDown -= OnMouseDown;
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.MouseLeftButtonUp -= OnMouseUp;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (AssociatedObject.DataContext is MainWindowViewModel vm)
            {
                _isDragging = true;
                _startMouse = e.GetPosition(AssociatedObject.Parent as UIElement);
                _startX = vm.InteractiveMap.ContainerLeft;
                _startY = vm.InteractiveMap.ContainerTop;
                AssociatedObject.CaptureMouse();
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && AssociatedObject.DataContext is MainWindowViewModel vm && AssociatedObject.Parent is UIElement canvas)
            {
                var pos = e.GetPosition(canvas);
                vm.InteractiveMap.ContainerLeft = _startX + (pos.X - _startMouse.X);
                vm.InteractiveMap.ContainerTop = _startY + (pos.Y - _startMouse.Y);
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            AssociatedObject.ReleaseMouseCapture();
        }
    }
}
