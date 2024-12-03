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
using TransStarter_Task.WPFApplication.Common.Models;
using TransStarter_Task.WPFApplication.Modules.MainApp.Model;

namespace TransStarter_Task.WPFApplication.Modules.MainApp.Components
{
    /// <summary>
    /// Логика взаимодействия для AnnuarySalesTable.xaml
    /// </summary>
    public partial class AnnualSalesTable : UserControl
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            nameof(Data), typeof(List<List<TableCellDTO>>), typeof(AnnualSalesTable), new PropertyMetadata(default(List<List<TableCellDTO>>)));

        public List<TableCellDTO> Data
        {
            get => (List<TableCellDTO>) GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public AnnualSalesTable ()
        {
            InitializeComponent();
        }
    }
}
