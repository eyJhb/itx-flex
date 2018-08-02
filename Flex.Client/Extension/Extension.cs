// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Extension.Extension
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using Itx.Flex.Client.Model;
using System;

namespace Itx.Flex.Client.Extension
{
  public static class Extension
  {
    public static bool IsNullOrWhitespace(this string value)
    {
      if (!string.IsNullOrEmpty(value))
        return value.Trim().Length == 0;
      return true;
    }

    public static HandInFieldValueType ToHandInFieldValueType(this HandinFieldType handinFieldType)
    {
      switch (handinFieldType)
      {
        case HandinFieldType.String:
          return HandInFieldValueType.StringValue;
        case HandinFieldType.Integer:
          return HandInFieldValueType.IntValue;
        case HandinFieldType.Boolean:
          return HandInFieldValueType.BoolValue;
        case HandinFieldType.Decimal:
          return HandInFieldValueType.DecimalValue;
        default:
          throw new ArgumentOutOfRangeException(nameof (handinFieldType), (object) handinFieldType, (string) null);
      }
    }

    public static Itx.Flex.Client.Model.HandInStatus ToHandInStatus(this Arcanic.ITX.Contracts.Flex.HandInStatus handInStatus)
    {
      switch (handInStatus)
      {
        case Arcanic.ITX.Contracts.Flex.HandInStatus.NotTagged:
          return Itx.Flex.Client.Model.HandInStatus.NotTagged;
        case Arcanic.ITX.Contracts.Flex.HandInStatus.Tagged:
          return Itx.Flex.Client.Model.HandInStatus.Tagged;
        case Arcanic.ITX.Contracts.Flex.HandInStatus.TaggedLate:
          return Itx.Flex.Client.Model.HandInStatus.TaggedLate;
        case Arcanic.ITX.Contracts.Flex.HandInStatus.TaggedExternally:
          return Itx.Flex.Client.Model.HandInStatus.TaggedExternally;
        case Arcanic.ITX.Contracts.Flex.HandInStatus.Uploaded:
          return Itx.Flex.Client.Model.HandInStatus.Uploaded;
        case Arcanic.ITX.Contracts.Flex.HandInStatus.UploadedLate:
          return Itx.Flex.Client.Model.HandInStatus.UploadedLate;
        default:
          throw new ArgumentOutOfRangeException(nameof (handInStatus), (object) handInStatus, (string) null);
      }
    }

    public static Itx.Flex.Client.Model.HandInType ToHandInType(this Arcanic.ITX.Contracts.Flex.HandInType handInType)
    {
      switch (handInType)
      {
        case Arcanic.ITX.Contracts.Flex.HandInType.NoSubmission:
          return Itx.Flex.Client.Model.HandInType.NoSubmission;
        case Arcanic.ITX.Contracts.Flex.HandInType.Handin:
          return Itx.Flex.Client.Model.HandInType.HandIn;
        case Arcanic.ITX.Contracts.Flex.HandInType.Blank:
          return Itx.Flex.Client.Model.HandInType.Blank;
        default:
          throw new ArgumentOutOfRangeException(nameof (handInType), (object) handInType, (string) null);
      }
    }

    public static bool HasReceivedSubmission(this Itx.Flex.Client.Model.HandInType handInType)
    {
      switch (handInType)
      {
        case Itx.Flex.Client.Model.HandInType.NoSubmission:
          return false;
        case Itx.Flex.Client.Model.HandInType.HandIn:
        case Itx.Flex.Client.Model.HandInType.Blank:
          return true;
        default:
          throw new ArgumentOutOfRangeException(nameof (handInType), (object) handInType, (string) null);
      }
    }

    public static bool HasTagged(this Itx.Flex.Client.Model.HandInStatus handInStatus)
    {
      switch (handInStatus)
      {
        case Itx.Flex.Client.Model.HandInStatus.NotTagged:
        case Itx.Flex.Client.Model.HandInStatus.TaggedExternally:
        case Itx.Flex.Client.Model.HandInStatus.Uploaded:
        case Itx.Flex.Client.Model.HandInStatus.UploadedLate:
          return false;
        case Itx.Flex.Client.Model.HandInStatus.Tagged:
        case Itx.Flex.Client.Model.HandInStatus.TaggedLate:
          return true;
        default:
          throw new ArgumentOutOfRangeException(nameof (handInStatus), (object) handInStatus, (string) null);
      }
    }

    public static bool HasUploaded(this Itx.Flex.Client.Model.HandInStatus handInStatus)
    {
      switch (handInStatus)
      {
        case Itx.Flex.Client.Model.HandInStatus.NotTagged:
        case Itx.Flex.Client.Model.HandInStatus.Tagged:
        case Itx.Flex.Client.Model.HandInStatus.TaggedLate:
        case Itx.Flex.Client.Model.HandInStatus.TaggedExternally:
          return false;
        case Itx.Flex.Client.Model.HandInStatus.Uploaded:
        case Itx.Flex.Client.Model.HandInStatus.UploadedLate:
          return true;
        default:
          throw new ArgumentOutOfRangeException(nameof (handInStatus), (object) handInStatus, (string) null);
      }
    }
  }
}
