// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Control.PropagateMouseWheelBehaviorUpwards
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Itx.Flex.Client.Control
{
  public sealed class PropagateMouseWheelBehaviorUpwards : Behavior<UIElement>
  {
    protected override void OnAttached()
    {
      base.OnAttached();
      this.AssociatedObject.PreviewMouseWheel += new MouseWheelEventHandler(this.AssociatedObject_PreviewMouseWheel);
    }

    protected override void OnDetaching()
    {
      this.AssociatedObject.PreviewMouseWheel -= new MouseWheelEventHandler(this.AssociatedObject_PreviewMouseWheel);
      base.OnDetaching();
    }

    private void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
      e.Handled = true;
      MouseWheelEventArgs mouseWheelEventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
      mouseWheelEventArgs.RoutedEvent = UIElement.MouseWheelEvent;
      if (!(this.AssociatedObject is FrameworkElement))
        return;
      FrameworkElement associatedObject = (FrameworkElement) this.AssociatedObject;
      if (!(associatedObject.Parent is UIElement))
        return;
      ((UIElement) associatedObject.Parent).RaiseEvent((RoutedEventArgs) mouseWheelEventArgs);
    }
  }
}
