// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Message.OnPinCodeLoginSuccessful
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;
using System.Collections.Generic;
using System.Linq;

namespace Itx.Flex.Client.Message
{
  public class OnPinCodeLoginSuccessful
  {
    public OnPinCodeLoginSuccessful(string pinCode, IEnumerable<AssignmentDecryptionKeyModel> assignmentDecryptionKeys, IEnumerable<AssignmentFileMetadata> redownloadedAssignmentFileMetadatas)
    {
      this.PinCode = pinCode;
      this.RedownloadedAssignmentFileMetadatas = redownloadedAssignmentFileMetadatas;
      this.AssignmentDecryptionKeys = (IEnumerable<AssignmentDecryptionKeyModel>) assignmentDecryptionKeys.ToList<AssignmentDecryptionKeyModel>();
    }

    public string PinCode { get; }

    public IEnumerable<AssignmentFileMetadata> RedownloadedAssignmentFileMetadatas { get; }

    public IEnumerable<AssignmentDecryptionKeyModel> AssignmentDecryptionKeys { get; }
  }
}
