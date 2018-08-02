// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.BoardingPassStorageService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

namespace Itx.Flex.Client.Service
{
  public class BoardingPassStorageService : IBoardingPassStorageService
  {
    private readonly IRegistryService _registryService;
    private readonly IWorkspaceStorageService _workspaceStorageService;
    private readonly IPinCodeStorageService _pinCodeStorageService;
    private readonly IHandInFileMetadataStorageService _handInFileMetadataStorageService;
    private readonly IHandInFieldIdStorageService _handInFieldIdStorageService;
    private const string BoardingPassKey = "BoardingPass";
    private const string PreviousBoardingPassKey = "PreviousBoardingPass";

    public BoardingPassStorageService(IRegistryService registryService, IWorkspaceStorageService workspaceStorageService, IPinCodeStorageService pinCodeStorageService, IHandInFileMetadataStorageService handInFileMetadataStorageService, IHandInFieldIdStorageService handInFieldIdStorageService)
    {
      this._registryService = registryService;
      this._workspaceStorageService = workspaceStorageService;
      this._pinCodeStorageService = pinCodeStorageService;
      this._handInFileMetadataStorageService = handInFileMetadataStorageService;
      this._handInFieldIdStorageService = handInFieldIdStorageService;
    }

    public bool HasExisting()
    {
      return !string.IsNullOrEmpty(this._registryService.GetValue("BoardingPass"));
    }

    public string GetExisting()
    {
      return this._registryService.GetValue("BoardingPass");
    }

    public void Store(string boardingPass)
    {
      string str1 = this._registryService.GetValue("PreviousBoardingPass");
      string str2 = this._registryService.GetValue("BoardingPass");
      if (!string.IsNullOrEmpty(str1) && str1 != boardingPass || !string.IsNullOrEmpty(str2) && str2 != boardingPass)
      {
        this._workspaceStorageService.Clear();
        this._pinCodeStorageService.Clear();
        this._handInFileMetadataStorageService.Clear();
        this._handInFieldIdStorageService.Clear();
      }
      this._registryService.SetValue("BoardingPass", boardingPass);
      this._registryService.ClearValue("PreviousBoardingPass");
    }

    public void Clear()
    {
      if (!this.HasExisting())
        return;
      this._registryService.SetValue("PreviousBoardingPass", this.GetExisting());
      this._registryService.ClearValue("BoardingPass");
    }
  }
}
