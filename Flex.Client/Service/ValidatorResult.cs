// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.ValidatorResult
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

namespace Itx.Flex.Client.Service
{
  public class ValidatorResult
  {
    public static ValidatorResult CreateValid()
    {
      return new ValidatorResult(true, (string) null);
    }

    public static ValidatorResult CreateInvalid(string errorMessageKey)
    {
      return new ValidatorResult(false, errorMessageKey);
    }

    private ValidatorResult(bool isValid, string errorMessageKey = null)
    {
      this.IsValid = isValid;
      this.ErrorMessageKey = errorMessageKey;
    }

    public string ErrorMessageKey { get; }

    public bool IsValid { get; }
  }
}
