using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;

namespace MarketProject.Extensions;

public static class WindowExtensions
{
    public static void ResponsiveWindow(this Window window, bool useMainWindowSize = false)
    {
        Control child = window.Content as Control;
        window.Content = null;

        LayoutTransformControl layoutTransformControl = new()
        {
            Child = child
        };

        window.Content = layoutTransformControl;

        ((Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime).MainWindow ?? window)
            .GetObservable(Window.ClientSizeProperty).Subscribe(size =>
        {
            var currentScreen = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)
                .MainWindow;
            var screenSize = currentScreen?.Screens.Primary.Bounds.Size;
            double width = useMainWindowSize ? size.Width : screenSize?.Width ?? size.Width;
            double height = useMainWindowSize ? size.Height : screenSize?.Height ?? size.Height;

            double scalex = width / 1920;
            double scaley = height / 1080;

            var layoutTransform = window.Content as LayoutTransformControl;
            
            layoutTransform.LayoutTransform = new ScaleTransform(scalex, scaley);
        });
    }
}