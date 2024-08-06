using System.Collections.Generic;
using Avalonia.Interactivity;
using MarketProject.Models;

namespace MarketProject.ViewModels;

public class ProdRegisterViewModel : ViewModelBase
{
    private string _msboxcontent;
    public string MsBoxContent
    {
        get => _msboxcontent;
        set
        {
            if (!string.IsNullOrEmpty(value))
                _msboxcontent = value;
            else
                _msboxcontent = "Campo Vazio!";
        }
    }
    public bool ButtonOkOnly { get; set; }
    public bool ButtonYesNo { get; set; }
}