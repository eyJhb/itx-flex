// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.UpdateProgramWindowViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using Arcanic.ITX.Contracts.Flex;
using Arcanic.ITX.Flex.WebserviceClient;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.AutoUpdate;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.Service;
using System;
using System.Threading.Tasks;

namespace Itx.Flex.Client.ViewModel
{
  public class UpdateProgramWindowViewModel : BaseViewModel, IUpdateProgramViewModel, IBaseViewModel
  {
    private readonly IUpdater _updater;
    private readonly IMessenger _messenger;
    private readonly IConfigurationService _configurationService;
    private readonly ILanguageService _languageService;
    private readonly IFlexClient _flexClient;
    private readonly ILoggerService _loggerService;
    private string _updateProgramUpdatingText;

    public UpdateProgramWindowViewModel(IUpdater updater, IMessenger messenger, IConfigurationService configurationService, ILanguageService languageService, IFlexClient flexClient, ILoggerService loggerService)
    {
      this._updater = updater;
      this._messenger = messenger;
      this._configurationService = configurationService;
      this._languageService = languageService;
      this._flexClient = flexClient;
      this._loggerService = loggerService;
      this.UpdateLanguage((OnLanguageChanged) null);
      messenger.Register<OnLanguageChanged>((object) this, new Action<OnLanguageChanged>(this.UpdateLanguage));
      this.CheckForUpdates((OnCheckForUpdates) null);
      messenger.Register<OnCheckForUpdates>((object) this, new Action<OnCheckForUpdates>(this.CheckForUpdates));
    }

    private void CheckForUpdates(OnCheckForUpdates obj = null)
    {
      Task.Factory.StartNew((Action) (() =>
      {
        try
        {
          GlobalResponse globalSettings = this._flexClient.GetGlobalSettings();
          this._configurationService.GlobalResponse = globalSettings;
          if (globalSettings != null)
            this._updater.Update(globalSettings.VersionInfo.WindowsClientUrl);
        }
        catch (Exception ex)
        {
          this._loggerService.Log(LogType.Error, "Update failed: " + ex.Message, ex.StackTrace);
        }
        this._messenger.Send<OnUpdateFinished>(new OnUpdateFinished());
      }));
    }

    private void UpdateLanguage(OnLanguageChanged onLanguageChanged = null)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() => this.UpdateProgramUpdatingText = this._languageService.GetString("UpdateProgramUpdatingText")));
    }

    public string UpdateProgramUpdatingText
    {
      get
      {
        return this._updateProgramUpdatingText;
      }
      set
      {
        this._updateProgramUpdatingText = value;
        this.OnPropertyChanged(nameof (UpdateProgramUpdatingText));
      }
    }
  }
}
