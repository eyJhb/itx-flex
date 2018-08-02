// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.StateHandlerViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.Model;
using System;

namespace Itx.Flex.Client.ViewModel
{
  public class StateHandlerViewModel : BaseViewModel, IStateHandlerViewModel
  {
    private readonly IUpdateProgramViewModel _updateProgramViewModel;
    private readonly IHealthCheckViewModel _healthCheckViewModel;
    private readonly ILoginViewModel _loginViewModel;
    private readonly IWorkSpaceViewModel _workSpaceViewModel;
    private readonly IOngoingExamViewModel _ongoingExamViewModel;
    private readonly IHandInReceivedViewModel _handInReceivedViewModel;
    private readonly IMessenger _messenger;
    private IStateViewModel _stateViewModel;
    private readonly ISubmitHandInViewModel _submitHandInViewModel;
    private readonly IHandInSubmittingViewModel _handInSubmittingViewModel;
    private IBaseViewModel _currentViewModel;

    public StateHandlerViewModel(IUpdateProgramViewModel updateProgramViewModel, IHealthCheckViewModel healthCheckViewModel, ILoginViewModel loginViewModel, IOngoingExamViewModel ongoingExamViewModel, IWorkSpaceViewModel workSpaceViewModel, IHandInReceivedViewModel handInReceivedViewModel, IMessenger messenger, IStateViewModel stateViewModel, ISubmitHandInViewModel submitHandInViewModel, IHandInSubmittingViewModel handInSubmittingViewModel)
    {
      this._updateProgramViewModel = updateProgramViewModel;
      this._healthCheckViewModel = healthCheckViewModel;
      this._loginViewModel = loginViewModel;
      this._workSpaceViewModel = workSpaceViewModel;
      this._ongoingExamViewModel = ongoingExamViewModel;
      this._handInReceivedViewModel = handInReceivedViewModel;
      this._messenger = messenger;
      this._stateViewModel = stateViewModel;
      this._submitHandInViewModel = submitHandInViewModel;
      this._handInSubmittingViewModel = handInSubmittingViewModel;
      messenger.Register<OnSuccessfulLogin>((object) this, new Action<OnSuccessfulLogin>(this.OnSuccessfulLogin));
      messenger.Register<OnSuccessfulHealthCheck>((object) this, new Action<OnSuccessfulHealthCheck>(this.OnSuccessfulHealthCheck));
      messenger.Register<OnExamStarted>((object) this, new Action<OnExamStarted>(this.OnExamStarted));
      messenger.Register<OnHandInStatusHandInTypeUpdated>((object) this, new Action<OnHandInStatusHandInTypeUpdated>(this.OnHandInReceivedUpdated));
      messenger.Register<OnUpdateFinished>((object) this, new Action<OnUpdateFinished>(this.OnUpdateFinished));
      this.OnCheckForUpdates((OnCheckForUpdates) null);
      messenger.Register<OnCheckForUpdates>((object) this, new Action<OnCheckForUpdates>(this.OnCheckForUpdates));
      messenger.Register<OnSubmitBlankHandIn>((object) this, new Action<OnSubmitBlankHandIn>(this.OnSubmitBlankHandIn));
      messenger.Register<OnSubmitHandIn>((object) this, new Action<OnSubmitHandIn>(this.OnSubmitHandIn));
      messenger.Register<OnInitiateSubmitHandIn>((object) this, new Action<OnInitiateSubmitHandIn>(this.OnInitiateSubmitHandIn));
      messenger.Register<OnEnableHandInReceived>((object) this, new Action<OnEnableHandInReceived>(this.OnEnableHandInReceived));
      messenger.Register<OnAllowHandInReceived>((object) this, new Action<OnAllowHandInReceived>(this.OnAllowHandInReceived));
    }

    public IStateViewModel StateViewModel
    {
      get
      {
        return this._stateViewModel;
      }
      private set
      {
        this._stateViewModel = value;
        this.OnPropertyChanged(nameof (StateViewModel));
      }
    }

    public IBaseViewModel CurrentViewModel
    {
      get
      {
        return this._currentViewModel;
      }
      private set
      {
        this._currentViewModel = value;
        this.OnPropertyChanged(nameof (CurrentViewModel));
      }
    }

    public bool IsHandInReceived()
    {
      if (!this.CurrentHandInStatus.HasUploaded())
        return this.CurrentHandInType == HandInType.Blank;
      return true;
    }

