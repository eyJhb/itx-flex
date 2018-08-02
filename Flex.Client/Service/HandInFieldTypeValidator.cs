// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.HandInFieldTypeValidator
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Itx.Flex.Client.Model;
using System.Text.RegularExpressions;

namespace Itx.Flex.Client.Service
{
  public class HandInFieldTypeValidator : IHandInFieldTypeValidator
  {
    private readonly string _regexValidation;

    public HandInFieldValueType HandInFieldValueType { get; }

    public HandInFieldTypeValidator(string regexValidation, HandInFieldValueType handInFieldValueType)
    {
      this.HandInFieldValueType = handInFieldValueType;
      this._regexValidation = regexValidation;
    }

    public ValidatorResult Validate(string value)
    {
      if (string.IsNullOrEmpty(value))
        return ValidatorResult.CreateInvalid(string.Format("HandInField{0}ValidatorInvalidErrorText", (object) this.HandInFieldValueType));
      if (!Regex.Match(value.Trim(), this._regexValidation).Success)
        return ValidatorResult.CreateInvalid(string.Format("HandInField{0}ValidatorInvalidErrorText", (object) this.HandInFieldValueType));
      return ValidatorResult.CreateValid();
    }
  }
}
