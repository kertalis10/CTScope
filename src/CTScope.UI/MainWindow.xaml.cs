using System.Windows;
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
        _viewModel.OutputText = "CT study folder selection is not implemented yet.";
        _viewModel.StatusText = "Ready";
    }
}
