using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CTScope.Dicom.Models;
using CTScope.Dicom.Readers;

namespace CTScope.UI.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly DicomStudyReader _dicomStudyReader = new();

    private string _statusText = "Ready";
    private string _selectedFolderPath = "No folder selected";
    private bool _isStudyLoaded;
    private int _sliceMin = 1;
    private int _sliceMax = 120;
    private int _currentSlice = 1;
    private string _viewerOverlayText = "Select a CT study folder to scan for DICOM studies.";
    private string _outputText = "No study loaded.";
    private DicomSeriesInfo? _selectedSeries;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<DicomStudyInfo> Studies { get; } = new();

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
        : "No series selected";

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

    public DicomSeriesInfo? SelectedSeries
    {
        get => _selectedSeries;
        set
        {
            if (_selectedSeries == value)
            {
                return;
            }

            _selectedSeries = value;
            OnPropertyChanged();
            UpdateSeriesSelectionState();
        }
    }

    public void ScanFolder(string folderPath)
    {
        SelectedFolderPath = folderPath;
        StatusText = "Scanning folder...";
        ViewerOverlayText = "Scanning DICOM files...";

        var scanResult = _dicomStudyReader.ScanFolder(folderPath);

        Studies.Clear();
        foreach (var study in scanResult.Studies)
        {
            Studies.Add(study);
        }

        SelectedSeries = null;
        IsStudyLoaded = false;

        var seriesCount = scanResult.Studies.Sum(study => study.Series.Count);
        var scanSummary = $"Scanned {scanResult.TotalFilesScanned} files. Opened {scanResult.DicomFilesOpened} DICOM files.";

        if (scanResult.Studies.Count == 0)
        {
            StatusText = $"{scanSummary} No DICOM studies found.";
            ViewerOverlayText = "No DICOM studies found.";
        }
        else
        {
            StatusText = $"{scanSummary} Found {scanResult.Studies.Count} studies and {seriesCount} series.";
            ViewerOverlayText = "Select a series from the discovered studies panel.";
        }

        OutputText = BuildOutput(scanResult, scanSummary, seriesCount);
    }

    public void SetSlice(int sliceNumber)
    {
        CurrentSlice = sliceNumber;
        if (SelectedSeries is null)
        {
            return;
        }

        ViewerOverlayText = $"Selected series: {SelectedSeries.DisplayName}";
        StatusText = $"Series selected. Slice UI set to {CurrentSlice}.";
    }

    private void UpdateSeriesSelectionState()
    {
        if (SelectedSeries is null)
        {
            SliceMin = 1;
            SliceMax = 1;
            CurrentSlice = 1;
            return;
        }

        IsStudyLoaded = true;
        SliceMin = 1;
        SliceMax = Math.Max(1, SelectedSeries.FileCount);
        CurrentSlice = SliceMin;

        var sizePart = SelectedSeries.Rows.HasValue && SelectedSeries.Columns.HasValue
            ? $"{SelectedSeries.Columns}x{SelectedSeries.Rows}"
            : "unknown size";

        ViewerOverlayText = $"Series selected: {SelectedSeries.DisplayName}";
        StatusText = $"Selected series with {SelectedSeries.FileCount} files ({sizePart}).";
    }

    private static string BuildOutput(DicomFolderScanResult scanResult, string scanSummary, int seriesCount)
    {
        var lines = new List<string>
        {
            "Scanning folder...",
            scanSummary,
            $"Found {scanResult.Studies.Count} studies and {seriesCount} series."
        };

        lines.AddRange(scanResult.Messages.Take(20));
        if (scanResult.Messages.Count > 20)
        {
            lines.Add($"...and {scanResult.Messages.Count - 20} more messages.");
        }

        return string.Join(Environment.NewLine, lines);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
