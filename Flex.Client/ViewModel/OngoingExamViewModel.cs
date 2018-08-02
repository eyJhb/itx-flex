// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.OngoingExamViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using Arcanic.ITX.Flex.WebserviceClient;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.Service;
using System;
using System.Windows.Input;

namespace Itx.Flex.Client.ViewModel
{
  public class OngoingExamViewModel : BaseViewModel, IOngoingExamViewModel, IBaseViewModel
  {
    private readonly ILanguageService _languageService;
    private readonly IMessenger _messenger;
    private readonly IFlexClient _flexClient;
    private string _ongoingExamNotYetAllowedToHandInText;
    private ClickablePathViewModel _urlToExternalSystemClickablePathViewModel;
    private string _ongoingExamOnlyExternalHandinEnabledText;
    private string _ongoingExamHeaderText;
    private string _ongoingExamExamTitleText;
    private string _examinationTitleDaDk;
    private string _examinationTitleEnGb;
    private string _ongoingExamSubmitBlankButtonText;
    private string _ongoingExamSubmitHandInButtonText;
    private bool _isHandInAllowed;
    private bool _isHandInEnabled;

    public PathLinkViewModel AssignmentFilesPathViewModel { get; }

    public PathLinkViewModel HandInPathViewModel { get; }

    public OngoingExamViewModel(ILanguageService languageService, IMessenger messenger, ILastSaveViewModel lastSaveViewModel, IExamTimeLeftViewModel examTimeLeftViewModel, IFlexClient flexClient)
    {
      this._languageService = languageService;
      this._messenger = messenger;
      this._flexClient = flexClient;
      this.LastSaveViewModel = lastSaveViewModel;
      this.ExamTimeLeftViewModel = examTimeLeftViewModel;
      this.AssignmentFilesPathViewModel = new PathLinkViewModel();
      this.HandInPathViewModel = new PathLinkViewModel();
      this.UpdateLanguage((OnLanguageChanged) null);
      messenger.Register<OnLanguageChanged>((object) this, new Action<OnLanguageChanged>(this.UpdateLanguage));
      messenger.Register<OnExaminationDataLoaded>((object) this, new Action<OnExaminationDataLoaded>(this.OnExaminationDataLoaded));
      messenger.Register<OnWorkspacePathsSet>((object) this, new Action<OnWorkspacePathsSet>(this.OnHandInPathSet));
      messenger.Register<OnEnableHandInReceived>((object) this, new Action<OnEnableHandInReceived>(this.OnEnableHandInReceived));
      messenger.Register<OnAllowHandInReceived>((object) this, new Action<OnAllowHandInReceived>(this.OnAllowHandInReceived));
      messenger.Register<OnExaminationUrlLoaded>((object) this, new Action<OnExaminationUrlLoaded>(this.OnExaminationUrlLoaded));
      this.IsHandInEnabled = true;
      this.SubmitBlankCommand = (ICommand) new RelayCommand((Action<object>) (c => this.SubmitBlankClick()), (Predicate<object>) null);
      this.SubmitHandInCommand = (ICommand) new RelayCommand((Action<object>) (c => this.SubmitHandInClick()), (Predicate<object>) null);
    }

