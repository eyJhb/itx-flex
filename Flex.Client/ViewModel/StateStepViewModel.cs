// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.StateStepViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Model;
using Itx.Flex.Client.Service;
using System;

namespace Itx.Flex.Client.ViewModel
{
  public class StateStepViewModel : BaseViewModel, IStateStepViewModel
  {
    private readonly ILanguageService _languageService;
    private StateStep _stateStep;
    private bool _isLast;
    private string _stateStepDescriptionTextKey;

    public StateStepViewModel(ILanguageService languageService, ViewState viewState, bool isLast, string textKey)
    {
      this._languageService = languageService;
      this.ViewState = viewState;
      this.StateStepDescriptionTextKey = textKey;
      this.IsLast = isLast;
    }

    public StateStep StateStep
    {
      get
      {
        return this._stateStep;
      }
      set
      {
        this._stateStep = value;
        this.OnPropertyChanged(nameof (StateStep));
      }
    }

    public void UpdateState(StateStep newStateStep)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.StateStep = newStateStep));
    }

    public bool IsLast
    {
      get
      {
        return this._isLast;
      }
      set
      {
        this._isLast = value;
        this.OnPropertyChanged(nameof (IsLast));
        this.OnPropertyChanged("IsNotLast");
      }
    }

    public bool IsNotLast
    {
      get
      {
        return !this.IsLast;
      }
    }

    private string StateStepDescriptionTextKey
    {
      get
      {
        return this._stateStepDescriptionTextKey;
      }
      set
      {
        this._stateStepDescriptionTextKey = value;
        this.OnPropertyChanged(nameof (StateStepDescriptionTextKey));
        this.OnPropertyChanged("StateStepDescriptionText");
      }
    }

    public string StateStepDescriptionText
    {
      get
      {
        return this._languageService.GetString(this.StateStepDescriptionTextKey);
      }
    }

    public ViewState ViewState { get; }
  }
}
