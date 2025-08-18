// 1. RelayCommand 클래스 수정 (null 허용 문제 해결)

using System.Windows.Input;

public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;

    // 매개변수 없는 생성자
    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = _ => execute();
        _canExecute = canExecute != null ? _ => canExecute() : null;
    }

    // 매개변수 있는 생성자  
    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

    public void Execute(object? parameter) => _execute(parameter);
}