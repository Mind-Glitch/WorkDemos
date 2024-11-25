using System.Diagnostics;
using System.Windows.Controls;
using TransStarter_Task.WPFApplication.API;

namespace TransStarter_Task.WPFApplication.Modules.Core.Components
{
    /// <summary>
    /// Логика взаимодействия для Loading.xaml
    /// </summary>
    public partial class Loading : UserControl
    {
        public Loading (LoadingController controller)
        {
            InitializeComponent();

            controller.OnStateChanged += Controller_OnStateChanged;
            controller.OperationNameChanged += Controller_OperationNameChanged;
        }

        private void Controller_OperationNameChanged (string obj)
        {
            Dispatcher.Invoke(() => Label_OperationName.Content = obj);
        }

        private void Controller_OnStateChanged (float obj)
        {
            Dispatcher.Invoke(() =>
            {
                Label_Percent.Content = $"{obj * 100:N2}%";
                Border_Loading.Width = obj * Border_Loading_Holder.ActualWidth;
            });
        }
    }
}
