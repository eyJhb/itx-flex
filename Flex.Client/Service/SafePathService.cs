// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.SafePathService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Itx.Flex.Client.Service
{
  public class SafePathService : ISafePathService
  {
    private const int MaxExamNameLength = 30;
    private static char[] _invalids;

    public string MakeValidFilename(string text, char? replacement = '_')
    {
      int capacity = Math.Min(text.Length, 30);
      StringBuilder stringBuilder = new StringBuilder(capacity);
      char[] chArray = SafePathService._invalids ?? (SafePathService._invalids = Path.GetInvalidFileNameChars());
      for (int index = 0; index < capacity; ++index)
      {
        char ch1 = text[index];
        if (((IEnumerable<char>) chArray).Contains<char>(ch1))
        {
          char? nullable = replacement;
          char ch2 = nullable.HasValue ? nullable.GetValueOrDefault() : char.MinValue;
          if (ch2 != char.MinValue)
            stringBuilder.Append(ch2);
        }
        else
          stringBuilder.Append(ch1);
      }
      if (stringBuilder.Length != 0)
        return stringBuilder.ToString();
      if (!replacement.HasValue)
        return "_";
      return replacement.ToString();
    }
  }
}
