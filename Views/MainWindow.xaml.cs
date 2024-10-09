using MvvmToolkitDemo01.ViewModels;
using System.Windows;

namespace MvvmToolkitDemo01.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVM();

        }
    }
}
