// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.LastSaveViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.Service;
using System;
using System.Timers;

namespace Itx.Flex.Client.ViewModel
{
  public class LastSaveViewModel : BaseViewModel, ILastSaveViewModel
  {
    private readonly ILanguageService _languageService;
    private readonly ITimerService _lastSaveTimer;
    private readonly ILastFileSaveService _lastFileSaveService;
    private int _timeSinceLastSaveInMinutes;
    private string _ongoingExamSaveStatusPluralText;
    private string _ongoingExamSaveStatusSingularText;

    public LastSaveViewModel(ILanguageService languageService, IMessenger messenger, ITimerService lastSaveTimer, IConfigurationService configurationService, ILastFileSaveService lastFileSaveService)
    {
      this._languageService = languageService;
      this._lastSaveTimer = lastSaveTimer;
      this._lastFileSaveService = lastFileSaveService;
      lastSaveTimer.AutoReset = true;
      lastSaveTimer.Interval = (double) (configurationService.LastSaveUpdateIntervalInSeconds * 1000);
      this.UpdateLanguage((OnLanguageChanged) null);
      messenger.Register<OnLanguageChanged>((object) this, new Action<OnLanguageChanged>(this.UpdateLanguage));
      messenger.Register<OnWorkspacePathsSet>((object) this, new Action<OnWorkspacePathsSet>(this.OnHandInPathSet));
    }

    private void UpdateLanguage(OnLanguageChanged onLanguageChanged = null)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.OngoingExamSaveStatusPluralText = this._languageService.GetString("OngoingExamSaveStatusPluralText");
        this.OngoingExamSaveStatusSingularText = this._languageService.GetString("OngoingExamSaveStatusSingularText");
      }));
    }

    private void OnHandInPathSet(OnWorkspacePathsSet onWorkspacePathsSet)
    {
      this._lastSaveTimer.Elapsed += (ElapsedEventHandler) ((s, e) => this.UpdateLastSave(onWorkspacePathsSet.HandInPath));
      this._lastSaveTimer.Start();
      this.UpdateLastSave(onWorkspacePathsSet.HandInPath);
    }

    private void UpdateLastSave(string handInPath)
    {
      if (!this._lastFileSaveService.AnyFilesInPath(handInPath))
        return;
      int retrievedTimeSinceLastSaveInMinutes = this._lastFileSaveService.GetTimeSinceLastSaveInMinutes(handInPath);
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.TimeSinceLastSaveInMinutes = retrievedTimeSinceLastSaveInMinutes));
    }

    public int TimeSinceLastSaveInMinutes
    {
      get
      {
        return this._timeSinceLastSaveInMinutes;
      }
      set
      {
        this._timeSinceLastSaveInMinutes = value;
        this.OnPropertyChanged(nameof (TimeSinceLastSaveInMinutes));
        this.OnPropertyChanged("OngoingExamSaveStatusText");
      }
    }

    public string OngoingExamSaveStatusPluralText
    {
      get
      {
        return this._ongoingExamSaveStatusPluralText;
      }
      set
      {
        this._ongoingExamSaveStatusPluralText = value;
        this.OnPropertyChanged("OngoingExamSaveStatusText");
      }
    }

    public string OngoingExamSaveStatusSingularText
    {
      get
      {
        return this._ongoingExamSaveStatusSingularText;
      }
      set
      {
        this._ongoingExamSaveStatusSingularText = value;
        this.OnPropertyChanged("OngoingExamSaveStatusText");
      }
    }

    public string OngoingExamSaveStatusText
    {
      get
      {
        if (this.TimeSinceLastSaveInMinutes != 1)
          return string.Format(this.OngoingExamSaveStatusPluralText, (object) this.TimeSinceLastSaveInMinutes);
        return this.OngoingExamSaveStatusSingularText;
      }
    }
  }
}
