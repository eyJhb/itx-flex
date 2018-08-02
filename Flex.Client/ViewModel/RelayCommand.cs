// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.RelayCommand
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.Windows.Input;

namespace Itx.Flex.Client.ViewModel
{
  public class RelayCommand : ICommand
  {
    private readonly Action<object> _execute;
    private readonly Predicate<object> _canExecute;

    public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
    {
      if (execute == null)
        throw new ArgumentNullException(nameof (execute));
      this._execute = execute;
      this._canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
      if (this._canExecute != null)
        return this._canExecute(parameter);
      return true;
    }

    public event EventHandler CanExecuteChanged
    {
      add
      {
        CommandManager.RequerySuggested += value;
      }
      remove
      {
        CommandManager.RequerySuggested -= value;
      }
    }

    public void Execute(object parameter)
    {
      this._execute(parameter ?? (object) "<N/A>");
    }
  }
}