    private void OnExaminationUrlLoaded(OnExaminationUrlLoaded onExaminationUrlLoaded)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.UrlToExternalSystemClickablePathViewModel = new ClickablePathViewModel(onExaminationUrlLoaded.ExaminationUrl, (string) null)));
    }

    private void OnAllowHandInReceived(OnAllowHandInReceived onAllowHandInReceived)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.IsHandInAllowed = onAllowHandInReceived.AllowHandin));
    }

    private void OnEnableHandInReceived(OnEnableHandInReceived onEnableHandInReceived)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.IsHandInEnabled = onEnableHandInReceived.EnableHandIn));
    }

    private void SubmitHandInClick()
    {
      this._messenger.Send<OnInitiateSubmitHandIn>(new OnInitiateSubmitHandIn());
    }

    private void SubmitBlankClick()
    {
      this._messenger.Send<OnOkCancelPopupOpened>(new OnOkCancelPopupOpened(new OkCancelPopupViewModel(this._languageService.GetString("OngoingExamConfirmSubmitBlankMessageText"), this._languageService.GetString("OngoingExamConfirmSubmitBlankButtonText"), this._languageService.GetString("OngoingExamCancelSubmitBlankButtonText"), this._messenger), (Action<bool>) (isOkSelected =>
      {
        if (!isOkSelected)
          return;
        HandInBlankResponse handInBlankResponse = this._flexClient.HandinBlank();
        if (handInBlankResponse.HandinStatus != HandInStatus.Tagged && handInBlankResponse.HandinStatus != HandInStatus.TaggedLate)
          return;
        this._messenger.Send<OnSubmitBlankHandIn>(new OnSubmitBlankHandIn(handInBlankResponse.HandinStatus.ToHandInStatus()));
      })));
    }

    private void OnHandInPathSet(OnWorkspacePathsSet onWorkspacePathsSet)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.AssignmentFilesPathViewModel.ClickablePathViewModel = new ClickablePathViewModel(onWorkspacePathsSet.AssignmentFilesPath, (string) null);
        this.HandInPathViewModel.ClickablePathViewModel = new ClickablePathViewModel(onWorkspacePathsSet.HandInPath, (string) null);
      }));
    }

    private void OnExaminationDataLoaded(OnExaminationDataLoaded onExaminationDataLoaded)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.ExaminationTitleDaDk = onExaminationDataLoaded.TitleDaDk;
        this.ExaminationTitleEnGb = onExaminationDataLoaded.TitleEnGb;
      }));
    }

    private void UpdateLanguage(OnLanguageChanged onLanguageChanged = null)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.OngoingExamHeaderText = this._languageService.GetString("OngoingExamHeaderText");
        this.AssignmentFilesPathViewModel.PathText = this._languageService.GetString("OngoingExamAssignmentFilesPathText");
        this.HandInPathViewModel.PathText = this._languageService.GetString("OngoingExamHandInPathText");
        this.OngoingExamExamTitleText = this._languageService.GetString("OngoingExamExamTitleText");
        this.OngoingExamSubmitHandInButtonText = this._languageService.GetString("OngoingExamSubmitHandInButtonText");
        this.OngoingExamSubmitBlankButtonText = this._languageService.GetString("OngoingExamSubmitBlankButtonText");
        this.OngoingExamOnlyExternalHandinEnabledText = this._languageService.GetString("OngoingExamOnlyExternalHandinEnabledText");
        this.OngoingExamNotYetAllowedToHandInText = this._languageService.GetString("OngoingExamNotYetAllowedToHandInText");
        this.OnPropertyChanged("ExamTitle");
      }));
    }

    public string OngoingExamNotYetAllowedToHandInText
    {
      get
      {
        return this._ongoingExamNotYetAllowedToHandInText;
      }
      set
      {
        if (this._ongoingExamNotYetAllowedToHandInText == value)
          return;
        this._ongoingExamNotYetAllowedToHandInText = value;
        this.OnPropertyChanged(nameof (OngoingExamNotYetAllowedToHandInText));
      }
    }

    public ClickablePathViewModel UrlToExternalSystemClickablePathViewModel
    {
      get
      {
        return this._urlToExternalSystemClickablePathViewModel;
      }
      set
      {
        if (this._urlToExternalSystemClickablePathViewModel == value)
          return;
        this._urlToExternalSystemClickablePathViewModel = value;
        this.OnPropertyChanged(nameof (UrlToExternalSystemClickablePathViewModel));
      }
    }

    public string OngoingExamOnlyExternalHandinEnabledText
    {
      get
      {
        return this._ongoingExamOnlyExternalHandinEnabledText;
      }
      set
      {
        if (this._ongoingExamOnlyExternalHandinEnabledText == value)
          return;
        this._ongoingExamOnlyExternalHandinEnabledText = value;
        this.OnPropertyChanged(nameof (OngoingExamOnlyExternalHandinEnabledText));
      }
    }

    public string OngoingExamHeaderText
    {
      get
      {
        return this._ongoingExamHeaderText;
      }
      set
      {
        this._ongoingExamHeaderText = value;
        this.OnPropertyChanged(nameof (OngoingExamHeaderText));
      }
    }

    public string OngoingExamExamTitleText
    {
      get
      {
        return this._ongoingExamExamTitleText;
      }
      set
      {
        this._ongoingExamExamTitleText = value;
        this.OnPropertyChanged(nameof (OngoingExamExamTitleText));
      }
    }

    private string ExaminationTitleDaDk
    {
      get
      {
        return this._examinationTitleDaDk;
      }
      set
      {
        this._examinationTitleDaDk = value;
        this.OnPropertyChanged(nameof (ExaminationTitleDaDk));
        this.OnPropertyChanged("ExamTitle");
      }
    }

    private string ExaminationTitleEnGb
    {
      get
      {
        return this._examinationTitleEnGb;
      }
      set
      {
        this._examinationTitleEnGb = value;
        this.OnPropertyChanged(nameof (ExaminationTitleEnGb));
        this.OnPropertyChanged("ExamTitle");
      }
    }

    public string ExamTitle
    {
      get
      {
        return this._languageService.SelectString(this.ExaminationTitleDaDk, this.ExaminationTitleEnGb);
      }
    }

    public string OngoingExamSubmitBlankButtonText
    {
      get
      {
        return this._ongoingExamSubmitBlankButtonText;
      }
      set
      {
        this._ongoingExamSubmitBlankButtonText = value;
        this.OnPropertyChanged(nameof (OngoingExamSubmitBlankButtonText));
      }
    }

    public string OngoingExamSubmitHandInButtonText
    {
      get
      {
        return this._ongoingExamSubmitHandInButtonText;
      }
      set
      {
        this._ongoingExamSubmitHandInButtonText = value;
        this.OnPropertyChanged(nameof (OngoingExamSubmitHandInButtonText));
      }
    }

    public bool IsHandInAllowed
    {
      get
      {
        return this._isHandInAllowed;
      }
      set
      {
        if (this._isHandInAllowed == value)
          return;
        this._isHandInAllowed = value;
        this.OnPropertyChanged(nameof (IsHandInAllowed));
        this.OnPropertyChanged("IsHandInNotAllowed");
      }
    }

    public bool IsHandInNotAllowed
    {
      get
      {
        if (this.IsHandInEnabled)
          return !this.IsHandInAllowed;
        return false;
      }
    }

    public bool IsHandInEnabled
    {
      get
      {
        return this._isHandInEnabled;
      }
      set
      {
        if (this._isHandInEnabled == value)
          return;
        this._isHandInEnabled = value;
        this.OnPropertyChanged(nameof (IsHandInEnabled));
        this.OnPropertyChanged("IsHandInDisabled");
        this.OnPropertyChanged("IsHandInNotAllowed");
      }
    }

    public bool IsHandInDisabled
    {
      get
      {
        return !this.IsHandInEnabled;
      }
    }

    public ICommand SubmitBlankCommand { get; }

    public ICommand SubmitHandInCommand { get; }

    public ILastSaveViewModel LastSaveViewModel { get; }

    public IExamTimeLeftViewModel ExamTimeLeftViewModel { get; }
  }
}
