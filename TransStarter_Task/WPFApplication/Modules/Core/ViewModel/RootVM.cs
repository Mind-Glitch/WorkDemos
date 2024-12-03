using System.Windows.Controls;
using TransStarter_Task.WPFApplication.Modules.Core.View;
using TransStarter_Task.WPFApplication.Modules.MainApp.View;

namespace TransStarter_Task.WPFApplication.Modules.Core.ViewModel;
public class RootVM : VMBase
{
    public RootVM () 
        : base("Корневой компонент")
    {
        List<UserControl> views =
        [
            new PopulateDBView(),
            new SalesTablesView()
        ];

        CurrentView = views.ElementAt(0);

         NavView = new NavView(new NavVM(views, UpdateCurrentView));
    }

    private void UpdateCurrentView(UserControl? obj)
    {
        CurrentView = obj;
    }

    public NavView NavView { get; }


    private object? _currentView;
    public object? CurrentView
    {
        get => _currentView;
        set => _ = SetField(ref _currentView, value);
    }
}