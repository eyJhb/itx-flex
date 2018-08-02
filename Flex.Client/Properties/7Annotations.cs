// Decompiled with JetBrains decompiler
// Type: Flex.Annotations.MeansImplicitUseAttribute
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;

namespace Flex.Annotations
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.GenericParameter)]
  public sealed class MeansImplicitUseAttribute : Attribute
  {
    public MeansImplicitUseAttribute()
      : this(ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default)
    {
    }

    public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags)
      : this(useKindFlags, ImplicitUseTargetFlags.Default)
    {
    }

    public MeansImplicitUseAttribute(ImplicitUseTargetFlags targetFlags)
      : this(ImplicitUseKindFlags.Default, targetFlags)
    {
    }

    public MeansImplicitUseAttribute(ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags)
    {
      this.UseKindFlags = useKindFlags;
      this.TargetFlags = targetFlags;
    }

    [UsedImplicitly]
    public ImplicitUseKindFlags UseKindFlags { get; private set; }

    [UsedImplicitly]
    public ImplicitUseTargetFlags TargetFlags { get; private set; }
  }
}
