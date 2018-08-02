// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.HandInReceivedViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.Model;
using Itx.Flex.Client.Service;
using System;
using System.Windows.Input;

namespace Itx.Flex.Client.ViewModel
{
  public class HandInReceivedViewModel : BaseViewModel, IHandInReceivedViewModel, IBaseViewModel
  {
    private readonly ILanguageService _languageService;
    private readonly IMessenger _messenger;
    private string _handInReceivedTypeHandinHeaderText;
    private string _handInReceivedTypeBlankHeaderText;
    private string _handInReceivedUploadedLateWarningText;
    private string _handInReceivedDoneText;
    private string _handInReceivedSurveillanceText;
    private string _handInReceivedEndProgramButtonText;
    private bool _submittedAfterDeadline;
    private string _handInReceivedExaminationUrlText;
    private ClickablePathViewModel _clickablePathViewModel;

    public HandInReceivedViewModel(ILanguageService languageService, IMessenger messenger)
    {
      this._languageService = languageService;
      this._messenger = messenger;
      this.UpdateLanguage((OnLanguageChanged) null);
      messenger.Register<OnLanguageChanged>((object) this, new Action<OnLanguageChanged>(this.UpdateLanguage));
      this.EndProgramCommand = (ICommand) new RelayCommand((Action<object>) (c => this.EndProgramClick()), (Predicate<object>) null);
      messenger.Register<OnExaminationUrlLoaded>((object) this, new Action<OnExaminationUrlLoaded>(this.OnExaminationUrlLoaded));
    }

    private void OnExaminationUrlLoaded(OnExaminationUrlLoaded onExaminationUrlLoaded)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.ClickablePathViewModel = new ClickablePathViewModel(onExaminationUrlLoaded.ExaminationUrl, (string) null)));
    }

    private void EndProgramClick()
    {
      this._messenger.Send<OnClosingProgramRequested>(new OnClosingProgramRequested());
    }

    public ICommand EndProgramCommand { get; set; }

    private void UpdateLanguage(OnLanguageChanged onLanguageChanged = null)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.HandInReceivedDoneText = this._languageService.GetString("HandInReceivedDoneText");
        this.HandInReceivedEndProgramButtonText = this._languageService.GetString("HandInReceivedEndProgramButtonText");
        this.HandInReceivedTypeHandinHeaderText = this._languageService.GetString("HandInReceivedTypeHandinHeaderText");
        this.HandInReceivedTypeBlankHeaderText = this._languageService.GetString("HandInReceivedTypeBlankHeaderText");
        this.HandInReceivedUploadedLateWarningText = this._languageService.GetString("HandInReceivedUploadedLateWarningText");
        this.HandInReceivedSurveillanceText = this._languageService.GetString("HandInReceivedSurveillanceText");
        this.HandInReceivedExaminationUrlText = this._languageService.GetString("HandInReceivedExaminationUrlText");
      }));
    }

    public string HeaderText
    {
      get
      {
        switch (this.HandInType)
        {
          case HandInType.NoSubmission:
            return "No Submission!!!";
          case HandInType.HandIn:
            return this.HandInReceivedTypeHandinHeaderText;
          case HandInType.Blank:
            return this.HandInReceivedTypeBlankHeaderText;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    private HandInType HandInType { get; set; }

    public string HandInReceivedTypeHandinHeaderText
    {
      get
      {
        return this._handInReceivedTypeHandinHeaderText;
      }
      set
      {
        this._handInReceivedTypeHandinHeaderText = value;
        this.OnPropertyChanged(nameof (HandInReceivedTypeHandinHeaderText));
        this.OnPropertyChanged("HeaderText");
      }
    }

    public string HandInReceivedTypeBlankHeaderText
    {
      get
      {
        return this._handInReceivedTypeBlankHeaderText;
      }
      set
      {
        this._handInReceivedTypeBlankHeaderText = value;
        this.OnPropertyChanged(nameof (HandInReceivedTypeBlankHeaderText));
        this.OnPropertyChanged("HeaderText");
      }
    }

    public string HandInReceivedUploadedLateWarningText
    {
      get
      {
        return this._handInReceivedUploadedLateWarningText;
      }
      set
      {
        this._handInReceivedUploadedLateWarningText = value;
        this.OnPropertyChanged(nameof (HandInReceivedUploadedLateWarningText));
      }
    }

    public string HandInReceivedDoneText
    {
      get
      {
        return this._handInReceivedDoneText;
      }
      set
      {
        this._handInReceivedDoneText = value;
        this.OnPropertyChanged(nameof (HandInReceivedDoneText));
      }
    }

    public string HandInReceivedSurveillanceText
    {
      get
      {
        return this._handInReceivedSurveillanceText;
      }
      set
      {
        this._handInReceivedSurveillanceText = value;
        this.OnPropertyChanged(nameof (HandInReceivedSurveillanceText));
      }
    }

    public string HandInReceivedEndProgramButtonText
    {
      get
      {
        return this._handInReceivedEndProgramButtonText;
      }
      set
      {
        this._handInReceivedEndProgramButtonText = value;
        this.OnPropertyChanged(nameof (HandInReceivedEndProgramButtonText));
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

    public string HandInReceivedExaminationUrlText
    {
      get
      {
        return this._handInReceivedExaminationUrlText;
      }
      set
      {
        if (this._handInReceivedExaminationUrlText == value)
          return;
        this._handInReceivedExaminationUrlText = value;
        this.OnPropertyChanged(nameof (HandInReceivedExaminationUrlText));
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

    public void Submitted(HandInType handInType, HandInStatus handInStatus)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.HandInType = handInType;
        this.SubmittedAfterDeadline = handInType == HandInType.HandIn && (handInStatus == HandInStatus.TaggedLate || handInStatus == HandInStatus.UploadedLate);
      }));
    }
  }
}
