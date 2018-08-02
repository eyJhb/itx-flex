// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.HandInSubmittingViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.Model;
using Itx.Flex.Client.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Itx.Flex.Client.ViewModel
{
  public class HandInSubmittingViewModel : BaseViewModel, IHandInSubmittingViewModel, IBaseViewModel
  {
    private readonly ILanguageService _languageService;
    private readonly IHandInUploadService _handInUploadService;
    private readonly IBoardingPassStorageService _boardingPassStorageService;
    private readonly IHandInFileService _handInFileService;
    private double _progressCurrent;
    private bool _showUrlToExam;
    private ClickablePathViewModel _clickablePathViewModel;
    private bool _submittedAfterDeadline;
    private string _handInSubmittingSubmissionRegisteredText;
    private string _handInSubmittingWarningCloseText;
    private string _handInSubmittingTaggedLateText;
    private string _handInSubmittingReceiptDetailsText;
    private string _handInSubmittingProgressStatusText;
    private string _handInSubmittingPendingQueueStatusText;
    private string _handInSubmittingUploadingStatusText;
    private string _handInSubmittingDoneStatusText;
    private string _handInSubmittingErrorStatusText;

    public HandInSubmittingViewModel(ILanguageService languageService, IMessenger messenger, IHandInUploadService handInUploadService, IBoardingPassStorageService boardingPassStorageService, IHandInFileService handInFileService)
    {
      this._languageService = languageService;
      this._handInUploadService = handInUploadService;
      this._boardingPassStorageService = boardingPassStorageService;
      this._handInFileService = handInFileService;
      this.UpdateLanguage((OnLanguageChanged) null);
      messenger.Register<OnLanguageChanged>((object) this, new Action<OnLanguageChanged>(this.UpdateLanguage));
      messenger.Register<OnHandInUploadProgressUpdated>((object) this, new Action<OnHandInUploadProgressUpdated>(this.OnHandInUploadProgressUpdated));
      messenger.Register<OnExaminationUrlLoaded>((object) this, new Action<OnExaminationUrlLoaded>(this.OnExaminationUrlLoaded));
    }

    private void OnExaminationUrlLoaded(OnExaminationUrlLoaded onExaminationUrlLoaded)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.ClickablePathViewModel = new ClickablePathViewModel(onExaminationUrlLoaded.ExaminationUrl, (string) null)));
    }

    public double ProgressMinimum
    {
      get
      {
        return 0.0;
      }
    }

    public double ProgressMaximum
    {
      get
      {
        return 100.0;
      }
    }

    public double ProgressCurrent
    {
      get
      {
        return this._progressCurrent;
      }
      set
      {
        this._progressCurrent = value;
        this.OnPropertyChanged(nameof (ProgressCurrent));
      }
    }

    private void OnHandInUploadProgressUpdated(OnHandInUploadProgressUpdated onHandInUploadProgressUpdated)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.ShowUrlToExam = onHandInUploadProgressUpdated.Step == OnHandInUploadProgressUpdated.UploadStep.Error;
        this.ProgressCurrent = Math.Max(onHandInUploadProgressUpdated.ProgressInPercent, this.ProgressCurrent);
        switch (onHandInUploadProgressUpdated.Step)
        {
          case OnHandInUploadProgressUpdated.UploadStep.Uploading:
            this.HandInSubmittingProgressStatusText = this.HandInSubmittingUploadingStatusText;
            break;
          case OnHandInUploadProgressUpdated.UploadStep.Done:
            this.HandInSubmittingProgressStatusText = this.HandInSubmittingDoneStatusText;
            break;
          case OnHandInUploadProgressUpdated.UploadStep.Error:
            this.HandInSubmittingProgressStatusText = this.HandInSubmittingErrorStatusText;
            break;
          default:
            this.HandInSubmittingProgressStatusText = this.HandInSubmittingPendingQueueStatusText;
            break;
        }
      }));
      if (onHandInUploadProgressUpdated.Step != OnHandInUploadProgressUpdated.UploadStep.Done)
        return;
      this.UploadFinished();
    }

    public void SubmittedHandIn(HandInStatus handInStatus, IEnumerable<SubmitHandInFileModel> submitHandInFiles)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.SubmittedAfterDeadline = handInStatus == HandInStatus.TaggedLate || handInStatus == HandInStatus.UploadedLate;
        this.ProgressCurrent = 0.0;
        this.UpdateLanguage((OnLanguageChanged) null);
      }));
      Task.Factory.StartNew((Action) (() => this._handInUploadService.Upload(submitHandInFiles)));
    }

    public void CancelUpload()
    {
      this._handInUploadService.StopUpload();
    }

    private void UploadFinished()
    {
      this._handInFileService.ClearStorage(this._boardingPassStorageService.GetExisting());
    }

    private void UpdateLanguage(OnLanguageChanged onLanguageChanged = null)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.HandInSubmittingSubmissionRegisteredText = this._languageService.GetString("HandInSubmittingSubmissionRegisteredText");
        this.HandInSubmittingWarningCloseText = this._languageService.GetString("HandInSubmittingWarningCloseText");
        this.HandInSubmittingReceiptDetailsText = this._languageService.GetString("HandInSubmittingReceiptDetailsText");
        this.HandInSubmittingTaggedLateText = this._languageService.GetString("HandInSubmittingTaggedLateText");
        this.HandInSubmittingPendingQueueStatusText = this._languageService.GetString("HandInSubmittingPendingQueueStatusText");
        this.HandInSubmittingDoneStatusText = this._languageService.GetString("HandInSubmittingDoneStatusText");
        this.HandInSubmittingErrorStatusText = this._languageService.GetString("HandInSubmittingErrorStatusText");
        this.HandInSubmittingUploadingStatusText = this._languageService.GetString("HandInSubmittingUploadingStatusText");
        this.HandInSubmittingProgressStatusText = this.HandInSubmittingPendingQueueStatusText;
      }));
    }

    public bool ShowUrlToExam
    {
      get
      {
        return this._showUrlToExam;
      }
      set
      {
        if (this._showUrlToExam == value)
          return;
        this._showUrlToExam = value;
        this.OnPropertyChanged(nameof (ShowUrlToExam));
      }
    }

    public ClickablePathViewModel ClickablePathViewModel
    {
      get
      {
        return this._clickablePathViewModel;
      }
      set
      {
        if (this._clickablePathViewModel == value)
          return;
        this._clickablePathViewModel = value;
        this.OnPropertyChanged(nameof (ClickablePathViewModel));
      }
    }

    public bool SubmittedAfterDeadline
    {
      get
      {
        return this._submittedAfterDeadline;
      }
      set
      {
        if (this._submittedAfterDeadline == value)
          return;
        this._submittedAfterDeadline = value;
        this.OnPropertyChanged(nameof (SubmittedAfterDeadline));
      }
    }

    public string HandInSubmittingSubmissionRegisteredText
    {
      get
      {
        return this._handInSubmittingSubmissionRegisteredText;
      }
      set
      {
        this._handInSubmittingSubmissionRegisteredText = value;
        this.OnPropertyChanged(nameof (HandInSubmittingSubmissionRegisteredText));
      }
    }

    public string HandInSubmittingWarningCloseText
    {
      get
      {
        return this._handInSubmittingWarningCloseText;
      }
      set
      {
        this._handInSubmittingWarningCloseText = value;
        this.OnPropertyChanged(nameof (HandInSubmittingWarningCloseText));
      }
    }

    public string HandInSubmittingTaggedLateText
    {
      get
      {
        return this._handInSubmittingTaggedLateText;
      }
      set
      {
        this._handInSubmittingTaggedLateText = value;
        this.OnPropertyChanged(nameof (HandInSubmittingTaggedLateText));
      }
    }

    public string HandInSubmittingReceiptDetailsText
    {
      get
      {
        return this._handInSubmittingReceiptDetailsText;
      }
      set
      {
        this._handInSubmittingReceiptDetailsText = value;
        this.OnPropertyChanged(nameof (HandInSubmittingReceiptDetailsText));
      }
    }

    public string HandInSubmittingProgressStatusText
    {
      get
      {
        return this._handInSubmittingProgressStatusText;
      }
      set
      {
        this._handInSubmittingProgressStatusText = value;
        this.OnPropertyChanged(nameof (HandInSubmittingProgressStatusText));
      }
    }

    private string HandInSubmittingPendingQueueStatusText
    {
      get
      {
        return this._handInSubmittingPendingQueueStatusText;
      }
      set
      {
        if (this._handInSubmittingPendingQueueStatusText == value)
          return;
        this._handInSubmittingPendingQueueStatusText = value;
        this.OnPropertyChanged(nameof (HandInSubmittingPendingQueueStatusText));
      }
    }

    private string HandInSubmittingUploadingStatusText
    {
      get
      {
        return this._handInSubmittingUploadingStatusText;
      }
      set
      {
        if (this._handInSubmittingUploadingStatusText == value)
          return;
        this._handInSubmittingUploadingStatusText = value;
        this.OnPropertyChanged(nameof (HandInSubmittingUploadingStatusText));
      }
    }

    public string HandInSubmittingDoneStatusText
    {
      get
      {
        return this._handInSubmittingDoneStatusText;
      }
      set
      {
        if (this._handInSubmittingDoneStatusText == value)
          return;
        this._handInSubmittingDoneStatusText = value;
        this.OnPropertyChanged(nameof (HandInSubmittingDoneStatusText));
      }
    }

    public string HandInSubmittingErrorStatusText
    {
      get
      {
        return this._handInSubmittingErrorStatusText;
      }
      set
      {
        if (this._handInSubmittingErrorStatusText == value)
          return;
        this._handInSubmittingErrorStatusText = value;
        this.OnPropertyChanged(nameof (HandInSubmittingErrorStatusText));
      }
    }
  }
}
