// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.SubmitHandInViewModel
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
  public class SubmitHandInViewModel : BaseViewModel, ISubmitHandInViewModel, IBaseViewModel
  {
    private readonly ILanguageService _languageService;
    private readonly IHandInFieldValueStorageService _handInFieldValueStorageService;
    private readonly IMessenger _messenger;
    private readonly IHandInFileService _handInFileService;
    private readonly IBoardingPassStorageService _boardingPassStorageService;
    private readonly IHandInFieldIdStorageService _handInFieldIdStorageService;
    private readonly IHeartbeatService _heartbeatService;
    private ObservableCollection<IHandInFieldViewModel> _handInFieldViewModels;

    public SubmitHandInViewModel(ILastSaveViewModel lastSaveViewModel, IExamTimeLeftViewModel examTimeLeftViewModel, ILanguageService languageService, IHandInFieldValueStorageService handInFieldValueStorageService, IHandInFileSelectorViewModel handInFileSelectorViewModel, IMessenger messenger, IHandInFileService handInFileService, IBoardingPassStorageService boardingPassStorageService, IHandInFieldIdStorageService handInFieldIdStorageService, IHeartbeatService heartbeatService)
    {
      this._languageService = languageService;
      this._handInFieldValueStorageService = handInFieldValueStorageService;
      this._messenger = messenger;
      this._handInFileService = handInFileService;
      this._boardingPassStorageService = boardingPassStorageService;
      this._handInFieldIdStorageService = handInFieldIdStorageService;
      this._heartbeatService = heartbeatService;
      this.LastSaveViewModel = lastSaveViewModel;
      this.ExamTimeLeftViewModel = examTimeLeftViewModel;
      this.HandInFileSelectorViewModel = handInFileSelectorViewModel;
      this._messenger.Register<OnInitiateSubmitHandIn>((object) this, new Action<OnInitiateSubmitHandIn>(this.OnInitiateSubmitHandIn));
      this.PreviousViewCommand = (ICommand) new RelayCommand((Action<object>) (c => this.PreviousViewClick()), (Predicate<object>) null);
      this.NextViewCommand = (ICommand) new RelayCommand((Action<object>) (c => this.NextViewClick()), (Predicate<object>) null);
      this._messenger.Register<OnHandInFieldsLoaded>((object) this, new Action<OnHandInFieldsLoaded>(this.OnHandInFieldsLoaded));
      this._messenger.Register<OnHandInFieldInputLostFocus>((object) this, new Action<OnHandInFieldInputLostFocus>(this.OnHandInFieldInputLostFocus));
    }

    private void OnHandInFieldInputLostFocus(OnHandInFieldInputLostFocus onHandInFieldInputLostFocus)
    {
      Task.Factory.StartNew((Action) (() => this._handInFieldValueStorageService.Store(this._boardingPassStorageService.GetExisting(), this.CreateHandInFieldId(), this.HandInFieldViewModels.Select<IHandInFieldViewModel, HandInFieldValueModel>((Func<IHandInFieldViewModel, HandInFieldValueModel>) (h => new HandInFieldValueModel(h.Id, h.Value))))));
    }

    private void OnHandInFieldsLoaded(OnHandInFieldsLoaded onHandInFieldsLoaded)
    {
      IEnumerable<HandInFieldValueModel> existingHandInFieldValues = this._handInFieldValueStorageService.Get(this._boardingPassStorageService.GetExisting(), this.CreateHandInFieldId());
      this.SetupHandInFields(onHandInFieldsLoaded.HandinFields, existingHandInFieldValues);
    }

    private Guid CreateHandInFieldId()
    {
      Guid? nullable = this._handInFieldIdStorageService.Get();
      if (!nullable.HasValue)
      {
        nullable = new Guid?(Guid.NewGuid());
        this._handInFieldIdStorageService.Store(nullable.Value);
      }
      return nullable.Value;
    }

    private void OnInitiateSubmitHandIn(OnInitiateSubmitHandIn onInitiateSubmitHandIn)
    {
      foreach (IHandInFieldViewModel inFieldViewModel in (Collection<IHandInFieldViewModel>) this.HandInFieldViewModels)
        inFieldViewModel.ResetValidation();
    }

    private void NextViewClick()
    {
      Task.Factory.StartNew((Action) (() =>
      {
        bool flag = false;
        foreach (IHandInFieldViewModel inFieldViewModel in (Collection<IHandInFieldViewModel>) this.HandInFieldViewModels)
          flag = !inFieldViewModel.IsValid() | flag;
        if (flag || !this.HandInFileSelectorViewModel.CanContinue())
          return;
        string boardingPass = this._boardingPassStorageService.GetExisting();
        Guid handInIdField = this.CreateHandInFieldId();
        this._handInFieldValueStorageService.Store(boardingPass, handInIdField, this.HandInFieldViewModels.Select<IHandInFieldViewModel, HandInFieldValueModel>((Func<IHandInFieldViewModel, HandInFieldValueModel>) (h => new HandInFieldValueModel(h.Id, h.Value))));
        this._messenger.Send<OnOkCancelPopupOpened>(new OnOkCancelPopupOpened(new OkCancelPopupViewModel(this._languageService.GetString("SubmitHandInConfirmSubmissionMessageText"), this._languageService.GetString("SubmitHandInConfirmSubmissionOkButtonText"), this._languageService.GetString("SubmitHandInConfirmSubmissionCancelButtonText"), this._messenger), (Action<bool>) (hasSelectedOk =>
        {
          if (!hasSelectedOk)
            return;
          HandInResult handInResult = this._handInFileService.TagHandIn(boardingPass, this.HandInFileSelectorViewModel.MainDocument, (IEnumerable<HandInFileModel>) this.HandInFileSelectorViewModel.Attachments.ToList<HandInFileModel>(), this._handInFieldValueStorageService.GetFileMetadata(boardingPass, handInIdField));
          if (handInResult.HandInStatus.HasTagged())
          {
            this._messenger.Send<OnSubmitHandIn>(new OnSubmitHandIn(handInResult.HandInStatus, handInResult.HandInFiles));
          }
          else
          {
            if (handInResult.HandInStatus != HandInStatus.TaggedExternally)
              return;
            this._heartbeatService.StartHeartbeats();
          }
        })));
      }));
    }

    private void PreviousViewClick()
    {
      foreach (IHandInFieldViewModel inFieldViewModel in (Collection<IHandInFieldViewModel>) this.HandInFieldViewModels)
        inFieldViewModel.ResetErrorDisplay();
      this._messenger.Send<OnExamStarted>(new OnExamStarted());
    }

    private void SetupHandInFields(IEnumerable<HandInFieldDescriptionModel> handInFieldDescriptions, IEnumerable<HandInFieldValueModel> existingHandInFieldValues)
    {
      List<HandInFieldViewModel> handInFieldViewModels = handInFieldDescriptions.Select<HandInFieldDescriptionModel, HandInFieldViewModel>((Func<HandInFieldDescriptionModel, HandInFieldViewModel>) (h => new HandInFieldViewModel((IHandInFieldTypeValidator) new HandInFieldTypeValidator(h.RegexValidation, h.ValueType), this._languageService, h.Required, h.TextDaDk, h.TextEnGb, h.Id, this._messenger, h.DescriptionDaDk, h.DescriptionEnGb))).ToList<HandInFieldViewModel>();
      IEnumerable<HandInFieldValueModel> source = existingHandInFieldValues;
      Func<HandInFieldValueModel, Guid> func = (Func<HandInFieldValueModel, Guid>) (k => k.Id);
      Func<HandInFieldValueModel, Guid> keySelector;
      Dictionary<Guid, string> dictionary = source.ToDictionary<HandInFieldValueModel, Guid, string>(keySelector, (Func<HandInFieldValueModel, string>) (v => v.Value));
      foreach (HandInFieldViewModel inFieldViewModel in handInFieldViewModels)
      {
        string str;
        if (dictionary.TryGetValue(inFieldViewModel.Id, out str))
          inFieldViewModel.HandInFieldValue = str;
      }
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.HandInFieldViewModels = new ObservableCollection<IHandInFieldViewModel>(handInFieldViewModels.Cast<IHandInFieldViewModel>())));
    }

    public ILastSaveViewModel LastSaveViewModel { get; }

    public IExamTimeLeftViewModel ExamTimeLeftViewModel { get; }

    public ObservableCollection<IHandInFieldViewModel> HandInFieldViewModels
    {
      get
      {
        return this._handInFieldViewModels;
      }
      set
      {
        this._handInFieldViewModels = value;
        this.OnPropertyChanged(nameof (HandInFieldViewModels));
        this.OnPropertyChanged("AnyRequiredHandInFields");
      }
    }

    public bool AnyRequiredHandInFields
    {
      get
      {
        return this.HandInFieldViewModels.Any<IHandInFieldViewModel>((Func<IHandInFieldViewModel, bool>) (h => h.IsRequired));
      }
    }

    public IHandInFileSelectorViewModel HandInFileSelectorViewModel { get; }

    public string SubmitHandInPreviousButtonText
    {
      get
      {
        return this._languageService.GetString("SubmitHandInGoBackToOngoingExamButtonText");
      }
    }

    public string SubmitHandInNextButtonText
    {
      get
      {
        return this._languageService.GetString("SubmitHandInGoToSubmitHandInButtonText");
      }
    }

    public ICommand PreviousViewCommand { get; }

    public ICommand NextViewCommand { get; }

    public string SubmitHandInHeaderText
    {
      get
      {
        return this._languageService.GetString(nameof (SubmitHandInHeaderText));
      }
    }

    public string SubmitHandInIsRequiredText
    {
      get
      {
        return this._languageService.GetString(nameof (SubmitHandInIsRequiredText));
      }
    }

    public IEnumerable<SubmitHandInFileModel> GetHandInFiles()
    {
      List<SubmitHandInFileModel> list = this.HandInFileSelectorViewModel.GetHandInFiles().ToList<SubmitHandInFileModel>();
      HandInFileModel fileMetadata = this._handInFieldValueStorageService.GetFileMetadata(this._boardingPassStorageService.GetExisting(), this.CreateHandInFieldId());
      list.Add(new SubmitHandInFileModel(fileMetadata.Name, fileMetadata.Path, SubmitHandInFileType.HandInFields));
      return (IEnumerable<SubmitHandInFileModel>) list;
    }
  }
}
