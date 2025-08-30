using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TarkovAssistant.App.Behaviors
{
    public class ActualSizeBehavior : Behavior<FrameworkElement>
    {
        public double ActualHeight
        {
            get { return (double)GetValue(ActualHeightProperty); }
            set { SetValue(ActualHeightProperty, value); }
        }

        public double ActualWidth
        {
            get { return (double)GetValue(ActualWidthProperty); }
            set { SetValue(ActualWidthProperty, value); }
        }

        public static readonly DependencyProperty ActualHeightProperty =
            DependencyProperty.Register(
                nameof(ActualHeight),
                typeof(double),
                typeof(ActualSizeBehavior),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ActualWidthProperty =
            DependencyProperty.Register(
                nameof(ActualWidth),
                typeof(double),
                typeof(ActualSizeBehavior),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SizeChanged += OnSizeChanged;
            ActualHeight = AssociatedObject.ActualHeight;
            ActualWidth = AssociatedObject.ActualWidth;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SizeChanged -= OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ActualHeight = e.NewSize.Height;
            ActualWidth = e.NewSize.Width;
        }
    }
}
