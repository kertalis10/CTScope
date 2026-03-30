using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;

namespace CTScope.UI;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _viewModel;
    }

    private void OpenCtStudyFolder_Click(object sender, RoutedEventArgs e)
    {
        using var dialog = new WinForms.FolderBrowserDialog
        {
            Description = "Select the CT study folder",
            ShowNewFolderButton = false,
            UseDescriptionForTitle = true
        };

        var result = dialog.ShowDialog();
        if (result != WinForms.DialogResult.OK || string.IsNullOrWhiteSpace(dialog.SelectedPath))
        {
            _viewModel.StatusText = "Folder selection canceled.";
            return;
        }

        _viewModel.LoadMockStudy(dialog.SelectedPath);

        SliceSlider.Minimum = 1;
        SliceSlider.Maximum = _viewModel.TotalSlices;
        SliceSlider.Value = _viewModel.CurrentSlice;
    }

    private void SliceSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (!_viewModel.IsStudyLoaded)
        {
            return;
        }

        var selectedSlice = (int)e.NewValue;
        if (selectedSlice == _viewModel.CurrentSlice)
        {
            return;
        }

        _viewModel.SetSlice(selectedSlice);
    }
}