    private void OnCheckForUpdates(OnCheckForUpdates obj = null)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.CurrentViewModel = (IBaseViewModel) this._updateProgramViewModel));
    }

    private void OnUpdateFinished(OnUpdateFinished onUpdateFinished)
    {
      if (this._loginViewModel.HasExistingLogin() && this._healthCheckViewModel.CheckHealth())
      {
        this._healthCheckViewModel.UpdateState();
      }
      else
      {
        this._messenger.Send<OnStartHealthCheck>(new OnStartHealthCheck());
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
        {
          this.StateViewModel.UpdateActiveView(ViewState.HealthCheck);
          this.CurrentViewModel = (IBaseViewModel) this._healthCheckViewModel;
        }));
      }
    }

    private void OnSuccessfulHealthCheck(OnSuccessfulHealthCheck onSuccessfulHealthCheck)
    {
      switch (this._loginViewModel.LoginUsingExisting())
      {
        case LoginResult.LoginFailed:
          DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
          {
            this.StateViewModel.UpdateActiveView(ViewState.Login);
            this.CurrentViewModel = (IBaseViewModel) this._loginViewModel;
          }));
          this._loginViewModel.SetupFailingLogin();
          break;
        case LoginResult.InvalidBoardingPass:
          this._messenger.Send<OnStartHealthCheck>(new OnStartHealthCheck());
          DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
          {
            this.StateViewModel.UpdateActiveView(ViewState.HealthCheck);
            this.CurrentViewModel = (IBaseViewModel) this._healthCheckViewModel;
          }));
          break;
        case LoginResult.LoginSuccessful:
          this._loginViewModel.UpdateState();
          break;
        case LoginResult.NoLoginFound:
          DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
          {
            this.StateViewModel.UpdateActiveView(ViewState.Login);
            this.CurrentViewModel = (IBaseViewModel) this._loginViewModel;
          }));
          break;
      }
    }

    private void OnSuccessfulLogin(OnSuccessfulLogin onSuccessfulLogin)
    {
      if (this._workSpaceViewModel.HasVerifiedPinCode())
        this._workSpaceViewModel.UpdateState();
      else
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
        {
          this.StateViewModel.UpdateActiveView(ViewState.Workspace);
          this.CurrentViewModel = (IBaseViewModel) this._workSpaceViewModel;
        }));
    }

    private HandInStatus CurrentHandInStatus { get; set; }

    private HandInType CurrentHandInType { get; set; }

    private void OnHandInReceivedUpdated(OnHandInStatusHandInTypeUpdated onHandInStatusHandInTypeUpdated)
    {
      if (this.CurrentHandInStatus == onHandInStatusHandInTypeUpdated.HandInStatus)
        return;
      this.CurrentHandInStatus = onHandInStatusHandInTypeUpdated.HandInStatus;
      this.CurrentHandInType = onHandInStatusHandInTypeUpdated.HandInType;
      if (!this._workSpaceViewModel.HasVerifiedPinCode())
        return;
      this.UpdateViewBasedOnCurrentHandIn(this.CurrentHandInType, this.CurrentHandInStatus);
    }

    private void UpdateViewBasedOnCurrentHandIn(HandInType handInType, HandInStatus handInStatus)
    {
      if (handInType == HandInType.HandIn && handInStatus.HasTagged())
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
        {
          if (this.CurrentViewModel == this._handInSubmittingViewModel)
            return;
          this.StateViewModel.UpdateActiveView(ViewState.Ongoing);
          this._handInSubmittingViewModel.SubmittedHandIn(handInStatus, this._submitHandInViewModel.GetHandInFiles());
          this.CurrentViewModel = (IBaseViewModel) this._handInSubmittingViewModel;
        }));
      else if (handInStatus.HasUploaded())
      {
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
        {
          this.StateViewModel.UpdateActiveView(ViewState.HandInReceived);
          this._handInReceivedViewModel.Submitted(handInType, handInStatus);
          this.CurrentViewModel = (IBaseViewModel) this._handInReceivedViewModel;
        }));
      }
      else
      {
        if (handInStatus != HandInStatus.NotTagged)
          return;
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
        {
          this.StateViewModel.UpdateActiveView(ViewState.Ongoing);
          this.CurrentViewModel = (IBaseViewModel) this._ongoingExamViewModel;
        }));
      }
    }

    private void OnExamStarted(OnExamStarted onExamStarted)
    {
      this.UpdateViewBasedOnCurrentHandIn(this.CurrentHandInType, this.CurrentHandInStatus);
    }

    private void OnInitiateSubmitHandIn(OnInitiateSubmitHandIn onInitiateSubmitHandIn)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.StateViewModel.UpdateActiveView(ViewState.Ongoing);
        this.CurrentViewModel = (IBaseViewModel) this._submitHandInViewModel;
      }));
    }

    private void OnSubmitBlankHandIn(OnSubmitBlankHandIn onSubmitBlankHandIn)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.CurrentHandInStatus = onSubmitBlankHandIn.HandInStatus;
        this.CurrentHandInType = HandInType.Blank;
        this.StateViewModel.UpdateActiveView(ViewState.HandInReceived);
        this._handInReceivedViewModel.Submitted(this.CurrentHandInType, this.CurrentHandInStatus);
        this.CurrentViewModel = (IBaseViewModel) this._handInReceivedViewModel;
      }));
    }

    private void OnSubmitHandIn(OnSubmitHandIn onSubmitHandIn)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.StateViewModel.UpdateActiveView(ViewState.Ongoing);
        this._handInSubmittingViewModel.SubmittedHandIn(onSubmitHandIn.HandInStatus, onSubmitHandIn.HandInFiles);
        this.CurrentViewModel = (IBaseViewModel) this._handInSubmittingViewModel;
      }));
    }

    private void OnEnableHandInReceived(OnEnableHandInReceived onEnableHandInReceived)
    {
      if (onEnableHandInReceived.EnableHandIn)
        return;
      this.StopHandInFlow();
    }

    private void StopHandInFlow()
    {
      if (this.CurrentViewModel != this._submitHandInViewModel && this.CurrentViewModel != this._handInSubmittingViewModel && this.CurrentViewModel != this._ongoingExamViewModel || this.CurrentHandInStatus.HasUploaded())
        return;
      this._handInSubmittingViewModel.CancelUpload();
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this._messenger.Send<OnOkCancelPopupClosing>(new OnOkCancelPopupClosing(false));
        this._messenger.Send<OnOkPopupClosing>(new OnOkPopupClosing());
        this.StateViewModel.UpdateActiveView(ViewState.Ongoing);
        this.CurrentViewModel = (IBaseViewModel) this._ongoingExamViewModel;
      }));
    }

    private void OnAllowHandInReceived(OnAllowHandInReceived onAllowHandInReceived)
    {
      if (onAllowHandInReceived.AllowHandin)
        return;
      this.StopHandInFlow();
    }
  }
}
