using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.VisualBasic.CompilerServices;
using TransStarter_Task.WPFApplication.API;
using TransStarter_Task.WPFApplication.Modules.Core.Components;
using TransStarter_Task.WPFApplication.Modules.Core.Model;
using TransStarter_Task.WPFApplication.Modules.Core.ViewModel;
using TransStarter_Task.WPFApplication.Persistance.Scripts;

namespace TransStarter_Task.WPFApplication.Modules.MainApp.ViewModel;
internal class PopulateDBVM : VMBase
{
    public PopulateDBVM (LoadingController controller)
        : base("Заполнение БД")
    {
        _controller = controller;

        StartFillingCommand = new ButtonCommand(OnStartFillingButtonClick);
    }

    private bool _fillingAvailable = true;
    public bool IsFillingAvailable
    {
        get => _fillingAvailable;
        set => _ = SetField(ref _fillingAvailable, value);
    }

    private object? _loadingView;
    public object? LoadingView
    {
        get => _loadingView;
        set => _ = SetField(ref _loadingView, value);
    }

    private readonly LoadingController _controller;

    public ICommand StartFillingCommand { get; }

    private void OnStartFillingButtonClick (object? obj)
    {
        IsFillingAvailable = false;
        LoadingView = new Loading(_controller);
        Task.Run(async () =>
        {
            await FillingScripts.Regenerate(20, 1000, TimeSpan.FromDays(365 * 5),
                _controller);
            LoadingView = null;
            IsFillingAvailable = true;
        });

    }
}