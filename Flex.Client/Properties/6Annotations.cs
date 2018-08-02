// Decompiled with JetBrains decompiler
// Type: Flex.Annotations.UsedImplicitlyAttribute
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Flex.Annotations
{
  [AttributeUsage(AttributeTargets.All)]
  public sealed class UsedImplicitlyAttribute : Attribute
  {
    public UsedImplicitlyAttribute()
      : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
    {
    }

    public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags)
      : this(useKindFlags, ImplicitUseTargetFlags.Default)
    {
    }

    public UsedImplicitlyAttribute(ImplicitUseTargetFlags targetFlags)
      : this(ImplicitUseKindFlags.Default, targetFlags)
    {
    }

    public UsedImplicitlyAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
    {
      this.UseKindFlags = useKindFlags;
      this.TargetFlags = targetFlags;
    }

    public ImplicitUseKindFlags UseKindFlags { get; private set; }

    public ImplicitUseTargetFlags TargetFlags { get; private set; }
  }
}
