// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.HashValidator
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Flex.WebserviceClient;
using System;
using System.IO;

namespace Itx.Flex.Client.Service
{
  public class HashValidator : IHashValidator
  {
    private readonly IHashProvider _hashProvider;

    public HashValidator(IHashProvider hashProvider)
    {
      this._hashProvider = hashProvider;
    }

    public bool IsValidHash(FileStream fileData, string fileHash)
    {
      try
      {
        return this._hashProvider.ComputeHashAsBase64((Stream) fileData).Equals(fileHash);
      }
      catch (Exception ex)
      {
        return false;
      }
    }
  }
}
