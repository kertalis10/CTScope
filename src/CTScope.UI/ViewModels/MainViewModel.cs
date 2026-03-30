using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CTScope.UI.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private string _statusText = "Ready";
    private string _selectedFolderPath = "No folder selected";
    private bool _isStudyLoaded;
    private int _sliceMin = 1;
    private int _sliceMax = 120;
    private int _currentSlice = 1;
    private string _viewerOverlayText = "Select a CT study folder to load mock slices.";

    public event PropertyChangedEventHandler? PropertyChanged;

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

    public string SelectedFolderPath
    {
        get => _selectedFolderPath;
        set
        {
            if (_selectedFolderPath == value)
            {
                return;
            }

            _selectedFolderPath = value;
            OnPropertyChanged();
        }
    }

    public bool IsStudyLoaded
    {
        get => _isStudyLoaded;
        set
        {
            if (_isStudyLoaded == value)
            {
                return;
            }

            _isStudyLoaded = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsSliceNavigationEnabled));
            OnPropertyChanged(nameof(SliceSummaryText));
        }
    }

    public bool IsSliceNavigationEnabled => IsStudyLoaded;

    public int SliceMin
    {
        get => _sliceMin;
        set
        {
            if (_sliceMin == value)
            {
                return;
            }

            _sliceMin = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SliceSummaryText));
        }
    }

    public int SliceMax
    {
        get => _sliceMax;
        set
        {
            if (_sliceMax == value)
            {
                return;
            }

            _sliceMax = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SliceSummaryText));
        }
    }

    public int CurrentSlice
    {
        get => _currentSlice;
        set
        {
            if (_currentSlice == value)
            {
                return;
            }

            _currentSlice = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CurrentSliceText));
            OnPropertyChanged(nameof(SliceSummaryText));
        }
    }

    public string CurrentSliceText => $"Slice {CurrentSlice}";

    public string SliceSummaryText => IsStudyLoaded
        ? $"Slice {CurrentSlice} / {SliceMax}"
        : "No mock study loaded";

    public string ViewerOverlayText
    {
        get => _viewerOverlayText;
        set
        {
            if (_viewerOverlayText == value)
            {
                return;
            }

            _viewerOverlayText = value;
            OnPropertyChanged();
        }
    }

    public void LoadMockStudy(string folderPath, int totalSlices)
    {
        SelectedFolderPath = folderPath;
        SliceMin = 1;
        SliceMax = totalSlices;
        CurrentSlice = SliceMin;
        IsStudyLoaded = true;
        ViewerOverlayText = $"Mock Slice View - Slice {CurrentSlice} of {SliceMax}";
        StatusText = "Folder selected. Mock study loaded.";
    }

    public void SetSlice(int sliceNumber)
    {
        CurrentSlice = sliceNumber;
        ViewerOverlayText = $"Mock Slice View - Slice {CurrentSlice} of {SliceMax}";
        StatusText = $"Displaying mock slice {CurrentSlice} of {SliceMax}";
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
