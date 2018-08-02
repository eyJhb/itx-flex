// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.WorkSpaceViewModel
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
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace Itx.Flex.Client.ViewModel
{
  public class WorkSpaceViewModel : BaseViewModel, IWorkSpaceViewModel, IBaseViewModel
  {
    private readonly IMessenger _messenger;
    private readonly ISelectDirectoryService _selectDirectoryService;
    private readonly IWorkspaceStorageService _workspaceStorageService;
    private readonly IAssignmentFileService _assignmentFileService;
    private readonly IDirectoryService _directoryService;
    private readonly ILanguageService _languageService;
    private readonly IPinCodeStorageService _pinCodeStorageService;
    private readonly IBackupService _backupService;
    private readonly ISafePathService _safePathService;
    private readonly IPinCodeValidator _pinCodeValidator;
    private readonly ITimerService _examStartsTimer;
    private readonly IDirectoryAccessValidator _directoryAccessValidator;
    private readonly ITimerService _lockUiOnInvalidPinCodeTimerService;
    private readonly IBoardingPassStorageService _boardingPassStorageService;
    private readonly ITimerService _assignmentFilesRedownloadTimerService;
    private readonly ILoggerService _loggerService;
    private string _workspacePath;
    private string _assignmentFilesPath;
    private string _handInPath;
    private string _workspaceAssignmentFilesDownloadedText;
    private string _workspaceAssignmentFilesNoFilesText;
    private string _workspaceAssignmentFilesDownloadingText;
    private string _workspaceAssignmentFilesFailedDownloadingText;
    private double _minutesLeftUntilExamStart;
    private string _workspaceTimeUntilPluralText;
    private string _workspaceTimeUntilSingularText;
    private string _workspaceSelectFolderButtonText;
    private string _workspaceChooseWorkspaceText;
    private string _workspaceAssignmentFilesPathText;
    private string _workspaceHandInPathText;
    private string _workspaceAssignmentFilesLockedText;
    private string _workspaceDirectoryAccessDeniedErrorText;
    private string _workspaceDirectoryAccessDeniedErrorButtonText;
    private DownloadState _downloadState;
    private ClickablePathViewModel _clickablePathViewModel;

    public WorkSpaceViewModel(IMessenger messenger, ISelectDirectoryService selectDirectoryService, IWorkspaceStorageService workspaceStorageService, IAssignmentFileService assignmentFileService, IDirectoryService directoryService, ILanguageService languageService, IPinCodeStorageService pinCodeStorageService, IBackupService backupService, ISafePathService safePathService, IPinCodeValidator pinCodeValidator, ITimerService examStartsTimer, IDirectoryAccessValidator directoryAccessValidator, ITimerService lockUiOnInvalidPinCodeTimerService, IBoardingPassStorageService boardingPassStorageService, ITimerService assignmentFilesRedownloadTimerService, ILoggerService loggerService)
    {
      this._messenger = messenger;
      this._selectDirectoryService = selectDirectoryService;
      this._workspaceStorageService = workspaceStorageService;
      this._assignmentFileService = assignmentFileService;
      this._directoryService = directoryService;
      this._languageService = languageService;
      this._pinCodeStorageService = pinCodeStorageService;
      this._backupService = backupService;
      this._safePathService = safePathService;
      this._pinCodeValidator = pinCodeValidator;
      this._examStartsTimer = examStartsTimer;
      this._directoryAccessValidator = directoryAccessValidator;
      this._lockUiOnInvalidPinCodeTimerService = lockUiOnInvalidPinCodeTimerService;
      this._boardingPassStorageService = boardingPassStorageService;
      this._assignmentFilesRedownloadTimerService = assignmentFilesRedownloadTimerService;
      this._loggerService = loggerService;
      this._examStartsTimer.AutoReset = false;
      this._examStartsTimer.Elapsed += (ElapsedEventHandler) ((sender, args) => this.ExamHasStarted());
      this._assignmentFilesRedownloadTimerService.AutoReset = false;
      this._assignmentFilesRedownloadTimerService.Elapsed += new ElapsedEventHandler(this.AssignmentFilesRedownloadTimerService_Elapsed);
      this._assignmentFilesRedownloadTimerService.Interval = 600000.0;
      this.SetWorkspacePathCommand = (ICommand) new RelayCommand((Action<object>) (c => this.SetWorkspaceClick()), (Predicate<object>) null);
      this.ShowUnlockAssignmentFilesCommand = (ICommand) new RelayCommand((Action<object>) (c => this.ShowUnlockAssignmentFilesClick()), (Predicate<object>) null);
      this.UpdateLanguage((OnLanguageChanged) null);
      messenger.Register<OnLanguageChanged>((object) this, new Action<OnLanguageChanged>(this.UpdateLanguage));
      messenger.Register<OnTimeLeftUntilExamStarts>((object) this, new Action<OnTimeLeftUntilExamStarts>(this.OnTimeLeftUntilExamStarts));
      messenger.Register<OnExaminationDataLoaded>((object) this, new Action<OnExaminationDataLoaded>(this.OnExaminationDataLoaded));
      messenger.Register<OnPinCodeLoginSuccessful>((object) this, new Action<OnPinCodeLoginSuccessful>(this.OnPinCodeLoginSuccessful));
      messenger.Register<OnPinCodeLoginForceContinue>((object) this, new Action<OnPinCodeLoginForceContinue>(this.OnPinCodeLoginForceContinue));
      messenger.Register<OnExaminationUrlLoaded>((object) this, new Action<OnExaminationUrlLoaded>(this.OnExaminationUrlLoaded));
    }

    private void OnExaminationUrlLoaded(OnExaminationUrlLoaded onExaminationUrlLoaded)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.ClickablePathViewModel = new ClickablePathViewModel(onExaminationUrlLoaded.ExaminationUrl, (string) null)));
    }

    private void OnPinCodeLoginForceContinue(OnPinCodeLoginForceContinue onPinCodeLoginForceContinue)
    {
      Task.Factory.StartNew((Action) (() =>
      {
        this.InitializeWorkspaceDirectories();
        this.StoreWorkspaceInformation(onPinCodeLoginForceContinue.PinCode);
        this.UpdateState(this.AssignmentFilesPath, this.HandInPath);
      }));
    }

    private void ExamHasStarted()
    {
      this.UpdateTimeUntilExamStart(0);
    }

    private void OnPinCodeLoginSuccessful(OnPinCodeLoginSuccessful onPinCodeLoginSuccessful)
    {
      Task.Factory.StartNew((Action) (() =>
      {
        this.InitializeWorkspaceDirectories();
        this.StoreWorkspaceInformation(onPinCodeLoginSuccessful.PinCode);
        this.AssignmentFileMetadatas = onPinCodeLoginSuccessful.RedownloadedAssignmentFileMetadatas ?? this.AssignmentFileMetadatas;
        if (this.AssignmentFileMetadatas != null)
          this._assignmentFileService.MoveDecryptedAssignmentFilesToWorkspace(onPinCodeLoginSuccessful.AssignmentDecryptionKeys, this.AssignmentFileMetadatas, this.AssignmentFilesPath);
        this.UpdateState(this.AssignmentFilesPath, this.HandInPath);
      }));
    }

    private void StoreWorkspaceInformation(string pinCode)
    {
      this._workspaceStorageService.Store(this.WorkspacePath, this.AssignmentFilesPath, this.HandInPath);
      this._pinCodeStorageService.Store(pinCode);
    }

    private void InitializeWorkspaceDirectories()
    {
      this._directoryService.CreateDirectory(this.AssignmentFilesPath);
      this._directoryService.CreateDirectory(this.HandInPath);
    }

    public bool HasVerifiedPinCode()
    {
      if (this._boardingPassStorageService.HasExisting() && this._pinCodeStorageService.HasExisting())
        return this._workspaceStorageService.HasExisting();
      return false;
    }

    private void UpdateState(string actualAssignmentFilesPath, string actualHandInPath)
    {
      this._backupService.StartBackup(actualHandInPath);
      this._messenger.Send<OnWorkspacePathsSet>(new OnWorkspacePathsSet(actualAssignmentFilesPath, actualHandInPath));
      this._messenger.Send<OnExamStarted>(new OnExamStarted());
    }

    public void UpdateState()
    {
      WorkspacePaths existing = this._workspaceStorageService.GetExisting();
      this.UpdateState(existing.AssignmentFilesPath, existing.HandInPath);
    }

    private string ExaminationTitleDaDk { get; set; }

    private string ExaminationTitleEnGb { get; set; }

    private DateTime ExaminationStart { get; set; }

    private void OnExaminationDataLoaded(OnExaminationDataLoaded onExaminationDataLoaded)
    {
      if (!this.HasVerifiedPinCode())
        Task.Factory.StartNew((Action) (() =>
        {
          this.UpdateAssignmentFiles();
          this._assignmentFilesRedownloadTimerService.Start();
        }));
      this.ExaminationTitleDaDk = onExaminationDataLoaded.TitleDaDk;
      this.ExaminationTitleEnGb = onExaminationDataLoaded.TitleEnGb;
      this.ExaminationStart = onExaminationDataLoaded.Start;
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.MaxTimeUntilExamStart = (double) onExaminationDataLoaded.EarliestLoginTimeBeforeStartInMinutes));
    }

    private void UpdateAssignmentFiles()
    {
      try
      {
        this.AssignmentFileMetadatas = this._assignmentFileService.SetupAssignmentFileMetadatas();
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
        {
          if (this.AssignmentFileMetadatas.Any<AssignmentFileMetadata>())
            this.DownloadState = DownloadState.Succeeded;
          else
            this.DownloadState = DownloadState.NoAssignmentFiles;
        }));
      }
      catch (Exception ex)
      {
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.DownloadState = DownloadState.Failed));
      }
    }

    private void AssignmentFilesRedownloadTimerService_Elapsed(object sender, ElapsedEventArgs e)
    {
      if (this.MinutesLeftUntilExamStart < 2.0)
        return;
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.DownloadState = DownloadState.Ongoing;
        Task.Factory.StartNew((Action) (() =>
        {
          this.UpdateAssignmentFiles();
          if (this.MinutesLeftUntilExamStart < 10.0)
            return;
          this._assignmentFilesRedownloadTimerService.Start();
        }));
      }));
    }

    private void OnTimeLeftUntilExamStarts(OnTimeLeftUntilExamStarts onTimeLeftUntilExamStarts)
    {
      if (onTimeLeftUntilExamStarts.ExaminationStartInSeconds > 0)
      {
        int num = 1000;
        this._examStartsTimer.Interval = (double) (onTimeLeftUntilExamStarts.ExaminationStartInSeconds * 1000 + num);
        this._examStartsTimer.Start();
      }
      this.UpdateTimeUntilExamStart((onTimeLeftUntilExamStarts.ExaminationStartInSeconds + 59) / 60);
    }

    private void UpdateTimeUntilExamStart(int timeUntilExamStartInMinutes)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.MinutesLeftUntilExamStart = (double) timeUntilExamStartInMinutes));
    }

    private void ShowUnlockAssignmentFilesClick()
    {
      IMessenger messenger1 = this._messenger;
      OnPinCodePopupOpened message = new OnPinCodePopupOpened();
      IMessenger messenger2 = this._messenger;
      IPinCodeValidator pinCodeValidator = this._pinCodeValidator;
      IAssignmentFileService assignmentFileService = this._assignmentFileService;
      ILanguageService languageService = this._languageService;
      ITimerService codeTimerService = this._lockUiOnInvalidPinCodeTimerService;
      IEnumerable<AssignmentFileMetadata> assignmentFileMetadatas = this.AssignmentFileMetadatas;
      int num = assignmentFileMetadatas != null ? (assignmentFileMetadatas.Any<AssignmentFileMetadata>() ? 1 : 0) : 0;
      ClickablePathViewModel clickablePathViewModel = this.ClickablePathViewModel;
      message.PinCodePopupViewModel = new PinCodePopupViewModel(messenger2, pinCodeValidator, assignmentFileService, languageService, codeTimerService, num != 0, clickablePathViewModel);
      messenger1.Send<OnPinCodePopupOpened>(message);
    }

    private IEnumerable<AssignmentFileMetadata> AssignmentFileMetadatas { get; set; }

    public ICommand SetWorkspacePathCommand { get; set; }

    public ICommand ShowUnlockAssignmentFilesCommand { get; set; }

    public string WorkspacePath
    {
      get
      {
        return this._workspacePath;
      }
      set
      {
        this._workspacePath = value;
        this.OnPropertyChanged(nameof (WorkspacePath));
        this.OnPropertyChanged("ShowButtonToUnlockAssignmentFiles");
      }
    }

    private string WorkspaceAssignmentFilesDirectoryText { get; set; }

    public string AssignmentFilesPath
    {
      get
      {
        return this._assignmentFilesPath;
      }
      set
      {
        this._assignmentFilesPath = value;
        this.OnPropertyChanged(nameof (AssignmentFilesPath));
      }
    }

    private string WorkspaceHandInDirectoryText { get; set; }

    public string HandInPath
    {
      get
      {
        return this._handInPath;
      }
      set
      {
        this._handInPath = value;
        this.OnPropertyChanged(nameof (HandInPath));
      }
    }

    private string WorkspaceBasePath { get; set; }

    private void SetWorkspaceClick()
    {
      DirectoryResult selectedDirectory = (DirectoryResult) null;
      try
      {
        selectedDirectory = this._selectDirectoryService.GetDirectoryLocation(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
      }
      catch (Exception ex)
      {
        this._loggerService.Log(LogType.Error, "Selecting folder resulted in an exception. " + ex.Message, ex.StackTrace);
        this._messenger.Send<OnWorkspaceDirectoryAccessDeniedPopupOpened>(new OnWorkspaceDirectoryAccessDeniedPopupOpened(new OkPopupViewModel(this.WorkspaceDirectoryAccessDeniedErrorText, this.WorkspaceDirectoryAccessDeniedErrorButtonText, this._messenger)));
      }
      DirectoryResult directoryResult = selectedDirectory;
      if ((directoryResult != null ? (directoryResult.HasSelected ? 1 : 0) : 0) == 0)
        return;
      Task.Factory.StartNew((Action) (() =>
      {
        bool canWriteToDirectory = this._directoryAccessValidator.HasWriteAccess(selectedDirectory.Path);
        bool isPathLengthAcceptable = this._directoryAccessValidator.IsPathLengthAcceptable(selectedDirectory.Path);
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
        {
          if (canWriteToDirectory & isPathLengthAcceptable)
          {
            this.WorkspaceBasePath = selectedDirectory.Path;
            this.UpdateWorkspace(this.WorkspaceBasePath);
          }
          else
            this._messenger.Send<OnWorkspaceDirectoryAccessDeniedPopupOpened>(new OnWorkspaceDirectoryAccessDeniedPopupOpened(new OkPopupViewModel(this.WorkspaceDirectoryAccessDeniedErrorText, this.WorkspaceDirectoryAccessDeniedErrorButtonText, this._messenger)));
        }));
      }));
    }

    private void UpdateWorkspace(string newWorkspaceBasePath)
    {
      if (string.IsNullOrEmpty(newWorkspaceBasePath))
        return;
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.WorkspacePath = newWorkspaceBasePath + "\\" + this._safePathService.MakeValidFilename(this._languageService.SelectString(this.ExaminationTitleDaDk, this.ExaminationTitleEnGb), new char?('_')) + " " + this.ExaminationStart.ToString("dd-MM-yyyy");
        this.AssignmentFilesPath = this.WorkspacePath + "\\" + this.WorkspaceAssignmentFilesDirectoryText;
        this.HandInPath = this.WorkspacePath + "\\" + this.WorkspaceHandInDirectoryText;
      }));
    }

    public string WorkspaceAssignmentFilesStatusText
    {
      get
      {
        switch (this.DownloadState)
        {
          case DownloadState.Ongoing:
            return this.WorkspaceAssignmentFilesDownloadingText;
          case DownloadState.Failed:
            return this.WorkspaceAssignmentFilesFailedDownloadingText;
          case DownloadState.Succeeded:
            return this.WorkspaceAssignmentFilesDownloadedText;
          case DownloadState.NoAssignmentFiles:
            return this.WorkspaceAssignmentFilesNoFilesText;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    private string WorkspaceAssignmentFilesDownloadedText
    {
      get
      {
        return this._workspaceAssignmentFilesDownloadedText;
      }
      set
      {
        this._workspaceAssignmentFilesDownloadedText = value;
        this.OnPropertyChanged(nameof (WorkspaceAssignmentFilesDownloadedText));
        this.OnPropertyChanged("WorkspaceAssignmentFilesStatusText");
      }
    }

    private string WorkspaceAssignmentFilesNoFilesText
    {
      get
      {
        return this._workspaceAssignmentFilesNoFilesText;
      }
      set
      {
        this._workspaceAssignmentFilesNoFilesText = value;
        this.OnPropertyChanged(nameof (WorkspaceAssignmentFilesNoFilesText));
        this.OnPropertyChanged("WorkspaceAssignmentFilesStatusText");
      }
    }

    private string WorkspaceAssignmentFilesDownloadingText
    {
      get
      {
        return this._workspaceAssignmentFilesDownloadingText;
      }
      set
      {
        this._workspaceAssignmentFilesDownloadingText = value;
        this.OnPropertyChanged(nameof (WorkspaceAssignmentFilesDownloadingText));
        this.OnPropertyChanged("WorkspaceAssignmentFilesStatusText");
      }
    }

    private string WorkspaceAssignmentFilesFailedDownloadingText
    {
      get
      {
        return this._workspaceAssignmentFilesFailedDownloadingText;
      }
      set
      {
        this._workspaceAssignmentFilesFailedDownloadingText = value;
        this.OnPropertyChanged(nameof (WorkspaceAssignmentFilesFailedDownloadingText));
        this.OnPropertyChanged("WorkspaceAssignmentFilesStatusText");
      }
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

    private double MaxTimeUntilExamStart { get; set; }

    public double ProgressBarCurrent
    {
      get
      {
        return (1.0 - this.MinutesLeftUntilExamStart / this.MaxTimeUntilExamStart) * this.ProgressBarMaximum;
      }
    }

    private double MinutesLeftUntilExamStart
    {
      get
      {
        return this._minutesLeftUntilExamStart;
      }
      set
      {
        this._minutesLeftUntilExamStart = value;
        this.OnPropertyChanged(nameof (MinutesLeftUntilExamStart));
        this.OnPropertyChanged("ProgressBarCurrent");
        this.OnPropertyChanged("ProgressBarTimeUntilText");
        this.OnPropertyChanged("ShowButtonToUnlockAssignmentFiles");
      }
    }

    public string WorkspaceTimeUntilPluralText
    {
      get
      {
        return this._workspaceTimeUntilPluralText;
      }
      set
      {
        this._workspaceTimeUntilPluralText = value;
        this.OnPropertyChanged(nameof (WorkspaceTimeUntilPluralText));
        this.OnPropertyChanged("ProgressBarTimeUntilText");
      }
    }

    public string WorkspaceTimeUntilSingularText
    {
      get
      {
        return this._workspaceTimeUntilSingularText;
      }
      set
      {
        this._workspaceTimeUntilSingularText = value;
        this.OnPropertyChanged(nameof (WorkspaceTimeUntilSingularText));
        this.OnPropertyChanged("ProgressBarTimeUntilText");
      }
    }

    public string ProgressBarTimeUntilText
    {
      get
      {
        if (this.MinutesLeftUntilExamStart < 1.0 || this.MinutesLeftUntilExamStart > 2.0)
          return string.Format(this.WorkspaceTimeUntilPluralText, (object) this.MinutesLeftUntilExamStart);
        return this.WorkspaceTimeUntilSingularText;
      }
    }

    public string WorkspaceShowAssignmentFilesUnlockButtonText
    {
      get
      {
        if (this.DownloadState != DownloadState.NoAssignmentFiles && this.DownloadState != DownloadState.Failed)
          return this._languageService.GetString(nameof (WorkspaceShowAssignmentFilesUnlockButtonText));
        return this._languageService.GetString("WorkspaceShowPinCodeUnlockWithoutAssignmentFilesButtonText");
      }
    }

    public bool ShowButtonToUnlockAssignmentFiles
    {
      get
      {
        if ((int) this.MinutesLeftUntilExamStart <= 0 && !string.IsNullOrEmpty(this.WorkspaceBasePath))
          return (uint) this.DownloadState > 0U;
        return false;
      }
    }

    public string WorkspaceSelectFolderButtonText
    {
      get
      {
        return this._workspaceSelectFolderButtonText;
      }
      set
      {
        this._workspaceSelectFolderButtonText = value;
        this.OnPropertyChanged(nameof (WorkspaceSelectFolderButtonText));
      }
    }

    public string WorkspaceChooseWorkspaceText
    {
      get
      {
        return this._workspaceChooseWorkspaceText;
      }
      set
      {
        this._workspaceChooseWorkspaceText = value;
        this.OnPropertyChanged(nameof (WorkspaceChooseWorkspaceText));
      }
    }

    public string WorkspaceAssignmentFilesPathText
    {
      get
      {
        return this._workspaceAssignmentFilesPathText;
      }
      set
      {
        this._workspaceAssignmentFilesPathText = value;
        this.OnPropertyChanged(nameof (WorkspaceAssignmentFilesPathText));
      }
    }

    public string WorkspaceHandInPathText
    {
      get
      {
        return this._workspaceHandInPathText;
      }
      set
      {
        this._workspaceHandInPathText = value;
        this.OnPropertyChanged(nameof (WorkspaceHandInPathText));
      }
    }

    public string WorkspaceAssignmentFilesLockedText
    {
      get
      {
        return this._workspaceAssignmentFilesLockedText;
      }
      set
      {
        this._workspaceAssignmentFilesLockedText = value;
        this.OnPropertyChanged(nameof (WorkspaceAssignmentFilesLockedText));
      }
    }

    public string WorkspaceDirectoryAccessDeniedErrorText
    {
      get
      {
        return this._workspaceDirectoryAccessDeniedErrorText;
      }
      set
      {
        this._workspaceDirectoryAccessDeniedErrorText = value;
        this.OnPropertyChanged(nameof (WorkspaceDirectoryAccessDeniedErrorText));
      }
    }

    public string WorkspaceDirectoryAccessDeniedErrorButtonText
    {
      get
      {
        return this._workspaceDirectoryAccessDeniedErrorButtonText;
      }
      set
      {
        this._workspaceDirectoryAccessDeniedErrorButtonText = value;
        this.OnPropertyChanged(nameof (WorkspaceDirectoryAccessDeniedErrorButtonText));
      }
    }

    public DownloadState DownloadState
    {
      get
      {
        return this._downloadState;
      }
      set
      {
        this._downloadState = value;
        this.OnPropertyChanged(nameof (DownloadState));
        this.OnPropertyChanged("WorkspaceAssignmentFilesStatusText");
        this.OnPropertyChanged("ShowButtonToUnlockAssignmentFiles");
        this.OnPropertyChanged("WorkspaceShowAssignmentFilesUnlockButtonText");
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

    private void UpdateLanguage(OnLanguageChanged onLanguageChanged = null)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.WorkspaceChooseWorkspaceText = this._languageService.GetString("WorkspaceChooseWorkspaceText");
        this.WorkspaceSelectFolderButtonText = this._languageService.GetString("WorkspaceSelectFolderButtonText");
        this.WorkspaceAssignmentFilesPathText = this._languageService.GetString("WorkspaceAssignmentFilesPathText");
        this.WorkspaceHandInPathText = this._languageService.GetString("WorkspaceHandInPathText");
        this.WorkspaceTimeUntilSingularText = this._languageService.GetString("WorkspaceTimeUntilSingularText");
        this.WorkspaceTimeUntilPluralText = this._languageService.GetString("WorkspaceTimeUntilPluralText");
        this.WorkspaceAssignmentFilesLockedText = this._languageService.GetString("WorkspaceAssignmentFilesLockedText");
        this.WorkspaceAssignmentFilesDownloadedText = this._languageService.GetString("WorkspaceAssignmentFilesDownloadedText");
        this.WorkspaceAssignmentFilesNoFilesText = this._languageService.GetString("WorkspaceAssignmentFilesNoFilesText");
        this.WorkspaceAssignmentFilesDownloadingText = this._languageService.GetString("WorkspaceAssignmentFilesDownloadingText");
        this.WorkspaceAssignmentFilesFailedDownloadingText = this._languageService.GetString("WorkspaceAssignmentFilesFailedDownloadingText");
        this.WorkspaceHandInDirectoryText = this._languageService.GetString("WorkspaceHandInDirectoryText");
        this.WorkspaceDirectoryAccessDeniedErrorButtonText = this._languageService.GetString("WorkspaceDirectoryAccessDeniedErrorButtonText");
        this.WorkspaceDirectoryAccessDeniedErrorText = this._languageService.GetString("WorkspaceDirectoryAccessDeniedErrorText");
        this.WorkspaceAssignmentFilesDirectoryText = this._languageService.GetString("WorkspaceAssignmentFilesDirectoryText");
        this.UpdateWorkspace(this.WorkspaceBasePath);
        this.OnPropertyChanged("WorkspaceShowAssignmentFilesUnlockButtonText");
      }));
    }
  }
}
