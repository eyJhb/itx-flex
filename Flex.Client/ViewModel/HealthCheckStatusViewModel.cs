// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.ViewModel.HealthCheckStatusViewModel
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Itx.Flex.Client.Message;
using Itx.Flex.Client.Service;
using System;
using System.Windows.Input;

namespace Itx.Flex.Client.ViewModel
{
  public class HealthCheckStatusViewModel : BaseViewModel
  {
    private readonly ILanguageService _languageService;
    private readonly IMessenger _messenger;
    private bool _showReadMore;
    private string _healthCheckOnErrorReadMoreText;
    private string _healthCheckOnErrorCloseReadMoreText;
    private string _healthCheckReadMoreText;
    private string _healthCheckText;
    private string _imageSource;

    public HealthCheckStatusViewModel(ILanguageService languageService, IMessenger messenger, string healthCheckTextKey, string imageSource, string healthCheckReadMoreTextKey)
    {
      this._languageService = languageService;
      this._messenger = messenger;
      this.HealthCheckTextKey = healthCheckTextKey;
      this.ImageSource = imageSource;
      if (!string.IsNullOrEmpty(healthCheckReadMoreTextKey))
      {
        this.HealthCheckReadMoreTextKey = healthCheckReadMoreTextKey;
        this.ShowReadMore = true;
      }
      this.UpdateLanguage((OnLanguageChanged) null);
      messenger.Register<OnLanguageChanged>((object) this, new Action<OnLanguageChanged>(this.UpdateLanguage));
      this.ReadMoreCommand = (ICommand) new RelayCommand((Action<object>) (c => this.ReadMoreClick()), (Predicate<object>) null);
    }

    private void ReadMoreClick()
    {
      this._messenger.Send<OnHealthCheckReadMorePopupOpened>(new OnHealthCheckReadMorePopupOpened(new OkPopupViewModel(this.HealthCheckReadMoreText, this.HealthCheckOnErrorCloseReadMoreText, this._messenger)));
    }

    public bool ShowReadMore
    {
      get
      {
        return this._showReadMore;
      }
      set
      {
        this._showReadMore = value;
        this.OnPropertyChanged(nameof (ShowReadMore));
      }
    }

    private void UpdateLanguage(OnLanguageChanged onLanguageChanged = null)
    {
      DispatcherHelper.CheckBeginInvokeOnUI((Action) (() =>
      {
        this.HealthCheckText = this._languageService.GetString(this.HealthCheckTextKey);
        if (!this.ShowReadMore)
          return;
        this.HealthCheckOnErrorReadMoreText = this._languageService.GetString("HealthCheckOnErrorReadMoreText");
        this.HealthCheckOnErrorCloseReadMoreText = this._languageService.GetString("HealthCheckOnErrorCloseReadMoreText");
        this.HealthCheckReadMoreText = this._languageService.GetString(this.HealthCheckReadMoreTextKey);
      }));
    }

    public string HealthCheckOnErrorReadMoreText
    {
      get
      {
        return this._healthCheckOnErrorReadMoreText;
      }
      set
      {
        this._healthCheckOnErrorReadMoreText = value;
        this.OnPropertyChanged(nameof (HealthCheckOnErrorReadMoreText));
      }
    }

    public string HealthCheckOnErrorCloseReadMoreText
    {
      get
      {
        return this._healthCheckOnErrorCloseReadMoreText;
      }
      set
      {
        this._healthCheckOnErrorCloseReadMoreText = value;
        this.OnPropertyChanged(nameof (HealthCheckOnErrorCloseReadMoreText));
      }
    }

    private string HealthCheckReadMoreTextKey { get; }

    public string HealthCheckReadMoreText
    {
      get
      {
        return this._healthCheckReadMoreText;
      }
      set
      {
        this._healthCheckReadMoreText = value;
        this.OnPropertyChanged(nameof (HealthCheckReadMoreText));
      }
    }

    private string HealthCheckTextKey { get; }

    public string HealthCheckText
    {
      get
      {
        return this._healthCheckText;
      }
      set
      {
        this._healthCheckText = value;
        this.OnPropertyChanged(nameof (HealthCheckText));
      }
    }

    public string ImageSource
    {
      get
      {
        return this._imageSource;
      }
      set
      {
        this._imageSource = value;
        this.OnPropertyChanged(nameof (ImageSource));
      }
    }

    public ICommand ReadMoreCommand { get; }
  }
}
