using System.Collections.Generic;
using Avalonia.Interactivity;
using MarketProject.Models;
using ReactiveUI;

namespace MarketProject.ViewModels;

public class ProdRegisterViewModel : ReactiveObject
{
    public DataAnnotationErrors dataErros { get; } = new();
}