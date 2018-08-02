// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Properties.Resources
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Itx.Flex.Client.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Itx.Flex.Client.Properties.Resources.resourceMan == null)
          Itx.Flex.Client.Properties.Resources.resourceMan = new ResourceManager("Itx.Flex.Client.Properties.Resources", typeof (Itx.Flex.Client.Properties.Resources).Assembly);
        return Itx.Flex.Client.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return Itx.Flex.Client.Properties.Resources.resourceCulture;
      }
      set
      {
        Itx.Flex.Client.Properties.Resources.resourceCulture = value;
      }
    }

    internal static Icon Icon
    {
      get
      {
        return (Icon) Itx.Flex.Client.Properties.Resources.ResourceManager.GetObject(nameof (Icon), Itx.Flex.Client.Properties.Resources.resourceCulture);
      }
    }
  }
}
