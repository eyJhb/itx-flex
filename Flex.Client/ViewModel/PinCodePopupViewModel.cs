// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.PinCodePopupViewModel
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
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace Itx.Flex.Client.ViewModel
{
  public class PinCodePopupViewModel : BaseViewModel, IDataErrorInfo
  {
    private readonly IMessenger _messenger;
    private readonly IPinCodeValidator _pinCodeValidator;
    private readonly IAssignmentFileService _assignmentFileService;
    private readonly ILanguageService _languageService;
    private readonly ITimerService _lockUiOnInvalidPinCodeTimerService;
    private ClickablePathViewModel _clickablePathViewModel;
    private bool _isPinCodeDisabled;
    private string _workspaceSubmitPinCodeErrorText;
    private string _workspaceUnlockAssignmentFilesInformationText;
    private string _workspaceUnlockAssignmentFilesButtonText;
    private string _pinCode;
    private bool _showPinCodeTextboxError;
    private bool _isForceContinueVisible;
    private string _validatedPinCode;

    public PinCodePopupViewModel(IMessenger messenger, IPinCodeValidator pinCodeValidator, IAssignmentFileService assignmentFileService, ILanguageService languageService, ITimerService lockUiOnInvalidPinCodeTimerService, bool hasAssignmentFiles, ClickablePathViewModel clickablePathViewModel)
    {
      this._messenger = messenger;
      this._pinCodeValidator = pinCodeValidator;
      this._assignmentFileService = assignmentFileService;
      this._languageService = languageService;
      this._lockUiOnInvalidPinCodeTimerService = lockUiOnInvalidPinCodeTimerService;
      this._lockUiOnInvalidPinCodeTimerService.Stop();
      this._lockUiOnInvalidPinCodeTimerService.AutoReset = false;
      this._lockUiOnInvalidPinCodeTimerService.Interval = 5000.0;
      this._lockUiOnInvalidPinCodeTimerService.Elapsed += new ElapsedEventHandler(this.LockUiOnInvalidPinCodeTimerService_Elapsed);
      this.UnlockAssignmentFilesCommand = (ICommand) new RelayCommand((Action<object>) (c => this.UnlockAssignmentFiles()), (Predicate<object>) null);
      this.ForceContinueCommand = (ICommand) new RelayCommand((Action<object>) (c => this.ForceContinue()), (Predicate<object>) null);
      this.UpdateLanguage(hasAssignmentFiles);
      this.ClickablePathViewModel = clickablePathViewModel;
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

    private void UpdateLanguage(bool hasAssignmentFiles)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.WorkspaceUnlockAssignmentFilesInformationText = this._languageService.GetString("WorkspaceUnlockAssignmentFilesInformationText");
        this.WorkspaceUnlockAssignmentFilesButtonText = hasAssignmentFiles ? this._languageService.GetString("WorkspaceUnlockAssignmentFilesButtonText") : this._languageService.GetString("WorkspacePinCodeStartExamWithoutAssignmentFilesButtonText");
      }));
    }

    private void ForceContinue()
    {
      this._messenger.Send<OnPinCodeLoginForceContinue>(new OnPinCodeLoginForceContinue(this._validatedPinCode));
    }

    private void LockUiOnInvalidPinCodeTimerService_Elapsed(object sender, ElapsedEventArgs e)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.WorkspaceSubmitPinCodeErrorText = this._languageService.GetString("WorkspacePinCodeInvalidErrorText");
        this.IsPinCodeDisabled = false;
      }));
      this._messenger.Send<OnAllowClosingPinCodePopup>(new OnAllowClosingPinCodePopup(true));
    }

    public bool CanUnlockFiles
    {
      get
      {
        return !this.IsPinCodeDisabled;
      }
    }

    public bool IsPinCodeDisabled
    {
      get
      {
        return this._isPinCodeDisabled;
      }
      set
      {
        this._isPinCodeDisabled = value;
        this.OnPropertyChanged(nameof (IsPinCodeDisabled));
        this.OnPropertyChanged("CanUnlockFiles");
      }
    }

    private void UnlockAssignmentFiles()
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.IsForceContinueVisible = false;
        this.ShowPinCodeTextboxError = true;
        this.IsPinCodeDisabled = true;
        Task.Factory.StartNew((Action) (() =>
        {
          if (!this._pinCodeValidator.Validate(this.PinCode).IsValid)
          {
            DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.IsPinCodeDisabled = false));
          }
          else
          {
            List<AssignmentDecryptionKeyModel> assignmentDecryptionKeys = this.GetAssignmentDecryptionKeys();
            if (assignmentDecryptionKeys == null)
              return;
            this._validatedPinCode = this.PinCode;
            this.ValidateAssignmentFiles(assignmentDecryptionKeys, this.PinCode);
          }
        }));
      }));
    }

    private void ValidateAssignmentFiles(List<AssignmentDecryptionKeyModel> assignmentDecryptionKeys, string pinCode)
    {
      try
      {
        IEnumerable<AssignmentFileMetadata> redownloadedAssignmentFileMetadatas = this._assignmentFileService.DownloadAnyNewAssignmentFiles((IEnumerable<AssignmentDecryptionKeyModel>) assignmentDecryptionKeys);
        this._messenger.Send<OnPinCodeLoginSuccessful>(new OnPinCodeLoginSuccessful(pinCode, (IEnumerable<AssignmentDecryptionKeyModel>) assignmentDecryptionKeys, redownloadedAssignmentFileMetadatas));
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.IsPinCodeDisabled = false));
      }
      catch (DownloadFailedException ex)
      {
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
        {
          this.UpdateLanguage(true);
          this.WorkspaceSubmitPinCodeErrorText = this._languageService.GetString("WorkspacePinCodeCannotRedownloadErrorText");
          this.IsPinCodeDisabled = false;
          this.IsForceContinueVisible = true;
        }));
      }
      catch (Exception ex)
      {
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
        {
          this.WorkspaceSubmitPinCodeErrorText = this._languageService.GetString("WorkspacePinCodeGeneralErrorText");
          this.IsPinCodeDisabled = false;
        }));
      }
    }

    private List<AssignmentDecryptionKeyModel> GetAssignmentDecryptionKeys()
    {
      try
      {
        return this._assignmentFileService.GetDecryptionKeys(this.PinCode).ToList<AssignmentDecryptionKeyModel>();
      }
      catch (InvalidPinCodeException ex)
      {
        this._messenger.Send<OnAllowClosingPinCodePopup>(new OnAllowClosingPinCodePopup(false));
        this._lockUiOnInvalidPinCodeTimerService.Start();
      }
      catch (Exception ex)
      {
        DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
        {
          this.WorkspaceSubmitPinCodeErrorText = this._languageService.GetString("WorkspacePinCodeGeneralErrorText");
          this.IsPinCodeDisabled = false;
        }));
      }
      return (List<AssignmentDecryptionKeyModel>) null;
    }

    public string WorkspaceSubmitPinCodeErrorText
    {
      get
      {
        return this._workspaceSubmitPinCodeErrorText;
      }
      set
      {
        this._workspaceSubmitPinCodeErrorText = value;
        this.OnPropertyChanged(nameof (WorkspaceSubmitPinCodeErrorText));
        this.OnPropertyChanged("PinCode");
      }
    }

    public ICommand UnlockAssignmentFilesCommand { get; }

    public string WorkspaceUnlockAssignmentFilesInformationText
    {
      get
      {
        return this._workspaceUnlockAssignmentFilesInformationText;
      }
      set
      {
        this._workspaceUnlockAssignmentFilesInformationText = value;
        this.OnPropertyChanged(nameof (WorkspaceUnlockAssignmentFilesInformationText));
      }
    }

    public string WorkspaceUnlockAssignmentFilesButtonText
    {
      get
      {
        return this._workspaceUnlockAssignmentFilesButtonText;
      }
      set
      {
        this._workspaceUnlockAssignmentFilesButtonText = value;
        this.OnPropertyChanged(nameof (WorkspaceUnlockAssignmentFilesButtonText));
      }
    }

    public string PinCode
    {
      get
      {
        return this._pinCode;
      }
      set
      {
        this._pinCode = value;
        this.OnPropertyChanged(nameof (PinCode));
        this.ShowPinCodeTextboxError = true;
        this.WorkspaceSubmitPinCodeErrorText = (string) null;
      }
    }

    private bool ShowPinCodeTextboxError
    {
      get
      {
        return this._showPinCodeTextboxError;
      }
      set
      {
        this._showPinCodeTextboxError = value;
        this.OnPropertyChanged(nameof (ShowPinCodeTextboxError));
        this.OnPropertyChanged("PinCode");
      }
    }

    public string this[string columnName]
    {
      get
      {
        if (!(columnName == "PinCode"))
          return string.Empty;
        if (!string.IsNullOrEmpty(this.WorkspaceSubmitPinCodeErrorText))
          return this._workspaceSubmitPinCodeErrorText;
        if (!this.ShowPinCodeTextboxError)
          return string.Empty;
        ValidatorResult validatorResult = this._pinCodeValidator.Validate(this.PinCode);
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

    public ICommand ForceContinueCommand { get; }

    public bool IsForceContinueVisible
    {
      get
      {
        return this._isForceContinueVisible;
      }
      set
      {
        this._isForceContinueVisible = value;
        this.OnPropertyChanged(nameof (IsForceContinueVisible));
      }
    }

    public string WorkspaceForceContinueButtonText
    {
      get
      {
        return this._languageService.GetString(nameof (WorkspaceForceContinueButtonText));
      }
    }

    public void HandleKeyDown(Key key)
    {
      if (key != Key.Escape || this.IsPinCodeDisabled)
        return;
      this._messenger.Send<OnPinCodeLoginCancelled>(new OnPinCodeLoginCancelled());
    }
  }
}
