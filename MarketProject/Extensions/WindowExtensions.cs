using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace MarketProject.Extensions;

public static class WindowExtensions
{
   public static void ResponsiveWindow(this Window window)
   {
      Control child = window.Content as Control;
      window.Content = null;
      
      LayoutTransformControl layoutTransformControl = new()
      {
         Child = child
      };

      window.Content = layoutTransformControl;
      
      window.GetObservable(Window.ClientSizeProperty).Subscribe(size =>
      {
         double width = size.Width;
         double height = size.Height;
        
         double scalex = width / 1920;
         double scaley = height / 1080;
 
         var layoutTransform = window.Content as LayoutTransformControl;
         layoutTransform.LayoutTransform = new ScaleTransform(scalex, scaley);
      });
   }
}