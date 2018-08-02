// Decompiled with JetBrains decompiler
// Type: Flex.Annotations.StringFormatMethodAttribute
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Flex.Annotations
{
  [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Delegate)]
  public sealed class StringFormatMethodAttribute : Attribute
  {
    public StringFormatMethodAttribute([NotNull] string formatParameterName)
    {
      this.FormatParameterName = formatParameterName;
    }

    [NotNull]
    public string FormatParameterName { get; private set; }
  }
}
