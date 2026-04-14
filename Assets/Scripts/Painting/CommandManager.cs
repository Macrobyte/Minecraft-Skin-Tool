using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandManager
{
    private Stack<ICommand> _undoStack;
    private Stack<ICommand> _redoStack;
    private int _maxHistory;

    public CommandManager(int maxHistory)
    {
        _undoStack = new Stack<ICommand>();
        _redoStack = new Stack<ICommand>();
        _maxHistory = maxHistory;
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        _undoStack.Push(command);
        
        _redoStack.Clear();

        if (_undoStack.Count > _maxHistory)
        {
            _undoStack = new Stack<ICommand>(_undoStack.Take(_maxHistory));
        }
    }

    public void Undo()
    {
        if (_undoStack.Count > 0)
        {
            ICommand command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
        }
    }

    public void Redo()
    {
        if (_redoStack.Count > 0)
        {
            ICommand command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
        }
    }
}
