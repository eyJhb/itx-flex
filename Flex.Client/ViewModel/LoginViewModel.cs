// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.LoginViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using Arcanic.ITX.Flex.WebserviceClient;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.Model;
using Itx.Flex.Client.Service;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace Itx.Flex.Client.ViewModel
{
  public class LoginViewModel : BaseViewModel, ILoginViewModel, IBaseViewModel, IDataErrorInfo
  {
    private readonly IMessenger _messenger;
    private readonly IBoardingPassValidator _boardingPassValidator;
    private readonly ILanguageService _languageService;
    private readonly IConfigurationService _configurationService;
    private readonly ITimerService _loginAttemptsTimer;
    private readonly IBoardingPassStorageService _boardingPassStorageService;
    private readonly IFlexClient _flexClient;
    private readonly IHeartbeatService _heartbeatService;
    private readonly IStorageCleanerService _storageCleanerService;
    private bool _canLogin;
    private string _boardingPass;
    private string _loginBoardingPassText;
    private string _loginBoardingPassHelperText;
    private string _loginBoardingPassContinueButtonText;
    private bool _showLoginTextboxError;

    public LoginViewModel(IMessenger messenger, IBoardingPassValidator boardingPassValidator, ILanguageService languageService, IConfigurationService configurationService, ITimerService loginAttemptsTimer, IBoardingPassStorageService boardingPassStorageService, IFlexClient flexClient, IHeartbeatService heartbeatService, IStorageCleanerService storageCleanerService)
    {
      this._messenger = messenger;
      this._boardingPassValidator = boardingPassValidator;
      this._languageService = languageService;
      this._configurationService = configurationService;
      this._loginAttemptsTimer = loginAttemptsTimer;
      this._boardingPassStorageService = boardingPassStorageService;
      this._flexClient = flexClient;
      this._heartbeatService = heartbeatService;
      this._storageCleanerService = storageCleanerService;
      this._messenger.Register<OnSuccessfulHealthCheck>((object) this, new Action<OnSuccessfulHealthCheck>(this.OnSuccessfulHealthCheck));
      this.UpdateText((OnLanguageChanged) null);
      this._messenger.Register<OnLanguageChanged>((object) this, new Action<OnLanguageChanged>(this.UpdateText));
      this._messenger.Register<OnLoginErrorPopupClosed>((object) this, new Action<OnLoginErrorPopupClosed>(this.OnLoginErrorPopupClosed));
      this.LoginCommand = (ICommand) new RelayCommand((Action<object>) (c => this.LoginClick()), (Predicate<object>) null);
      this._loginAttemptsTimer.AutoReset = false;
      this._loginAttemptsTimer.Interval = (double) (configurationService.BoardingPassLoginLockDurationInSeconds * 1000);
      this._loginAttemptsTimer.Elapsed += (ElapsedEventHandler) ((s, e) =>
      {
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.CanLogin = true));
        this.LoginAttemptsCounter = 0;
      });
    }

    private void OnLoginErrorPopupClosed(OnLoginErrorPopupClosed onLoginErrorPopupClosed)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.CanLogin = true));
      if (this.LoginAttemptsCounter < this._configurationService.BoardingPassLoginAttemptsBeforeLocking)
        return;
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.CanLogin = false));
      this._loginAttemptsTimer.Start();
    }

    private void UpdateText(OnLanguageChanged onLanguageChanged = null)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.LoginBoardingPassText = this._languageService.GetString("LoginBoardingPassText");
        this.LoginBoardingPassHelperText = this._languageService.GetString("LoginBoardingPassHelperText");
        this.LoginBoardingPassContinueButtonText = this._languageService.GetString("LoginBoardingPassContinueButtonText");
        this.LoginFailedLoginText = this._languageService.GetString("LoginFailedLoginText");
        this.LoginUnauthorizedBoardingPassText = this._languageService.GetString("LoginUnauthorizedBoardingPassText");
        this.LoginFailedLoginButtonText = this._languageService.GetString("LoginFailedLoginButtonText");
        this.OnPropertyChanged("BoardingPass");
      }));
    }

    public bool CanLogin
    {
      get
      {
        return this._canLogin;
      }
      set
      {
        this._canLogin = value;
        this.OnPropertyChanged(nameof (CanLogin));
      }
    }

    private void OnSuccessfulHealthCheck(OnSuccessfulHealthCheck onSuccessfulHealthCheck)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.CanLogin = true));
    }

    public string BoardingPass
    {
      get
      {
        return this._boardingPass;
      }
      set
      {
        this._boardingPass = value;
        this.OnPropertyChanged(nameof (BoardingPass));
        this.ShowLoginTextboxError = true;
      }
    }

    public string LoginBoardingPassText
    {
      get
      {
        return this._loginBoardingPassText;
      }
      set
      {
        this._loginBoardingPassText = value;
        this.OnPropertyChanged(nameof (LoginBoardingPassText));
      }
    }

    public string LoginBoardingPassHelperText
    {
      get
      {
        return this._loginBoardingPassHelperText;
      }
      set
      {
        this._loginBoardingPassHelperText = value;
        this.OnPropertyChanged(nameof (LoginBoardingPassHelperText));
      }
    }

    public string LoginBoardingPassContinueButtonText
    {
      get
      {
        return this._loginBoardingPassContinueButtonText;
      }
      set
      {
        this._loginBoardingPassContinueButtonText = value;
        this.OnPropertyChanged(nameof (LoginBoardingPassContinueButtonText));
      }
    }

    private bool ShowLoginTextboxError
    {
      get
      {
        return this._showLoginTextboxError;
      }
      set
      {
        this._showLoginTextboxError = value;
        this.OnPropertyChanged(nameof (ShowLoginTextboxError));
        this.OnPropertyChanged("BoardingPass");
      }
    }

    private void LoginClick()
    {
      if (!this._boardingPassValidator.Validate(this.BoardingPass).IsValid)
        return;
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.ShowLoginTextboxError = true;
        this.CanLogin = false;
      }));
      Task.Factory.StartNew((Action) (() =>
      {
        switch (this.Login(this.BoardingPass))
        {
          case LoginState.Success:
            this.LoginSuccessful();
            break;
          case LoginState.Unauthorized:
            DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.BoardingPass = ""));
            this.LoginFailed(this.LoginUnauthorizedBoardingPassText);
            break;
          case LoginState.Failed:
            this.LoginFailed(this.LoginFailedLoginText);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }));
    }

    private LoginState Login(string boardingPassLogin)
    {
      try
      {
        this._flexClient.SetBoardingPassPrefix(boardingPassLogin);
        this._flexClient.Authenticate(boardingPassLogin);
        this._boardingPassStorageService.Store(boardingPassLogin);
        ExaminationResponse examination = this._flexClient.GetExamination();
        IMessenger messenger = this._messenger;
        string daDk = examination.Title.DaDK;
        string enGb = examination.Title.EnGB;
        DateTime dateTime = examination.Start;
        DateTime localTime1 = dateTime.ToLocalTime();
        dateTime = examination.End;
        DateTime localTime2 = dateTime.ToLocalTime();
        string username = examination.Username;
        int beforeStartInMinutes = examination.EarliestLoginTimeBeforeStartInMinutes;
        OnExaminationDataLoaded message = new OnExaminationDataLoaded(daDk, enGb, localTime1, localTime2, username, beforeStartInMinutes);
        messenger.Send<OnExaminationDataLoaded>(message);
        this._messenger.Send<OnHandInFieldsLoaded>(new OnHandInFieldsLoaded(examination.HandinFields.Select<HandinField, HandInFieldDescriptionModel>((Func<HandinField, HandInFieldDescriptionModel>) (h => new HandInFieldDescriptionModel()
        {
          ValueType = h.Type.ToHandInFieldValueType(),
          TextDaDk = h.Title.DaDK,
          Required = h.IsRequired,
          TextEnGb = h.Title.EnGB,
          Id = h.Id,
          DescriptionDaDk = h.Description.DaDK,
          DescriptionEnGb = h.Description.EnGB,
          RegexValidation = h.Regex
        }))));
        this._messenger.Send<OnExaminationUrlLoaded>(new OnExaminationUrlLoaded(examination.Url));
        this._messenger.Send<OnAllowedFileExtensionsLoaded>(new OnAllowedFileExtensionsLoaded(examination.SubmitExtensions.Select<SubmitExtension, string>((Func<SubmitExtension, string>) (se => se.Extension))));
        this._messenger.Send<OnHandInFilesizeLimitLoaded>(new OnHandInFilesizeLimitLoaded(examination.MaxHandInFileSizeInMegabytes));
        this._heartbeatService.StartHeartbeats();
        return LoginState.Success;
      }
      catch (UnauthorizedBoardingPassException ex)
      {
        this._storageCleanerService.Clean();
        return LoginState.Unauthorized;
      }
      catch (Exception ex)
      {
        return LoginState.Failed;
      }
    }

    public LoginResult LoginUsingExisting()
    {
      if (this._boardingPassStorageService.HasExisting())
      {
        switch (this.Login(this._boardingPassStorageService.GetExisting()))
        {
          case LoginState.Success:
            return LoginResult.LoginSuccessful;
          case LoginState.Unauthorized:
            return LoginResult.InvalidBoardingPass;
          case LoginState.Failed:
            return LoginResult.LoginFailed;
        }
      }
      return LoginResult.NoLoginFound;
    }

    public bool HasExistingLogin()
    {
      return this._boardingPassStorageService.HasExisting();
    }

    public void UpdateState()
    {
      this._messenger.Send<OnSuccessfulLogin>(new OnSuccessfulLogin());
    }

    public void SetupFailingLogin()
    {
      if (!this._boardingPassStorageService.HasExisting())
        return;
      string existingBoardingPass = this._boardingPassStorageService.GetExisting();
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.BoardingPass = existingBoardingPass));
      this.LoginFailed(this.LoginFailedLoginText);
    }

    private int LoginAttemptsCounter { get; set; }

    private void LoginFailed(string errorMessage)
    {
      this._messenger.Send<OnLoginErrorPopupOpened>(new OnLoginErrorPopupOpened()
      {
        OkPopupViewModel = new OkPopupViewModel(errorMessage, this.LoginFailedLoginButtonText, this._messenger)
      });
      ++this.LoginAttemptsCounter;
    }

    private string LoginUnauthorizedBoardingPassText { get; set; }

    private string LoginFailedLoginText { get; set; }

    private string LoginFailedLoginButtonText { get; set; }

    private void LoginSuccessful()
    {
      this.UpdateState();
    }

    public ICommand LoginCommand { get; }

    public string this[string columnName]
    {
      get
      {
        if (!(columnName == "BoardingPass") || !this.ShowLoginTextboxError)
          return string.Empty;
        ValidatorResult validatorResult = this._boardingPassValidator.Validate(this.BoardingPass);
        if (!validatorResult.IsValid)
          return this._languageService.GetString(validatorResult.ErrorMessageKey);
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
  }
}
