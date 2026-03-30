using System.Windows;
using System.Windows.Controls;
using CTScope.Dicom.Models;
using CTScope.UI.ViewModels;
using Forms = System.Windows.Forms;

namespace CTScope.UI;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainViewModel();
        DataContext = _viewModel;
    }

    private void OpenCtStudyFolder_Click(object sender, RoutedEventArgs e)
    {
        using var folderDialog = new Forms.FolderBrowserDialog
        {
            Description = "Select CT study folder",
            UseDescriptionForTitle = true,
            ShowNewFolderButton = false
        };

        var dialogResult = folderDialog.ShowDialog();
        if (dialogResult == Forms.DialogResult.OK)
        {
            _viewModel.ScanFolder(folderDialog.SelectedPath);
        }
        else
        {
            _viewModel.StatusText = "Folder selection canceled.";
        }
    }

    private void SliceSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (!_viewModel.IsStudyLoaded)
        {
            return;
        }

        var selectedSlice = (int)e.NewValue;
        _viewModel.SetSlice(selectedSlice);
    }

    private void StudyTree_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is DicomSeriesInfo series)
        {
            _viewModel.SelectedSeries = series;
        }
    }
}
