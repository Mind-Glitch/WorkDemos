using System.Windows.Input;

namespace TransStarter_Task.WPFApplication.Modules.Core.Model;
internal class ButtonCommand : ICommand
{
    public ButtonCommand (Action<object?> command, Func<object, bool>? canExecuteCheck)
    {
        _command = command;
        _canExecuteCheck = canExecuteCheck;
    }

    public ButtonCommand(Action<object?> command)
    {
        _command = command;
        _canExecuteCheck = null;
    }

    private readonly Action<object?> _command;
    private readonly Func<object, bool>? _canExecuteCheck;

    /// <summary>
    /// pass bool as parameter
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public bool CanExecute (object? parameter)
    {
        if (_canExecuteCheck == null)
            return true;
        return parameter != null && _canExecuteCheck.Invoke(parameter);
    }

    public void Execute (object? parameter)
    {
        _command.Invoke(parameter);
    }

    public event EventHandler? CanExecuteChanged;
}