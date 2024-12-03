using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
using TransStarter_Task.WPFApplication.API.Xlsx;
using TransStarter_Task.WPFApplication.Common.Models;
using TransStarter_Task.WPFApplication.Modules.MainApp.Model;

namespace TransStarter_Task.WPFApplication.Modules.MainApp.Components
{
    /// <summary>
    /// Логика взаимодействия для SalesTablesList.xaml
    /// </summary>
    public partial class SalesTablesList : UserControl
    {
        public SalesTablesList ()
        {
            InitializeComponent();
        }
    }
}
