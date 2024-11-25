using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TransStarter_Task.WPFApplication.Modules.Core.ViewModel;
public abstract class VMBase : INotifyPropertyChanged
{
    public VMBase()
    {
        Name = string.Empty;
    }

    public VMBase(string name)
    {
        Name = name;
    }

    public string Name { get; }

    private void OnPropertyChanged ([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T> (ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if ( EqualityComparer<T>.Default.Equals(field, value) )
            return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public void UpdateProperty<T> ([CallerMemberName] string? propertyName = null) =>
        OnPropertyChanged(propertyName);
    

    public event PropertyChangedEventHandler? PropertyChanged;
}