// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.StateViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Model;
using Itx.Flex.Client.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Itx.Flex.Client.ViewModel
{
  public class StateViewModel : BaseViewModel, IStateViewModel
  {
    private ObservableCollection<IStateStepViewModel> _stateStepViewModels = new ObservableCollection<IStateStepViewModel>();
    private readonly ILoggerService _loggerService;

    public ObservableCollection<IStateStepViewModel> StateStepViewModels
    {
      get
      {
        return this._stateStepViewModels;
      }
      set
      {
        this._stateStepViewModels = value;
        this.OnPropertyChanged(nameof (StateStepViewModels));
      }
    }

    public StateViewModel(ILanguageService languageService, ILoggerService loggerService)
    {
      StateViewModel stateViewModel = this;
      this._loggerService = loggerService;
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        stateViewModel.StateStepViewModels = new ObservableCollection<IStateStepViewModel>((IEnumerable<IStateStepViewModel>) new List<IStateStepViewModel>()
        {
          (IStateStepViewModel) new StateStepViewModel(languageService, ViewState.HealthCheck, false, "StateHealthCheckText"),
          (IStateStepViewModel) new StateStepViewModel(languageService, ViewState.Login, false, "StateLoginText"),
          (IStateStepViewModel) new StateStepViewModel(languageService, ViewState.Workspace, false, "StateWorkspaceText"),
          (IStateStepViewModel) new StateStepViewModel(languageService, ViewState.Ongoing, false, "StateOngoingExamText"),
          (IStateStepViewModel) new StateStepViewModel(languageService, ViewState.HandInReceived, true, "StateHandInReceivedText")
        }.OrderByDescending<IStateStepViewModel, ViewState>((Func<IStateStepViewModel, ViewState>) (s => s.ViewState)));
        stateViewModel.SetAwaitingState((IEnumerable<IStateStepViewModel>) stateViewModel.StateStepViewModels);
      }));
    }

    public void UpdateActiveView(ViewState viewState)
    {
      try
      {
        this.SetFinishedState(this.StateStepViewModels.Where<IStateStepViewModel>((Func<IStateStepViewModel, bool>) (s => s.ViewState < viewState)));
        this.SetOngoingState(this.StateStepViewModels.Single<IStateStepViewModel>((Func<IStateStepViewModel, bool>) (s => s.ViewState == viewState)));
        this.SetAwaitingState(this.StateStepViewModels.Where<IStateStepViewModel>((Func<IStateStepViewModel, bool>) (s => s.ViewState > viewState)));
      }
      catch (Exception ex)
      {
        this._loggerService.Log(LogType.Error, "Error switching to new state. ExMessage: " + ex.Message, ex.StackTrace);
      }
    }

    private void SetOngoingState(IStateStepViewModel ongoingStateStep)
    {
      ongoingStateStep.UpdateState(StateStep.Ongoing);
    }

    private void SetAwaitingState(IEnumerable<IStateStepViewModel> awaitingStateSteps)
    {
      foreach (IStateStepViewModel awaitingStateStep in awaitingStateSteps)
        awaitingStateStep.UpdateState(StateStep.Awaiting);
    }

    private void SetFinishedState(IEnumerable<IStateStepViewModel> finishedStateSteps)
    {
      foreach (IStateStepViewModel finishedStateStep in finishedStateSteps)
        finishedStateStep.UpdateState(StateStep.Finished);
    }
  }
}
