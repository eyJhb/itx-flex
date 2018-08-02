// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.HealthCheckViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.Model;
using Itx.Flex.Client.Service;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace Itx.Flex.Client.ViewModel
{
  public class HealthCheckViewModel : BaseViewModel, IHealthCheckViewModel, IBaseViewModel
  {
    private readonly IMessenger _messenger;
    private readonly ICompositeHealthCheckService _compositeHealthCheckService;
    private readonly ILanguageService _languageService;
    private readonly ITimerService _countdownTimerService;
    private readonly IConfigurationService _configurationService;
    private int _timeUntilNextRetryInSeconds;
    private int _nextTryIn;
    private string _healthCheckAutomaticRetryPluralText;
    private string _healthCheckAutomaticRetrySingularText;
    private bool _canStartExamination;
    private bool _healthCheckDone;
    private OverallHealthCheckStatus _silentHealthCheck;
    private string _healthCheckStatusHeaderText;
    private string _overallStatusText;
    private string _healthCheckStartExaminationButtonText;
    private ObservableCollection<HealthCheckStatusViewModel> _healthCheckStatusViewModels;

    public HealthCheckViewModel(IMessenger messenger, ICompositeHealthCheckService compositeHealthCheckService, ILanguageService languageService, ITimerService countdownTimerService, IConfigurationService configurationService)
    {
      this._messenger = messenger;
      this._compositeHealthCheckService = compositeHealthCheckService;
      this._languageService = languageService;
      this._countdownTimerService = countdownTimerService;
      this._configurationService = configurationService;
      this._countdownTimerService.AutoReset = true;
      this._countdownTimerService.Interval = 1000.0;
      this._countdownTimerService.Elapsed += new ElapsedEventHandler(this.CountdownTimerServiceOnElapsed);
      this.StartExaminationCommand = (ICommand) new RelayCommand((Action<object>) (c => this.StartExamination()), (Predicate<object>) null);
      this._messenger.Register<OnStartHealthCheck>((object) this, new Action<OnStartHealthCheck>(this.OnStartHealthCheck));
      this.UpdateLanguage((OnLanguageChanged) null);
      this._messenger.Register<OnLanguageChanged>((object) this, new Action<OnLanguageChanged>(this.UpdateLanguage));
    }

    private void ResetTimeUntilNextRetry()
    {
      this._nextTryIn = this._configurationService.HealthCheckRetryIntervalInSeconds;
    }

    public int TimeUntilNextRetryInSeconds
    {
      get
      {
        return this._timeUntilNextRetryInSeconds;
      }
      set
      {
        this._timeUntilNextRetryInSeconds = value;
        this.OnPropertyChanged(nameof (TimeUntilNextRetryInSeconds));
        this.OnPropertyChanged("HealthCheckAutomaticRetryText");
        this.OnPropertyChanged("HealthCheckFailed");
      }
    }

    private void CountdownTimerServiceOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
    {
      --this._nextTryIn;
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.TimeUntilNextRetryInSeconds = this._nextTryIn));
      if (this._nextTryIn != 0)
        return;
      this._countdownTimerService.Stop();
      this.OnStartHealthCheck((OnStartHealthCheck) null);
    }

    private void UpdateLanguage(OnLanguageChanged onLanguageChanged = null)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.HealthCheckStatusHeaderText = this._languageService.GetString("HealthCheckStatusHeaderText");
        this.HealthCheckStartExaminationButtonText = this._languageService.GetString("HealthCheckStartExaminationButtonText");
        this.HealthCheckAutomaticRetryPluralText = this._languageService.GetString("HealthCheckAutomaticRetryPluralText");
        this.HealthCheckAutomaticRetrySingularText = this._languageService.GetString("HealthCheckAutomaticRetrySingularText");
        this.OverallStatusText = this._languageService.GetString(this.OverallStatusTextKey);
      }));
    }

    public string HealthCheckAutomaticRetryText
    {
      get
      {
        if (this.TimeUntilNextRetryInSeconds <= 1)
          return this.HealthCheckAutomaticRetrySingularText;
        return string.Format(this.HealthCheckAutomaticRetryPluralText, (object) this.TimeUntilNextRetryInSeconds);
      }
    }

    private string HealthCheckAutomaticRetryPluralText
    {
      get
      {
        return this._healthCheckAutomaticRetryPluralText;
      }
      set
      {
        this._healthCheckAutomaticRetryPluralText = value;
        this.OnPropertyChanged(nameof (HealthCheckAutomaticRetryPluralText));
        this.OnPropertyChanged("HealthCheckAutomaticRetryText");
      }
    }

    private string HealthCheckAutomaticRetrySingularText
    {
      get
      {
        return this._healthCheckAutomaticRetrySingularText;
      }
      set
      {
        this._healthCheckAutomaticRetrySingularText = value;
        this.OnPropertyChanged(nameof (HealthCheckAutomaticRetrySingularText));
        this.OnPropertyChanged("HealthCheckAutomaticRetryText");
      }
    }

    private void StartExamination()
    {
      this.UpdateState();
    }

    public bool CanStartExamination
    {
      get
      {
        return this._canStartExamination;
      }
      set
      {
        this._canStartExamination = value;
        this.OnPropertyChanged(nameof (CanStartExamination));
      }
    }

    public bool HealthCheckFailed
    {
      get
      {
        return this.TimeUntilNextRetryInSeconds > 0;
      }
    }

    public bool HealthCheckDone
    {
      get
      {
        return this._healthCheckDone;
      }
      set
      {
        this._healthCheckDone = value;
        this.OnPropertyChanged(nameof (HealthCheckDone));
        this.OnPropertyChanged("ShowHealthCheckSpinner");
      }
    }

    public bool ShowHealthCheckSpinner
    {
      get
      {
        return !this._healthCheckDone;
      }
    }

    private void OnStartHealthCheck(OnStartHealthCheck onStartHealthCheck = null)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.HealthCheckDone = false;
        Task.Factory.StartNew((Action) (() =>
        {
          OverallHealthCheckStatus overallHealthCheck = this._silentHealthCheck ?? this._compositeHealthCheckService.Check();
          if (overallHealthCheck.MustUpdate)
          {
            this._messenger.Send<OnCheckForUpdates>(new OnCheckForUpdates());
          }
          else
          {
            this._silentHealthCheck = (OverallHealthCheckStatus) null;
            DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
            {
              this.HealthCheckDone = true;
              this.OverallStatusTextKey = overallHealthCheck.OverallStatusTextKey;
              this.OverallStatusText = this._languageService.GetString(this.OverallStatusTextKey);
              this.HealthCheckStatusViewModels = new ObservableCollection<HealthCheckStatusViewModel>(overallHealthCheck.HealthCheckStatuses.Select<HealthCheckStatus, HealthCheckStatusViewModel>((Func<HealthCheckStatus, HealthCheckStatusViewModel>) (hcs => new HealthCheckStatusViewModel(this._languageService, this._messenger, hcs.DescriptionKey, hcs.ImageSource, hcs.ReadMoreKey))));
              this.CanStartExamination = overallHealthCheck.CanContinue;
              if (this.CanStartExamination)
                return;
              this.ResetTimeUntilNextRetry();
              this._countdownTimerService.Start();
            }));
          }
        }));
      }));
    }

    public bool CheckHealth()
    {
      this._silentHealthCheck = this._silentHealthCheck ?? this._compositeHealthCheckService.Check();
      return this._silentHealthCheck.CanContinue;
    }

    public void UpdateState()
    {
      this._messenger.Send<OnSuccessfulHealthCheck>(new OnSuccessfulHealthCheck());
    }

    public ICommand StartExaminationCommand { get; }

    public string HealthCheckStatusHeaderText
    {
      get
      {
        return this._healthCheckStatusHeaderText;
      }
      set
      {
        this._healthCheckStatusHeaderText = value;
        this.OnPropertyChanged(nameof (HealthCheckStatusHeaderText));
      }
    }

    public string OverallStatusText
    {
      get
      {
        return this._overallStatusText;
      }
      set
      {
        this._overallStatusText = value;
        this.OnPropertyChanged(nameof (OverallStatusText));
      }
    }

    private string OverallStatusTextKey { get; set; }

    public string HealthCheckStartExaminationButtonText
    {
      get
      {
        return this._healthCheckStartExaminationButtonText;
      }
      set
      {
        this._healthCheckStartExaminationButtonText = value;
        this.OnPropertyChanged(nameof (HealthCheckStartExaminationButtonText));
      }
    }

    public ObservableCollection<HealthCheckStatusViewModel> HealthCheckStatusViewModels
    {
      get
      {
        return this._healthCheckStatusViewModels;
      }
      set
      {
        this._healthCheckStatusViewModels = value;
        this.OnPropertyChanged(nameof (HealthCheckStatusViewModels));
      }
    }
  }
}
