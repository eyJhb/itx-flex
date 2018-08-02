// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.HandInFileSelectorViewModel
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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Itx.Flex.Client.ViewModel
{
  public class HandInFileSelectorViewModel : BaseViewModel, IHandInFileSelectorViewModel
  {
    private IList<string> _allowedFileExtensions = (IList<string>) new List<string>();
    private ObservableCollection<HandInFileViewModel> _selectedAttachments = new ObservableCollection<HandInFileViewModel>();
    private readonly ILanguageService _languageService;
    private readonly ISelectFilesService _selectFilesService;
    private readonly IMessenger _messenger;
    private readonly IHandInFileMetadataStorageService _handInFileMetadataStorageService;
    private readonly IPathService _pathService;
    private readonly IFileService _fileService;
    private readonly ILoggerService _loggerService;
    private HandInFileViewModel _selectedMainDocument;

    public HandInFileSelectorViewModel(ILanguageService languageService, ISelectFilesService selectFilesService, IMessenger messenger, IHandInFileMetadataStorageService handInFileMetadataStorageService, IPathService pathService, IFileService fileService, IConfigurationService configurationService, ILoggerService loggerService)
    {
      this._languageService = languageService;
      this._selectFilesService = selectFilesService;
      this._messenger = messenger;
      this._handInFileMetadataStorageService = handInFileMetadataStorageService;
      this._pathService = pathService;
      this._fileService = fileService;
      this._loggerService = loggerService;
      this.ChooseMainDocumentCommand = (ICommand) new RelayCommand((Action<object>) (c => this.ChooseMainDocumentClick()), (Predicate<object>) null);
      this.AddAttachmentCommand = (ICommand) new RelayCommand((Action<object>) (c => this.AddAttachmentClick()), (Predicate<object>) null);
      this.RemoveMainDocumentCommand = (ICommand) new RelayCommand((Action<object>) (c => this.RemoveMainDocumentClick()), (Predicate<object>) null);
      this.RemoveAttachmentCommand = (ICommand) new RelayCommand(new Action<object>(this.RemoveAttachmentClick), (Predicate<object>) null);
      this.MaxHandInFileSizeInBytes = (long) configurationService.MaxHandInFileSizeInBytes;
      messenger.Register<OnWorkspacePathsSet>((object) this, new Action<OnWorkspacePathsSet>(this.OnWorkspacePathsSet));
      messenger.Register<OnAllowedFileExtensionsLoaded>((object) this, new Action<OnAllowedFileExtensionsLoaded>(this.OnAllowedFileExtensionsSet));
      messenger.Register<OnHandInFilesizeLimitLoaded>((object) this, new Action<OnHandInFilesizeLimitLoaded>(this.OnHandInFilesizeLimitLoaded));
    }

    private long MaxHandInFileSizeInBytes { get; set; }

    private void OnHandInFilesizeLimitLoaded(OnHandInFilesizeLimitLoaded onHandInFilesizeLimitLoaded)
    {
      this.MaxHandInFileSizeInBytes = (long) onHandInFilesizeLimitLoaded.MaxHandInFileSizeInBytes;
    }

    private void OnAllowedFileExtensionsSet(OnAllowedFileExtensionsLoaded onAllowedFileExtensionsLoaded)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.AllowedFileExtensions = (IList<string>) onAllowedFileExtensionsLoaded.FileExtensions.ToList<string>()));
    }

    public ICommand RemoveAttachmentCommand { get; }

    private void RemoveAttachmentClick(object name)
    {
      IEnumerable<HandInFileViewModel> newAttachments = this.SelectedAttachments.Where<HandInFileViewModel>((Func<HandInFileViewModel, bool>) (a => a.HandInFileModel.Name != name.ToString()));
      Task.Factory.StartNew((Action) (() => this._handInFileMetadataStorageService.StoreAttachmentFilePaths(newAttachments.Select<HandInFileViewModel, HandInFileModel>((Func<HandInFileViewModel, HandInFileModel>) (e => e.HandInFileModel)))));
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.SelectedAttachments = new ObservableCollection<HandInFileViewModel>(newAttachments)));
    }

    public ICommand RemoveMainDocumentCommand { get; }

    private void RemoveMainDocumentClick()
    {
      Task.Factory.StartNew((Action) (() => this._handInFileMetadataStorageService.ClearMainDocument()));
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.SelectedMainDocument = (HandInFileViewModel) null));
    }

    private void OnWorkspacePathsSet(OnWorkspacePathsSet onWorkspacePathsSet)
    {
      this.HandInDirectory = onWorkspacePathsSet.HandInPath;
      List<SubmitHandInFileModel> handInFiles = this.GetHandInFiles().ToList<SubmitHandInFileModel>();
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        List<SubmitHandInFileModel> list = handInFiles.Where<SubmitHandInFileModel>((Func<SubmitHandInFileModel, bool>) (f => f.SubmitHandInFileType == SubmitHandInFileType.Attachment)).ToList<SubmitHandInFileModel>();
        List<SubmitHandInFileModel> source = list;
        Func<SubmitHandInFileModel, bool> func = (Func<SubmitHandInFileModel, bool>) (f => f.SubmitHandInFileType == SubmitHandInFileType.Attachment);
        Func<SubmitHandInFileModel, bool> predicate;
        if (source.Any<SubmitHandInFileModel>(predicate))
          this.SelectedAttachments = new ObservableCollection<HandInFileViewModel>(list.Select<SubmitHandInFileModel, HandInFileViewModel>((Func<SubmitHandInFileModel, HandInFileViewModel>) (a => new HandInFileViewModel(new HandInFileModel(a.Name, a.Path)))));
        SubmitHandInFileModel submitHandInFileModel = handInFiles.FirstOrDefault<SubmitHandInFileModel>((Func<SubmitHandInFileModel, bool>) (f => f.SubmitHandInFileType == SubmitHandInFileType.MainDocument));
        if (submitHandInFileModel == null)
          return;
        this.SelectedMainDocument = new HandInFileViewModel(new HandInFileModel(submitHandInFileModel.Name, submitHandInFileModel.Path));
      }));
    }

    public IEnumerable<SubmitHandInFileModel> GetHandInFiles()
    {
      List<HandInFileModel> list = this._handInFileMetadataStorageService.GetAttachmentFilePaths().Where<HandInFileModel>((Func<HandInFileModel, bool>) (a => this._fileService.Exists(a.Path))).ToList<HandInFileModel>();
      HandInFileModel documentFilePath = this._handInFileMetadataStorageService.GetMainDocumentFilePath();
      int num = string.IsNullOrEmpty(documentFilePath?.Path) ? 0 : (this._fileService.Exists(documentFilePath.Path) ? 1 : 0);
      List<SubmitHandInFileModel> submitHandInFileModelList = new List<SubmitHandInFileModel>(list.Select<HandInFileModel, SubmitHandInFileModel>((Func<HandInFileModel, SubmitHandInFileModel>) (a => new SubmitHandInFileModel(a.Name, a.Path, SubmitHandInFileType.Attachment))));
      if (num != 0)
        submitHandInFileModelList.Add(new SubmitHandInFileModel(documentFilePath.Name, documentFilePath.Path, SubmitHandInFileType.MainDocument));
      return (IEnumerable<SubmitHandInFileModel>) submitHandInFileModelList;
    }

    private string HandInDirectory { get; set; }

    public IList<string> AllowedFileExtensions
    {
      get
      {
        return this._allowedFileExtensions;
      }
      set
      {
        this._allowedFileExtensions = value;
        this.OnPropertyChanged(nameof (AllowedFileExtensions));
      }
    }

    private void AddAttachmentClick()
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        FilesResult selectedFilesNoFilter = this._selectFilesService.GetSelectedFilesNoFilter(this.HandInDirectory);
        if (!selectedFilesNoFilter.HasSelected)
          return;
        List<HandInFileModel> selectedAttachments = selectedFilesNoFilter.HandInFileModels.ToList<HandInFileModel>();
        try
        {
          List<HandInFileModel> list = selectedAttachments.Where<HandInFileModel>((Func<HandInFileModel, bool>) (e => this._fileService.GetSizeInBytes(e.Path) > this.MaxHandInFileSizeInBytes)).ToList<HandInFileModel>();
          if (list.Any<HandInFileModel>())
          {
            this._messenger.Send<OnOkPopupOpened>(new OnOkPopupOpened(new OkPopupViewModel(string.Format(this._languageService.GetString("HandInFileSelectorAttachmentsExceedsFileLimitMessageText"), (object) (this.MaxHandInFileSizeInBytes / 1048576L), (object) string.Join("\n", list.Select<HandInFileModel, string>((Func<HandInFileModel, string>) (e => e.Name)).ToArray<string>())), this._languageService.GetString("HandInFileSelectorFilesExceedsFileLimitOkButtonText"), this._messenger)));
            if (list.Count == selectedAttachments.Count)
              return;
            selectedAttachments = selectedAttachments.Except<HandInFileModel>((IEnumerable<HandInFileModel>) list).ToList<HandInFileModel>();
          }
        }
        catch (Exception ex)
        {
          this._loggerService.Log("Could not check filesize of selected files: " + string.Join(", ", selectedAttachments.Select<HandInFileModel, string>((Func<HandInFileModel, string>) (s => s.Path)).ToArray<string>()), ex);
          return;
        }
        List<HandInFileModel> duplicateHandInFiles = selectedAttachments.Where<HandInFileModel>((Func<HandInFileModel, bool>) (e => this.AnyExistingHandInFilesForName(e, (IEnumerable<HandInFileModel>) selectedAttachments))).ToList<HandInFileModel>();
        if (duplicateHandInFiles.Any<HandInFileModel>())
          this._messenger.Send<OnOkCancelPopupOpened>(new OnOkCancelPopupOpened(new OkCancelPopupViewModel(this._languageService.GetString("HandInFileSelectorAddDuplicatesMessageText"), this._languageService.GetString("HandInFileSelectorDuplicatesAddButtonText"), this._languageService.GetString("HandInFileSelectorDuplicatesCancelButtonText"), this._messenger), (Action<bool>) (isOkSelected =>
          {
            if (!isOkSelected)
              return;
            List<HandInFileModel> list = selectedAttachments.Where<HandInFileModel>((Func<HandInFileModel, bool>) (sf => duplicateHandInFiles.Select<HandInFileModel, string>((Func<HandInFileModel, string>) (a => a.Path)).All<string>((Func<string, bool>) (a => a != sf.Path)))).ToList<HandInFileModel>();
            List<HandInFileModel> existingHandInFiles = this.SelectedAttachments.Select<HandInFileViewModel, HandInFileModel>((Func<HandInFileViewModel, HandInFileModel>) (e => e.HandInFileModel)).ToList<HandInFileModel>();
            if (this.SelectedMainDocument?.HandInFileModel != null)
              existingHandInFiles.Add(this.SelectedMainDocument.HandInFileModel);
            existingHandInFiles.AddRange((IEnumerable<HandInFileModel>) selectedAttachments);
            IEnumerable<HandInFileModel> collection = duplicateHandInFiles.Select<HandInFileModel, HandInFileModel>((Func<HandInFileModel, HandInFileModel>) (a => this.GetNewHandInFileModel(a, existingHandInFiles)));
            list.AddRange(collection);
            this.AddAttachments((IEnumerable<HandInFileModel>) list);
          })));
        else
          this.AddAttachments((IEnumerable<HandInFileModel>) selectedAttachments);
      }));
    }

    private HandInFileModel GetNewHandInFileModel(HandInFileModel newHandInFileModel, List<HandInFileModel> alreadyAddedHandInFileModels)
    {
      int num = 1;
      string newName;
      while (true)
      {
        newName = this._pathService.GetFileNameWithoutExtension(newHandInFileModel.Name) + string.Format(" ({0})", (object) num);
        if (!alreadyAddedHandInFileModels.All<HandInFileModel>((Func<HandInFileModel, bool>) (a => this._pathService.GetFileNameWithoutExtension(a.Name) != newName)))
          ++num;
        else
          break;
      }
      return new HandInFileModel(newName + "." + this._pathService.GetExtension(newHandInFileModel.Name), newHandInFileModel.Path);
    }

    private void AddAttachments(IEnumerable<HandInFileModel> newAttachments)
    {
      List<HandInFileViewModel> existingAttachments = this.SelectedAttachments.ToList<HandInFileViewModel>();
      existingAttachments.AddRange(newAttachments.Select<HandInFileModel, HandInFileViewModel>((Func<HandInFileModel, HandInFileViewModel>) (e => new HandInFileViewModel(e))));
      Task.Factory.StartNew((Action) (() => this._handInFileMetadataStorageService.StoreAttachmentFilePaths(existingAttachments.Select<HandInFileViewModel, HandInFileModel>((Func<HandInFileViewModel, HandInFileModel>) (e => e.HandInFileModel)))));
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.SelectedAttachments = new ObservableCollection<HandInFileViewModel>(existingAttachments)));
    }

    private bool AnyExistingHandInFilesForName(HandInFileModel handInFileModel, IEnumerable<HandInFileModel> newAttachments)
    {
      if ((string.IsNullOrEmpty(this.SelectedMainDocument?.HandInFileModel?.Name) || !(this.SelectedMainDocument.HandInFileModel.Name == handInFileModel.Name)) && !this.SelectedAttachments.Any<HandInFileViewModel>((Func<HandInFileViewModel, bool>) (e => e.HandInFileModel?.Name == handInFileModel.Name)))
        return newAttachments.Any<HandInFileModel>((Func<HandInFileModel, bool>) (a =>
        {
          if (a.Path != handInFileModel.Path)
            return a.Name == handInFileModel.Name;
          return false;
        }));
      return true;
    }

    private void ChooseMainDocumentClick()
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        FilesResult selectedFile = this._selectFilesService.GetSelectedFile(this.HandInDirectory, this._languageService.GetString("HandInFileSelectorValidExtensionsText"), (IEnumerable<string>) this.AllowedFileExtensions);
        if (!selectedFile.HasSelected || !selectedFile.HandInFileModels.Any<HandInFileModel>())
          return;
        HandInFileModel selectedMainDocument = selectedFile.HandInFileModels.First<HandInFileModel>();
        try
        {
          if (this._fileService.GetSizeInBytes(selectedMainDocument.Path) > this.MaxHandInFileSizeInBytes)
          {
            this._messenger.Send<OnOkPopupOpened>(new OnOkPopupOpened(new OkPopupViewModel(string.Format(this._languageService.GetString("HandInFileSelectorMainDocumentExceedsFileLimitMessageText"), (object) (this.MaxHandInFileSizeInBytes / 1048576L)), this._languageService.GetString("HandInFileSelectorFilesExceedsFileLimitOkButtonText"), this._messenger)));
            return;
          }
        }
        catch (Exception ex)
        {
          return;
        }
        if (this.SelectedAttachments.Any<HandInFileViewModel>((Func<HandInFileViewModel, bool>) (e => e.HandInFileModel?.Name == selectedMainDocument.Name)))
          this._messenger.Send<OnOkCancelPopupOpened>(new OnOkCancelPopupOpened(new OkCancelPopupViewModel(this._languageService.GetString("HandInFileSelectorAddDuplicatesMessageText"), this._languageService.GetString("HandInFileSelectorDuplicatesAddButtonText"), this._languageService.GetString("HandInFileSelectorDuplicatesCancelButtonText"), this._messenger), (Action<bool>) (isOkSelected =>
          {
            if (!isOkSelected)
              return;
            List<HandInFileViewModel> list = this.SelectedAttachments.ToList<HandInFileViewModel>();
            HandInFileModel newMainDocument = this.GetNewHandInFileModel(selectedMainDocument, list.Select<HandInFileViewModel, HandInFileModel>((Func<HandInFileViewModel, HandInFileModel>) (e => e.HandInFileModel)).ToList<HandInFileModel>());
            DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.SetMainDocument(newMainDocument)));
          })));
        else
          this.SetMainDocument(selectedMainDocument);
      }));
    }

    private void SetMainDocument(HandInFileModel selectedMainDocument)
    {
      Task.Factory.StartNew((Action) (() => this._handInFileMetadataStorageService.StoreMainDocumentFilePath(selectedMainDocument)));
      this.SelectedMainDocument = new HandInFileViewModel(selectedMainDocument);
    }

    public string HandInFileSelectorMainDocumentText
    {
      get
      {
        return this._languageService.GetString(nameof (HandInFileSelectorMainDocumentText));
      }
    }

    public string HandInFileSelectorChooseMainDocumentButtonText
    {
      get
      {
        return this._languageService.GetString(nameof (HandInFileSelectorChooseMainDocumentButtonText));
      }
    }

    public string HandInFileSelectorAttachmentsText
    {
      get
      {
        return this._languageService.GetString(nameof (HandInFileSelectorAttachmentsText));
      }
    }

    public string HandInFileSelectorChooseAttachmentButtonText
    {
      get
      {
        return this._languageService.GetString(nameof (HandInFileSelectorChooseAttachmentButtonText));
      }
    }

    public string HandInFileSelectorAllowedFormatsText
    {
      get
      {
        return this._languageService.GetString(nameof (HandInFileSelectorAllowedFormatsText));
      }
    }

    public ObservableCollection<HandInFileViewModel> SelectedAttachments
    {
      get
      {
        return this._selectedAttachments;
      }
      set
      {
        this._selectedAttachments = value;
        this.OnPropertyChanged(nameof (SelectedAttachments));
      }
    }

    public IEnumerable<HandInFileModel> Attachments
    {
      get
      {
        return this.SelectedAttachments.Select<HandInFileViewModel, HandInFileModel>((Func<HandInFileViewModel, HandInFileModel>) (e => e.HandInFileModel));
      }
    }

    public HandInFileViewModel SelectedMainDocument
    {
      get
      {
        return this._selectedMainDocument;
      }
      set
      {
        this._selectedMainDocument = value;
        this.OnPropertyChanged(nameof (SelectedMainDocument));
      }
    }

    public HandInFileModel MainDocument
    {
      get
      {
        return this.SelectedMainDocument.HandInFileModel;
      }
    }

    public ICommand ChooseMainDocumentCommand { get; }

    public ICommand AddAttachmentCommand { get; }

    public bool CanContinue()
    {
      return this.SelectedMainDocument?.HandInFileModel != null;
    }

    public string HandInFileSelectorRemoveMainDocumentButtonText
    {
      get
      {
        return this._languageService.GetString(nameof (HandInFileSelectorRemoveMainDocumentButtonText));
      }
    }

    public string HandInFileSelectorRemoveAttachmentButtonText
    {
      get
      {
        return this._languageService.GetString(nameof (HandInFileSelectorRemoveAttachmentButtonText));
      }
    }

    public string HandInFileSelectorMainDocumentRequiredText
    {
      get
      {
        return this._languageService.GetString(nameof (HandInFileSelectorMainDocumentRequiredText));
      }
    }
  }
}
