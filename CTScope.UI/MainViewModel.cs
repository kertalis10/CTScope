using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CTScope.UI;

public sealed class MainViewModel : INotifyPropertyChanged
{
    private const int DefaultTotalSlices = 120;

    private string _statusText = "Ready";
    private string _selectedFolderPath = "No folder selected";
    private int _currentSlice = 1;
    private int _totalSlices = DefaultTotalSlices;
    private bool _isStudyLoaded;

    public event PropertyChangedEventHandler? PropertyChanged;

    public string StatusText
    {
        get => _statusText;
        set => SetField(ref _statusText, value);
    }

    public string SelectedFolderPath
    {
        get => _selectedFolderPath;
        set => SetField(ref _selectedFolderPath, value);
    }

    public int CurrentSlice
    {
        get => _currentSlice;
        set
        {
            if (!SetField(ref _currentSlice, value))
            {
                return;
            }

            OnPropertyChanged(nameof(CurrentSliceDisplay));
            OnPropertyChanged(nameof(ViewerOverlayText));
        }
    }

    public int TotalSlices
    {
        get => _totalSlices;
        set
        {
            if (!SetField(ref _totalSlices, value))
            {
                return;
            }

            OnPropertyChanged(nameof(CurrentSliceDisplay));
            OnPropertyChanged(nameof(ViewerOverlayText));
        }
    }

    public bool IsStudyLoaded
    {
        get => _isStudyLoaded;
        set
        {
            if (!SetField(ref _isStudyLoaded, value))
            {
                return;
            }

            OnPropertyChanged(nameof(ViewerOverlayText));
        }
    }

    public string CurrentSliceDisplay => $"Slice {CurrentSlice} / {TotalSlices}";

    public string ViewerOverlayText => IsStudyLoaded
        ? $"Mock Slice View\nSlice {CurrentSlice} of {TotalSlices}"
        : "No study loaded\nChoose a CT folder to begin";

    public void LoadMockStudy(string folderPath)
    {
        SelectedFolderPath = folderPath;
        TotalSlices = DefaultTotalSlices;
        CurrentSlice = 1;
        IsStudyLoaded = true;
        StatusText = "Folder selected. Mock study loaded.";
    }

    public void SetSlice(int slice)
    {
        CurrentSlice = slice;
        StatusText = $"Displaying mock slice {CurrentSlice} of {TotalSlices}";
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
