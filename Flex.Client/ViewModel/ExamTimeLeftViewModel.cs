// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.ExamTimeLeftViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.Service;
using System;

namespace Itx.Flex.Client.ViewModel
{
  public class ExamTimeLeftViewModel : BaseViewModel, IExamTimeLeftViewModel
  {
    private readonly ILanguageService _languageService;
    private double _timeLeftUntilExamEndInMinutes;
    private string _ongoingExamTimeLeftPluralText;
    private string _ongoingExamTimeLeftSingularText;

    public ExamTimeLeftViewModel(IMessenger messenger, ILanguageService languageService)
    {
      this._languageService = languageService;
      this.UpdateLanguage((OnLanguageChanged) null);
      messenger.Register<OnLanguageChanged>((object) this, new Action<OnLanguageChanged>(this.UpdateLanguage));
      messenger.Register<OnExaminationDataLoaded>((object) this, new Action<OnExaminationDataLoaded>(this.OnExaminationDataLoaded));
      messenger.Register<OnTimeLeftUntilExamEnds>((object) this, new Action<OnTimeLeftUntilExamEnds>(this.OnTimeLeftUntilExamEnds));
    }

    private void UpdateLanguage(OnLanguageChanged onLanguageChanged = null)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.OngoingExamTimeLeftPluralText = this._languageService.GetString("OngoingExamTimeLeftPluralText");
        this.OngoingExamTimeLeftSingularText = this._languageService.GetString("OngoingExamTimeLeftSingularText");
      }));
    }

    private void OnExaminationDataLoaded(OnExaminationDataLoaded onExaminationDataLoaded)
    {
      TimeSpan examinationDuration = onExaminationDataLoaded.End - onExaminationDataLoaded.Start;
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.ExamLengthInMinutes = examinationDuration.TotalMinutes));
    }

    private void OnTimeLeftUntilExamEnds(OnTimeLeftUntilExamEnds onTimeLeftUntilExamEnds)
    {
      int retrievedMinutesLeftUntilExaminationEnds = (onTimeLeftUntilExamEnds.ExaminationEndInSeconds + 59) / 60;
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.TimeLeftUntilExamEndInMinutes = (double) retrievedMinutesLeftUntilExaminationEnds));
    }

    public double ProgressBarMaximum
    {
      get
      {
        return 100.0;
      }
    }

    public double ProgressBarMinimum
    {
      get
      {
        return 0.0;
      }
    }

    private double ExamLengthInMinutes { get; set; }

    private double TimeLeftUntilExamEndInMinutes
    {
      get
      {
        return this._timeLeftUntilExamEndInMinutes;
      }
      set
      {
        this._timeLeftUntilExamEndInMinutes = value;
        this.OnPropertyChanged(nameof (TimeLeftUntilExamEndInMinutes));
        this.OnPropertyChanged("OngoingExamTimeLeftText");
        this.OnPropertyChanged("ProgressBarCurrent");
      }
    }

    public double ProgressBarCurrent
    {
      get
      {
        return (1.0 - this.TimeLeftUntilExamEndInMinutes / this.ExamLengthInMinutes) * this.ProgressBarMaximum;
      }
    }

    private string OngoingExamTimeLeftPluralText
    {
      get
      {
        return this._ongoingExamTimeLeftPluralText;
      }
      set
      {
        this._ongoingExamTimeLeftPluralText = value;
        this.OnPropertyChanged("OngoingExamTimeLeftText");
      }
    }

    private string OngoingExamTimeLeftSingularText
    {
      get
      {
        return this._ongoingExamTimeLeftSingularText;
      }
      set
      {
        this._ongoingExamTimeLeftSingularText = value;
        this.OnPropertyChanged("OngoingExamTimeLeftText");
      }
    }

    public string OngoingExamTimeLeftText
    {
      get
      {
        if (this.TimeLeftUntilExamEndInMinutes < 1.0 || this.TimeLeftUntilExamEndInMinutes > 2.0)
          return string.Format(this.OngoingExamTimeLeftPluralText, (object) (int) this.TimeLeftUntilExamEndInMinutes);
        return this.OngoingExamTimeLeftSingularText;
      }
    }
  }
}
