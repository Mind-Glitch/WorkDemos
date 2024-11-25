using System.Windows;
using TransStarter_Task.WPFApplication.Modules.Core.View;
using TransStarter_Task.WPFApplication.Modules.Core.ViewModel;

namespace TransStarter_Task
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow ()
        {
            DataContext = new Root();
            InitializeComponent();
        }
    }
}