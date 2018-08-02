// Decompiled with JetBrains decompiler
// Type: Flex.Annotations.BaseTypeRequiredAttribute
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Flex.Annotations
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  [BaseTypeRequired(typeof (Attribute))]
  public sealed class BaseTypeRequiredAttribute : Attribute
  {
    public BaseTypeRequiredAttribute([NotNull] Type baseType)
    {
      this.BaseType = baseType;
    }

    [NotNull]
    public Type BaseType { get; private set; }
  }
}
