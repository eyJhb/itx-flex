// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.BoardingPassValidator
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

namespace Itx.Flex.Client.Service
{
  public class BoardingPassValidator : IBoardingPassValidator
  {
    public ValidatorResult Validate(string boardingPass)
    {
      if (string.IsNullOrEmpty(boardingPass))
        return ValidatorResult.CreateInvalid("LoginBoardingPassEmptyErrorText");
      if (boardingPass.Length != 6 || boardingPass.Length == 6 && boardingPass.Contains(" "))
        return ValidatorResult.CreateInvalid("LoginBoardingPassIncorrectNumberOfDigitsErrorText");
      return ValidatorResult.CreateValid();
    }
  }
}
