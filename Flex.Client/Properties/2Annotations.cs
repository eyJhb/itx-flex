// Decompiled with JetBrains decompiler
// Type: Flex.Annotations.NotifyPropertyChangedInvocatorAttribute
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Flex.Annotations
{
  [AttributeUsage(AttributeTargets.Method)]
  public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
  {
    public NotifyPropertyChangedInvocatorAttribute()
    {
    }

    public NotifyPropertyChangedInvocatorAttribute([NotNull] string parameterName)
    {
      this.ParameterName = parameterName;
    }

    [CanBeNull]
    public string ParameterName { get; private set; }
  }
}
