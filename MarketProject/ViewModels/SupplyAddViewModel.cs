using System;
using System.Collections;
using System.ComponentModel;

namespace MarketProject.ViewModels;

public class SupplyAddViewModel : ViewModelBase, INotifyDataErrorInfo
{
    public IEnumerable GetErrors(string propertyName)
    {
        throw new NotImplementedException();
    }

    public bool HasErrors { get; }
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
}