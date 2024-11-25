using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TransStarter_Task.WPFApplication.Modules.MainApp.ViewModel;

namespace TransStarter_Task.WPFApplication.Modules.MainApp.View
{
    /// <summary>
    /// Логика взаимодействия для SellTables.xaml
    /// </summary>
    public partial class SellTablesView : UserControl
    {
        public SellTablesView ()
        {
            DataContext = new SellTablesVM();
            InitializeComponent();
        }
    }
}
