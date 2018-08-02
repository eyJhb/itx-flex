// Decompiled with JetBrains decompiler
// Type: Itx.Flex.Client.Service.WebClientService
// Assembly: Flex.Client, Version=3.8.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 56747C71-E9A4-4DB3-B21A-436758D0FC8C
// Assembly location: C:\Users\Stella\AppData\Local\Arcanic\ITX Flex\Flex.Client.exe

using System.Net;

namespace Itx.Flex.Client.Service
{
  public class WebClientService : IWebClientService
  {
    private readonly WebClient _webClient;

    public WebClientService(WebClient webClient)
    {
      this._webClient = webClient;
    }

    public byte[] DownloadData(string address)
    {
      return this._webClient.DownloadData(address);
    }

    public void DownloadFile(string url, string filepath)
    {
      this._webClient.DownloadFile(url, filepath);
    }
  }
}
