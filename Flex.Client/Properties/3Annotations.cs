// Decompiled with JetBrains decompiler
// Type: Flex.Annotations.ContractAnnotationAttribute
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Flex.Annotations
{
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
  public sealed class ContractAnnotationAttribute : Attribute
  {
    public ContractAnnotationAttribute([NotNull] string contract)
      : this(contract, false)
    {
    }

    public ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
    {
      this.Contract = contract;
      this.ForceFullStates = forceFullStates;
    }

    [NotNull]
    public string Contract { get; private set; }

    public bool ForceFullStates { get; private set; }
  }
}
