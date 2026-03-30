using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using CTScope.UI.ViewModels;

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
        using var folderDialog = new FolderBrowserDialog
        {
            Description = "Select CT study folder",
            UseDescriptionForTitle = true,
            ShowNewFolderButton = false
        };

        var dialogResult = folderDialog.ShowDialog();
        if (dialogResult == System.Windows.Forms.DialogResult.OK)
        {
            const int mockSliceCount = 120;
            _viewModel.LoadMockStudy(folderDialog.SelectedPath, mockSliceCount);
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
}
