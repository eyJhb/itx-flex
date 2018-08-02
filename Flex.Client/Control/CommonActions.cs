// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Control.CommonActions
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Itx.Flex.Client.Control
{
  public class CommonActions : TriggerAction<FrameworkElement>
  {
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (string), typeof (CommonActions));
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (string), typeof (CommonActions));

    public string Command
    {
      get
      {
        return this.GetValue(CommonActions.CommandProperty) as string;
      }
      set
      {
        this.SetValue(CommonActions.CommandProperty, (object) value);
      }
    }

    public string CommandParameter
    {
      get
      {
        return this.GetValue(CommonActions.CommandParameterProperty) as string;
      }
      set
      {
        this.SetValue(CommonActions.CommandParameterProperty, (object) value);
      }
    }

    protected override void Invoke(object param)
    {
      if (this.Command == null)
        return;
      object dataContext = this.AssociatedObject.DataContext;
      if (dataContext == null)
        return;
      ICommand command = dataContext.GetType().GetProperty(this.Command).GetValue(dataContext, (object[]) null) as ICommand;
      object parameter = this.AssociatedObject.DataContext.GetType().GetProperty(this.CommandParameter).GetValue(this.AssociatedObject.DataContext, (object[]) null);
      if (command == null || !command.CanExecute(parameter))
        return;
      command.Execute(parameter);
    }
  }
}
