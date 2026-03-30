using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CTScope.UI.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private string _outputText = "Select a CT study folder to begin.";
    private string _statusText = "Ready";

    public event PropertyChangedEventHandler? PropertyChanged;

    public string OutputText
    {
        get => _outputText;
        set
        {
            if (_outputText == value)
            {
                return;
            }

            _outputText = value;
            OnPropertyChanged();
        }
    }

    public string StatusText
    {
        get => _statusText;
        set
        {
            if (_statusText == value)
            {
                return;
            }

            _statusText = value;
            OnPropertyChanged();
        }
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
