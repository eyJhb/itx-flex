// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.HandInFieldViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.Model;
using Itx.Flex.Client.Service;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Itx.Flex.Client.ViewModel
{
  public class HandInFieldViewModel : BaseViewModel, IDataErrorInfo, IHandInFieldViewModel
  {
    private readonly ILanguageService _languageService;
    private readonly string _titleTextEnGb;
    private readonly IMessenger _messenger;
    private readonly string _descriptionTextDaDk;
    private readonly string _descriptionTextEnGb;
    private readonly string _titleTextDaDk;
    private readonly IHandInFieldTypeValidator _handInFieldTypeValidator;
    private string _handInFieldValue;
    private bool _showFieldTextboxError;

    public HandInFieldViewModel(IHandInFieldTypeValidator handInFieldTypeValidator, ILanguageService languageService, bool isRequired, string titleTextDaDk, string titleTextEnGb, Guid id, IMessenger messenger, string descriptionTextDaDk, string descriptionTextEnGb)
    {
      this._languageService = languageService;
      this._titleTextEnGb = titleTextEnGb;
      this._messenger = messenger;
      this._descriptionTextDaDk = descriptionTextDaDk;
      this._descriptionTextEnGb = descriptionTextEnGb;
      this._titleTextDaDk = titleTextDaDk;
      this.IsRequired = isRequired;
      this.Id = id;
      this._handInFieldTypeValidator = handInFieldTypeValidator;
      this.LostInputFocus = (ICommand) new RelayCommand(new Action<object>(this.LostInputFocusUpdate), (Predicate<object>) null);
    }

    public ICommand LostInputFocus { get; }

    private void LostInputFocusUpdate(object c)
    {
      this._messenger.Send<OnHandInFieldInputLostFocus>(new OnHandInFieldInputLostFocus());
    }

    public HandInFieldValueType HandInFieldValueType
    {
      get
      {
        return this._handInFieldTypeValidator.HandInFieldValueType;
      }
    }

    public bool IsRequired { get; }

    public Guid Id { get; }

    public string HandInFieldTitleText
    {
      get
      {
        return this._languageService.SelectString(this._titleTextDaDk, this._titleTextEnGb);
      }
    }

    public string HandInFieldDescriptionText
    {
      get
      {
        return this._languageService.SelectString(this._descriptionTextDaDk, this._descriptionTextEnGb);
      }
    }

    public string HandInFieldBoolYesText
    {
      get
      {
        return this._languageService.GetString(nameof (HandInFieldBoolYesText));
      }
    }

    public string HandInFieldBoolNoText
    {
      get
      {
        return this._languageService.GetString(nameof (HandInFieldBoolNoText));
      }
    }

    public string HandInFieldValue
    {
      get
      {
        return this._handInFieldValue;
      }
      set
      {
        this._handInFieldValue = value;
        this.OnPropertyChanged(nameof (HandInFieldValue));
      }
    }

    public string Value
    {
      get
      {
        if (string.IsNullOrEmpty(this.HandInFieldValue))
          return (string) null;
        return this.HandInFieldValue.Trim();
      }
    }

    private bool ShowFieldTextboxError
    {
      get
      {
        return this._showFieldTextboxError;
      }
      set
      {
        this._showFieldTextboxError = value;
        this.OnPropertyChanged(nameof (ShowFieldTextboxError));
      }
    }

    public string this[string columnName]
    {
      get
      {
        if (!(columnName == "HandInFieldValue"))
          return string.Empty;
        if (this.ShowFieldTextboxError)
          return this.GetErrorText(this.HandInFieldValue);
        this.ShowFieldTextboxError = true;
        return string.Empty;
      }
    }

    public string Error
    {
      get
      {
        return string.Empty;
      }
    }

    private string GetErrorText(string value)
    {
      if (this.IsRequired && value.IsNullOrWhitespace())
        return this._languageService.GetString("HandInFieldFieldRequiredErrorText");
      ValidatorResult validatorResult = this._handInFieldTypeValidator.Validate(value);
      if (!value.IsNullOrWhitespace() && !validatorResult.IsValid)
        return this._languageService.GetString(validatorResult.ErrorMessageKey);
      return string.Empty;
    }

    public void ResetErrorDisplay()
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.ShowFieldTextboxError = false));
    }

    public bool IsValid()
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.ShowFieldTextboxError = true;
        this.OnPropertyChanged("HandInFieldValue");
      }));
      return this.GetErrorText(this.HandInFieldValue) == string.Empty;
    }

    public void ResetValidation()
    {
      if (!this.HandInFieldValue.IsNullOrWhitespace())
        return;
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.ShowFieldTextboxError = false));
    }
  }
}
