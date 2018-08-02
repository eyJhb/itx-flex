// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Model.AssignmentDecryptionKeyModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

namespace Itx.Flex.Client.Model
{
  public class AssignmentDecryptionKeyModel
  {
    public AssignmentDecryptionKeyModel(string filename, string ciphertextHash, string key, string initializationVector, string cleartextHash)
    {
      this.Filename = filename;
      this.CiphertextHash = ciphertextHash;
      this.Key = key;
      this.InitializationVector = initializationVector;
      this.CleartextHash = cleartextHash;
    }

    public string Filename { get; }

    public string CiphertextHash { get; }

    public string Key { get; }

    public string InitializationVector { get; }

    public string CleartextHash { get; }
  }
}
