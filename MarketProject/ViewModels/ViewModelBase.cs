using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace MarketProject.ViewModels;

public class ViewModelBase : ReactiveObject, INotifyDataErrorInfo
{  
    private readonly Dictionary<string, List<string>> _propertyErrors = new();
    public bool HasErrors => _propertyErrors.Count != 0;
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public IEnumerable GetErrors(string? propertyName)
        => _propertyErrors.GetValueOrDefault(propertyName, null);

    protected void AddError(string propertyName, string errorMessage)
    {
        if (!_propertyErrors.ContainsKey(propertyName))
            _propertyErrors.Add(propertyName, new List<string>());
        
        _propertyErrors[propertyName].Add(errorMessage);
        OnErrorsChanged(propertyName);
    }
    
    protected void RemoveError(string propertyName)
    {
        _propertyErrors.Remove(propertyName);
        OnErrorsChanged(propertyName);
    }

    protected void ClearErrors(string propertyName)
    {
        if (_propertyErrors.Remove(propertyName))
            OnErrorsChanged(propertyName);
    }
    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}