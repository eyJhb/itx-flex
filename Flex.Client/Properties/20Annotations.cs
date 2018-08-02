// Decompiled with JetBrains decompiler
// Type: Flex.Annotations.AspMvcControllerAttribute
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Flex.Annotations
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
  public sealed class AspMvcControllerAttribute : Attribute
  {
    public AspMvcControllerAttribute()
    {
    }

    public AspMvcControllerAttribute([NotNull] string anonymousProperty)
    {
      this.AnonymousProperty = anonymousProperty;
    }

    [CanBeNull]
    public string AnonymousProperty { get; private set; }
  }
}
