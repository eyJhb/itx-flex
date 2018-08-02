// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.PinCodeValidator
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

namespace Itx.Flex.Client.Service
{
  public class PinCodeValidator : IPinCodeValidator
  {
    public ValidatorResult Validate(string pinCode)
    {
      if (string.IsNullOrEmpty(pinCode))
        return ValidatorResult.CreateInvalid("WorkspacePinCodeEmptyErrorText");
      int result;
      if (!int.TryParse(pinCode, out result))
        return ValidatorResult.CreateInvalid("WorkspacePinCodeOnlyNumbersAllowedErrorText");
      if (pinCode.Length != 5 || pinCode.Length == 5 && pinCode.Contains(" "))
        return ValidatorResult.CreateInvalid("WorkspacePinCodeIncorrectNumberOfDigitsErrorText");
      return ValidatorResult.CreateValid();
    }
  }
}
