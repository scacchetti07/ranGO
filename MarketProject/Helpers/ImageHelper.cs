using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace MarketProject.Helpers;

public static class ImageHelper
{
    public static Bitmap LoadFromResource(Uri resourceUri) 
        => new(AssetLoader.Open(resourceUri));
}