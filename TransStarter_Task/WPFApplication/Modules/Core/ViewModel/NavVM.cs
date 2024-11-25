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

            SellTablesButton = new ButtonCommand(OnPressSellTablesButton);
            PopulateDBButton = new ButtonCommand(OnPressPopulateDBButton);
            CloseButton = new ButtonCommand(OnPressCloseButton);
        }

        private readonly Action<UserControl?> UpdateCurrentView;


        public List<UserControl> ViewModels { get; }

        public ICommand SellTablesButton { get; }
        public ICommand PopulateDBButton { get; }
        public ICommand CloseButton { get; }

        private void OnPressSellTablesButton (object? obj)
        {
            /*if ( obj == null )
                return;*/
            //todo: проверить
            var view = ViewModels.Find(x => x.GetType() == typeof(SellTablesView));
            UpdateCurrentView.Invoke(view);
        }

        private void OnPressPopulateDBButton (object? obj)
        {
            /*if ( obj == null )
                return;*/
            UpdateCurrentView.Invoke(ViewModels.Find(x=>x.GetType() == typeof(PopulateDBView)));
        }

        private static void OnPressCloseButton (object? obj) =>
            System.Windows.Application.Current.Shutdown();
        
    }
}
