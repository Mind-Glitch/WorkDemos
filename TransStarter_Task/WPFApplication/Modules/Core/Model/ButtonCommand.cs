using System.Windows.Input;
using DocumentFormat.OpenXml.Wordprocessing;

namespace TransStarter_Task.WPFApplication.Modules.Core.Model;
internal class ButtonCommand : ICommand
{
    public ButtonCommand (Action<object?> command, Func<object?, bool>? canExecuteCheck)
    {
        _command = command;
        _canExecuteCheck = canExecuteCheck;
    }

    public ButtonCommand (Action<object?> command)
    {
        _command = command;
        _canExecuteCheck = null;
    }

    private readonly Action<object?> _command;
    private readonly Func<object?, bool>? _canExecuteCheck;

    /// <summary>
    /// pass bool as parameter
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool CanExecute (object? parameter)
    {
        var val = _canExecuteCheck == null || _canExecuteCheck(parameter);
        return _canExecuteCheck == null || _canExecuteCheck(parameter);
    }

    public void Execute (object? parameter)
    {
        // убирает варнинг, который меня раздражает.
        CanExecuteChanged?.Invoke(null, EventArgs.Empty);
        _command.Invoke(parameter);
    }

    public event EventHandler? CanExecuteChanged;
}