// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.MainWindowViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.AutoUpdate;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.Service;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Itx.Flex.Client.ViewModel
{
  public class MainWindowViewModel : BaseViewModel, IMainWindowViewModel
  {
    private IStateHandlerViewModel _stateHandlerViewModel;
    private readonly IMessenger _messenger;
    private readonly ILanguageService _languageService;
    private readonly IGrabberService _grabberService;
    private readonly IStorageCleanerService _storageCleanerService;
    private readonly IConfigurationService _configurationService;
    private readonly IServerLogService _serverLogService;
    private readonly ILogDumpService _logDumpService;
    private bool _hasLoggedIn;
    private bool _grabsRunning;
    private bool _alreadyClosing;
    private string _mainWindowTitle;

    public IStateHandlerViewModel StateHandlerViewModel
    {
      get
      {
        return this._stateHandlerViewModel;
      }
      private set
      {
        this._stateHandlerViewModel = value;
        this.OnPropertyChanged(nameof (StateHandlerViewModel));
      }
    }

    public MainWindowViewModel(ICurrentVersionProvider currentVersionProvider, IMessenger messenger, IStateHandlerViewModel stateHandlerViewModel, ILanguageService languageService, IGrabberService grabberService, IStorageCleanerService storageCleanerService, IConfigurationService configurationService, IServerLogService serverLogService, ILogDumpService logDumpService)
    {
      this.StateHandlerViewModel = stateHandlerViewModel;
      this._messenger = messenger;
      this._languageService = languageService;
      this._grabberService = grabberService;
      this._storageCleanerService = storageCleanerService;
      this._configurationService = configurationService;
      this._serverLogService = serverLogService;
      this._logDumpService = logDumpService;
      this.Version = currentVersionProvider.Version.ToString(3);
      messenger.Register<OnClosingProgramRequested>((object) this, new Action<OnClosingProgramRequested>(this.OnClosingProgramRequested));
      this.OnLanguageChanged((OnLanguageChanged) null);
      messenger.Register<OnLanguageChanged>((object) this, new Action<OnLanguageChanged>(this.OnLanguageChanged));
      messenger.Register<OnNewGrabSettings>((object) this, new Action<OnNewGrabSettings>(this.OnNewGrabSettings));
      messenger.Register<OnSuccessfulHealthCheck>((object) this, new Action<OnSuccessfulHealthCheck>(this.OnSuccessfulHealthCheck));
      messenger.Register<OnSuccessfulLogin>((object) this, new Action<OnSuccessfulLogin>(this.OnSuccessfulLogin));
    }

    private void OnSuccessfulLogin(OnSuccessfulLogin onSuccessfulLogin)
    {
      this._hasLoggedIn = true;
    }

    private void OnSuccessfulHealthCheck(OnSuccessfulHealthCheck obj)
    {
      Task.Factory.StartNew((Action) (() => this._grabberService.UploadPreviousSessions()));
      Task.Factory.StartNew((Action) (() => this._logDumpService.DumpLog()));
    }

    private void OnNewGrabSettings(OnNewGrabSettings onNewGrabSettings)
    {
      this._grabsRunning = onNewGrabSettings.NoGrabsAfterSeconds > 0;
    }

    private void OnLanguageChanged(OnLanguageChanged obj = null)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.MainWindowTitle = this._languageService.GetString("MainWindowTitle");
        this.MainWindowWarnOnCloseCancelButtonText = this._languageService.GetString("MainWindowWarnOnCloseCancelButtonText");
        this.MainWindowWarnOnCloseOkButtonText = this._languageService.GetString("MainWindowWarnOnCloseOkButtonText");
        this.MainWindowWarnOnCloseText = this._languageService.GetString("MainWindowWarnOnCloseText");
      }));
    }

    private void OnClosingProgramRequested(OnClosingProgramRequested onClosingProgramRequested)
    {
      if (this._alreadyClosing)
        return;
      this._alreadyClosing = true;
      if (this._grabsRunning && !this.StateHandlerViewModel.IsHandInReceived())
      {
        this._messenger.Send<OnOkCancelPopupOpened>(new OnOkCancelPopupOpened(new OkCancelPopupViewModel(this.MainWindowWarnOnCloseText, this.MainWindowWarnOnCloseOkButtonText, this.MainWindowWarnOnCloseCancelButtonText, this._messenger), (Action<bool>) (hasSelectedOk =>
        {
          if (hasSelectedOk)
            this.CloseProgramAfterShowingWarning();
          else
            this._alreadyClosing = false;
        })));
      }
      else
      {
        if (this._hasLoggedIn)
          this._serverLogService.ClosedWithoutConfirmation();
        this.CloseProgram();
      }
    }

    private void CloseProgramAfterShowingWarning()
    {
      this._serverLogService.ClosedAfterConfirmation();
      this.CloseProgram();
    }

    private void CloseProgram()
    {
      Task.Factory.StartNew((Action) (() =>
      {
        if (this.StateHandlerViewModel.IsHandInReceived())
          this._storageCleanerService.Clean();
        this._grabberService.Stop();
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => Application.Current.Shutdown()));
      }));
    }

    private string MainWindowTitle
    {
      get
      {
        return this._mainWindowTitle;
      }
      set
      {
        this._mainWindowTitle = value;
        this.OnPropertyChanged(nameof (MainWindowTitle));
        this.OnPropertyChanged("MainWindowTitleText");
      }
    }

    public string MainWindowTitleText
    {
      get
      {
        return string.Format(this.MainWindowTitle, (object) this._configurationService.OfficialVersionName, (object) this.Version);
      }
    }

    private string MainWindowWarnOnCloseText { get; set; }

    private string MainWindowWarnOnCloseOkButtonText { get; set; }

    private string MainWindowWarnOnCloseCancelButtonText { get; set; }

    private string Version { get; }
  }
}
