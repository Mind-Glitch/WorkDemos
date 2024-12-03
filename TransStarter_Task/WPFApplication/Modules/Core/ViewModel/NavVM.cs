using System.Windows.Controls;
using System.Windows.Input;
using TransStarter_Task.WPFApplication.Modules.Core.Model;
using TransStarter_Task.WPFApplication.Modules.MainApp.View;

namespace TransStarter_Task.WPFApplication.Modules.Core.ViewModel
{
    public class NavVM : VMBase
    {
        public NavVM (List<UserControl> viewModels, Action<UserControl?> updateCurrentView) 
            : base("Панель навигации")
        {
            UpdateCurrentView = updateCurrentView;
            ViewModels = viewModels;

            SalesTablesButton = new ButtonCommand(OnPressSalesTablesButton);
            PopulateDBButton = new ButtonCommand(OnPressPopulateDBButton);
            //CloseButton = new ButtonCommand(OnPressCloseButton);
        }

        private readonly Action<UserControl?> UpdateCurrentView;


        public List<UserControl> ViewModels { get; }

        public ICommand SalesTablesButton { get; }
        public ICommand PopulateDBButton { get; }
        //public ICommand CloseButton { get; }

        private void OnPressSalesTablesButton (object? obj)
        {
            var view = ViewModels.Find(x => x.GetType() == typeof(SalesTablesView));
            UpdateCurrentView.Invoke(view);
        }

        private void OnPressPopulateDBButton (object? obj)
        {
            UpdateCurrentView.Invoke(ViewModels.Find(x=>x.GetType() == typeof(PopulateDBView)));
        }

        private static void OnPressCloseButton (object? obj) =>
            System.Windows.Application.Current.Shutdown();
        
    }
}
