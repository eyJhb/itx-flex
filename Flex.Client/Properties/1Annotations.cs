// Decompiled with JetBrains decompiler
// Type: Flex.Annotations.ValueProviderAttribute
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Flex.Annotations
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
  public sealed class ValueProviderAttribute : Attribute
  {
    public ValueProviderAttribute([NotNull] string name)
    {
      this.Name = name;
    }

    [NotNull]
    public string Name { get; private set; }
  }
}
